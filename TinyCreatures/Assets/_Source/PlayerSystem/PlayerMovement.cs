using _Source.PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Player))]

sealed public class PlayerMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [HideInInspector] public StaminaController staminaController;
    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool sprinting = false;

    private Player player;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private float speed;
    private float defaultSpeed;
    private float speedMultiplier;
    private float jumpForce;
    private float slideSpeed; // �������� ����������
    private float slopeAngle; // ���� ������� �����������

    private bool isFacingRight;
    private float horizontal;
    private bool isSliding; // ���� ����������
    private bool isOnSlope; // ���� ���������� �� ��������� �����������
    private Vector2 slopeNormal; // ������� �����������
    
    private bool jumpHeld = false;
    private bool hasJumped = false;

    private void Awake()
    {
        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        staminaController = GetComponent<StaminaController>();
        player = GetComponent<Player>();
        speed = player.speed;
        defaultSpeed = speed;
        speedMultiplier = player.sprintMultiplier;
        jumpForce = player.jumpForce;
        slideSpeed = player.slideSpeed;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);


        if (!isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        HandleJump();
        HandleSprint();
        HandleSlide();

        Debug.Log(Mathf.Abs(rb.velocity.x) + " " + Mathf.Abs(rb.velocity.y));
    }

    private void HandleSlide()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);

        if (hit && Mathf.Abs(hit.normal.x) > 0.1f) // ���������, ���� �� ������
        {
            isOnSlope = true;
            slopeNormal = hit.normal; // �������� ������� �����������

            // ������������ ����������� ���������� �� �������
            Vector2 slideDirection = new Vector2(slopeNormal.x, -slopeNormal.y);

            // �������� ���������� ������� �� ��������� �������
            float currentSlideSpeed = sprinting ? slideSpeed * speedMultiplier : slideSpeed;

            // ��������� ����������
            rb.velocity = new Vector2(slideDirection.x * currentSlideSpeed, rb.velocity.y);
        }
        else
        {
            isOnSlope = false;
        }
    }

    private void HandleJump()
    {
        // ��������� ������, ���� ������ ����� � ����� �� �����
        if (jumpHeld && IsGrounded() && !hasJumped)
        {
            staminaController.CanJump();  // ���������, ������� �� ������� �� ������
            if (canJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                hasJumped = true;
            }
        }

        if (IsGrounded() && rb.velocity.y <= 0f)
        {
            hasJumped = false;
        }
    }

    private void HandleSprint()
    {
        if (sprinting)
        {
            if (staminaController.playerStamina > 0) // ���������, ���� �� ���������� ������� ��� �������
            {
                speed = defaultSpeed * speedMultiplier; // ���������
                if (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.y) > 0.1f)
                {
                    staminaController.Sprinting(); // ��������� �������
                }
            }
            else
            {
                // ���� ������� ������������, ��������� ������
                sprinting = false;
                speed = defaultSpeed;
            }
        }
        else
        {
            speed = defaultSpeed; // ���������� ������� ��������
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed && canSprint)
        {
            sprinting = true;
            speed *= speedMultiplier;
        }

        if (context.canceled)
        {
            sprinting = false;
            //speed = defaultSpeed;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpHeld = true;
        }

        // ����� ������ �������
        if (context.canceled)
        {
            // ������������� ������
            jumpHeld = false;

            // ���� ����� ��� ��� �������� �����, ��������� ��� �������� (����� ������ ����������� ������)
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
    }

    private bool IsGrounded()
    {
        if (isOnSlope)
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        }
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;

        if (isOnSlope && horizontal > 0f && slopeNormal.x > 0f) // ���� �� ������� � �������� ���� �����
        {
            horizontal = 0; // ��������� �������� �����
        }
        else if (isOnSlope && horizontal < 0f && slopeNormal.x < 0f) // ���������� ��� ������ �������
        {
            horizontal = 0;
        }
    }
}
