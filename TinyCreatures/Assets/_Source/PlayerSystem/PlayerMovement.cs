using _Source.PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]

sealed public class PlayerMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D rb;

    [HideInInspector] public StaminaController staminaController;

    private float speed;
    private float defaultSpeed;
    private float speedMultiplier;
    private float jumpForce;

    private PlayerControls gameInput;

    private bool isFacingRight;
    private float horizontal;

    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool sprinting = false;

    private bool jumpHeld = false;
    private bool hasJumped = false;

    private void Awake()
    {
        staminaController = GetComponent<StaminaController>();
        gameInput = new PlayerControls();
        speed = player.speed;
        defaultSpeed = speed;
        speedMultiplier = player.sprintMultiplier;
        jumpForce = player.jumpForce;
        //gameInput.Enable();
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if(!isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        HandleJump();
        HandleSprint();
    }

    private void HandleJump()
    {
        // Обработка прыжка, если пробел зажат и игрок на земле
        if (jumpHeld && IsGrounded() && !hasJumped)
        {
            staminaController.CanJump();  // Проверяем, хватает ли стамины на прыжок
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
            if (staminaController.playerStamina > 0) // Проверяем, есть ли достаточно стамины для спринта
            {
                speed = defaultSpeed * speedMultiplier; // Спринтуем
                staminaController.Sprinting(); // Уменьшаем стамину
            }
            else
            {
                // Если стамины недостаточно, отключаем спринт
                sprinting = false;
                speed = defaultSpeed;
            }
        }
        else
        {
            speed = defaultSpeed; // Возвращаем обычную скорость
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

        // Когда пробел отпущен
        if (context.canceled)
        {
            // Останавливаем прыжки
            jumpHeld = false;

            // Если игрок все еще движется вверх, уменьшаем его скорость (чтобы прыжок остановился плавно)
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
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
