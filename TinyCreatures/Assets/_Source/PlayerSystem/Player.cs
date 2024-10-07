using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        //[SerializeField] private SwitchPlayerIcon playerIcon;
        //[SerializeField] private HealthBar healthBar;
        [SerializeField] private int maxHealth;        // Здоровье игрока
        public float speed;                         // Скорость игрока
        public float sprintMultiplier = 1.5f;       // Множитель скорости от спринта
        public float jumpForce;                     // Сила прыжка

        private int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
            //playerIcon.SwitchIcon(currentHealth);
            //healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            //playerIcon.SwitchIcon(currentHealth);
//healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died.");
            Destroy(gameObject);
        }
    }
}