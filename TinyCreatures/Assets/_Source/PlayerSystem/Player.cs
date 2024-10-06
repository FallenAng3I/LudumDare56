using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int maxHealth;        // Здоровье игрока
        public float speed;                         // Скорость игрока
        public float sprintMultiplier = 1.5f;       // Множитель скорости от спринта
        public float jumpForce;                     // Сила прыжка

        public HealthBar healthBar;

        private int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            healthBar.SetHealth(currentHealth);
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