using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Параметры стамины")]
    public float playerStamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float jumpCost = 20f;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool sprinting = false;

    [Header("Параметры регенерации стамины")]
    [SerializeField, Range(0, 50)] private float staminaDrain = 0.5f;
    [SerializeField, Range(0, 50)] private float staminaRegen = 0.5f;

    [Header("Элементы UI")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private PlayerMovement playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        sprinting = playerController.sprinting;

        if (playerStamina >= maxStamina / 2)
        {
            playerController.canSprint = true;
        }
        if (playerStamina <= 0f)
        {
            playerController.canSprint = false;
        }

        //Sprinting();

        if (!sprinting)
        {
            if (playerStamina < maxStamina)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    sliderCanvasGroup.alpha = 1;
                    hasRegenerated = true;
                }
            }
        }

        //Debug.Log("Стамина = " + playerStamina + " спринт? " + sprinting + " можно спринтить? " + playerController.canSprint);
    }

    public void Sprinting()
    {
        if ((playerStamina >= 0))
        {
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                playerController.canSprint = false;
                hasRegenerated = false;
                sliderCanvasGroup.alpha = 0;
            }
        }
    }

    public void CanJump()
    {
        if (playerStamina >= (maxStamina * jumpCost / maxStamina))
        {
            playerController.canJump = true;
            playerStamina -= jumpCost;
            UpdateStamina(1);
        }
        else
        {
            playerController.canJump = false;
        }
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
