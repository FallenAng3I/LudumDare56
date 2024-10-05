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
    private float slideSpeed; // Скорость скольжения

    private bool isFacingRight;
    private float horizontal;
    private bool isSliding; // Флаг скольжения
    private bool isOnSlope; // Флаг нахождения на наклонной поверхности
    private Vector2 slopeNormal; // Нормаль поверхности

    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool sprinting = false;

    private bool jumpHeld = false;
    private bool hasJumped = false;

    private void Awake()
    {
        staminaController = GetComponent<StaminaController>();
        speed = player.speed;
        defaultSpeed = speed;
        speedMultiplier = player.sprintMultiplier;
        jumpForce = player.jumpForce;
        slideSpeed = player.slideSpeed;
    }

    private void Update()
    {
        if (!isOnSlope) // Только если не на наклонной поверхности
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

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
    }

    private void HandleSlide()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);

        if (hit && Mathf.Abs(hit.normal.x) > 0.1f) // Проверяем, есть ли наклон
        {
            isOnSlope = true;
            slopeNormal = hit.normal; // Получаем нормаль поверхности

            // Рассчитываем направление скольжения по наклону
            Vector2 slideDirection = new Vector2(slopeNormal.x, -slopeNormal.y);

            // Скорость скольжения зависит от состояния спринта
            float currentSlideSpeed = sprinting ? slideSpeed * speedMultiplier : slideSpeed;

            // Добавляем скольжение
            rb.velocity = new Vector2(slideDirection.x * currentSlideSpeed, rb.velocity.y);
        }
        else
        {
            isOnSlope = false;
        }

        Debug.Log("На наклонной? " + isOnSlope + " на земле? " + IsGrounded());
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

        if (isOnSlope && horizontal > 0f && slopeNormal.x > 0f) // Если на наклоне и пытается идти вверх
        {
            horizontal = 0; // Запрещаем движение вверх
        }
        else if (isOnSlope && horizontal < 0f && slopeNormal.x < 0f) // Аналогично для другой стороны
        {
            horizontal = 0;
        }
    }
}
