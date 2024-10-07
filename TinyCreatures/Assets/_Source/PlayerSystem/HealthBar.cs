using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFillUI = null;

    public void SetMaxHealth(int health)
    {
        healthFillUI.fillAmount = health;
    }
    public void SetHealth(int health, int maxHealth)
    {
        healthFillUI.fillAmount = (float)health / maxHealth;
    }
}
