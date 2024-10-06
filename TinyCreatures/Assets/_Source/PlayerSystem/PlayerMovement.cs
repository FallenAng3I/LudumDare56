using _Source.PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Player))]

sealed public class PlayerMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float jumpOnSlopeMultiplier = 1.5f;

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
    private float slopeSideAngle;

    private bool isFacingRight;
    private float horizontal;
    private bool isSliding; // ���� ����������
    private bool isOnSlope;
    private bool canWalkOnSlope;
    private Vector2 slopeNormal; // ������� �����������
    private RaycastHit2D slopeHit;

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
        if (!isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        MoveHandle();
        HandleJump();
        HandleSprint();
        SlopeCheck();
    }

    private void MoveHandle()
    {
        if (IsGrounded() && !isOnSlope)
        {
            newVelocity.Set(speed * horizontal, 0.0f);
            rb.velocity = newVelocity;
        }
        else if (IsGrounded() && isOnSlope && canWalkOnSlope)
        {
            newVelocity.Set(speed * slopeNormalPerp.x * -horizontal, speed * slopeNormalPerp.y * -horizontal);
            rb.velocity = newVelocity;
        }
        else if (!IsGrounded())
        {
            newVelocity.Set(speed * horizontal, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
        //SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer); ;
        
        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            slopeHit = hit;
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else 
        {
            canWalkOnSlope = true; 
        }

        if (isOnSlope && horizontal == 0.0f && canWalkOnSlope)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }

    private void HandleJump()
    {
        // ��������� ������, ���� ������ ����� � ����� �� �����
        if (jumpHeld && IsGrounded() && !hasJumped && slopeDownAngle <= maxSlopeAngle)
        {
            staminaController.CanJump();  // ���������, ������� �� ������� �� ������
            if (canJump)
            {
                newVelocity.Set(0.0f, 0.0f);
                rb.velocity = newVelocity;
                newForce.Set(0.0f, jumpForce);
                rb.AddForce(newForce, ForceMode2D.Impulse);
            }
        }

        // ���� ����� �� ��������� �����������, ���� ������� ������ maxSlopeAngle, ��������� ������
        if (jumpHeld && isOnSlope && slopeDownAngle > maxSlopeAngle)
        {
            staminaController.CanJump();  // ���������, ������� �� ������� �� ������
            if (canJump)
            {
                // ���������� ������� ����������� ��� ����������� ������
                Vector2 slopeNormal = slopeHit.normal;  // ������� ��������� �����������
                Vector2 jumpDirection = slopeNormal;  // ������ � ���������������� �����������

                // ��������� ������� �������� ����������, ����������� �
                Vector2 slideVelocity = rb.velocity;  // ������� �������� ����������
                float slideInfluenceFactor = 0.5f;  // ����������� ������� �������� ���������� (����� ���������)
                Vector2 adjustedSlideVelocity = slideVelocity * slideInfluenceFactor;

                // ����� ���� ������ � ������ ����������
                Vector2 totalJumpForce = (jumpDirection * jumpForce + adjustedSlideVelocity).normalized * jumpForce;

                // ��������� ������
                rb.velocity = Vector2.zero;  // �������� ������� �������� ����� �������
                rb.AddForce(totalJumpForce * jumpOnSlopeMultiplier, ForceMode2D.Impulse);  // ������ � ������ ����������
                hasJumped = true;
            }
        }

        if (IsGrounded() && rb.velocity.y <= 0f)
        {
            hasJumped = false;
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

    private void HandleSprint()
    {
        if (sprintHeld && staminaController.playerStamina > 0) // ���� ������ ������� ������ � ���� �������
        {
            if ((Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.y) > 0.1f) && canSprint)
            {
                staminaController.Sprinting(); // ��������� ������� ��� ��������
            }

            // ���� ������ �������, �� ������� ������������
            if (staminaController.playerStamina <= 0)
            {
                sprinting = false;
                canSprint = false;
                speed = defaultSpeed; // ���������� ������� ��������
            }
        }
        else if (!sprintHeld || staminaController.playerStamina <= 0) // ���� ������ �� ������ ��� ������� �����������
        {
            sprinting = false;
            canSprint = false;
            speed = defaultSpeed; // ���������� ������� ��������
        }

        if (!sprintHeld && (staminaController.playerStamina > 0f && staminaController.playerStamina <= 50f))
        {
            canSprint = false;
        }

        if (staminaController.playerStamina > 50f) // ��������, ������ �������� ��� >50% �������
        {
            canSprint = true;
        }

        // �������������� ��������: ���� ������ ������������ � ������� �������������� �� ������, ������������ ������
        if (canSprint && sprintHeld)
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

    private bool IsGrounded()
    {
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
    }
}
