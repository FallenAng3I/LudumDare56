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
    [SerializeField] private float slopeCheckDistance;
    
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
    private float slopeDownAngle;
    private float slopeDownAngleOld;

    private bool isFacingRight;
    private float horizontal;
    private bool isSliding; // ���� ����������
    private bool isOnSlope;
    private Vector2 slopeNormal; // ������� �����������

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;
    
    private bool jumpHeld = false;
    private bool sprintHeld = false;
    private bool hasJumped = false;

    private void Awake()
    {
        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        staminaController = GetComponent<StaminaController>();
        player = GetComponent<Player>();

        colliderSize = cc.size;
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
        SlopeCheck();
        //HandleSlide();

        Debug.Log(Mathf.Abs(rb.velocity.x) + " " + Mathf.Abs(rb.velocity.y));
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
        SlopeCheckVertiacl(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }

    private void SlopeCheckVertiacl(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal);

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }

    //private void HandleSlide()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);

    //    if (hit && Mathf.Abs(hit.normal.x) > 0.1f) // ���������, ���� �� ������
    //    {
    //        isOnSlope = true;
    //        slopeNormal = hit.normal; // �������� ������� �����������

    //        // ������������ ����������� ���������� �� �������
    //        Vector2 slideDirection = new Vector2(slopeNormal.x, -slopeNormal.y);

    //        // �������� ���������� ������� �� ��������� �������
    //        float currentSlideSpeed = sprinting ? slideSpeed * speedMultiplier : slideSpeed;

    //        // ��������� ����������
    //        rb.velocity = new Vector2(slideDirection.x * currentSlideSpeed, rb.velocity.y);
    //    }
    //    else
    //    {
    //        isOnSlope = false;
    //    }
    //}

    private void HandleJump()
    {
        // ��������� ������, ���� ������ ����� � ����� �� �����
        if (jumpHeld && IsGrounded() && !hasJumped)
        {
            staminaController.CanJump();  // ���������, ������� �� ������� �� ������
            if (canJump)
            {
                newVelocity.Set(0.0f, 0.0f);
                rb.velocity = newVelocity;
                newForce.Set(0.0f, jumpForce);
                rb.AddForce(newForce, ForceMode2D.Impulse);
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //hasJumped = true;
            }
        }

        if (IsGrounded() && rb.velocity.y <= 0f)
        {
            hasJumped = false;
        }
    }

    private void HandleSprint()
    {
        if (sprintHeld && staminaController.playerStamina > 0) // ���� ������ ������� ������ � ���� �������
        {
            // ���������, ���� �� ����������� ��������, ����� ���������� ������ (��� ����� ���������� �����������)
            if (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.y) > 0.1f)
            {
                staminaController.Sprinting(); // ��������� ������� ��� ��������
            }

            // ���� ������ �������, �� ������� ������������
            if (staminaController.playerStamina <= 0)
            {
                sprinting = false;
                speed = defaultSpeed; // ���������� ������� ��������
            }
        }
        else if (!sprintHeld || staminaController.playerStamina <= 0) // ���� ������ �� ������ ��� ������� �����������
        {
            sprinting = false;
            speed = defaultSpeed; // ���������� ������� ��������
        }

        // �������������� ��������: ���� ������ ������������ � ������� �������������� �� ������, ������������ ������
        if (sprintHeld && staminaController.playerStamina > 50f) // ��������, ������ �������� ��� >50% �������
        {
            sprinting = true;
            speed = defaultSpeed * speedMultiplier; // ���������� �������� �������
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed && canSprint)
        {
            sprintHeld = true;
            speed = defaultSpeed * speedMultiplier; // ���������� ������
        }

        if (context.canceled)
        {
            sprintHeld = false; // ������������� ������ ��� ���������� ������
            speed = defaultSpeed; // ���������� ������� ��������
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
        //if (isOnSlope)
        //{
        //    return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        //}
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
        //rb.velocity = new
        horizontal = context.ReadValue<Vector2>().x;

        //if (isOnSlope && horizontal > 0f && slopeNormal.x > 0f) // ���� �� ������� � �������� ���� �����
        //{
        //    horizontal = 0; // ��������� �������� �����
        //}
        //else if (isOnSlope && horizontal < 0f && slopeNormal.x < 0f) // ���������� ��� ������ �������
        //{
        //    horizontal = 0;
        //}
    }
}
