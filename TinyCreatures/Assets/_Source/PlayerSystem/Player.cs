using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
<<<<<<< Updated upstream
        [SerializeField] private SwitchPlayerIcon playerIcon;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private int maxHealth;        // Здоровье игрока
=======
        //[SerializeField] private SwitchPlayerIcon playerIcon;
        //[SerializeField] private HealthBar healthBar;
        [SerializeField] public int maxHealth;        // Здоровье игрока
>>>>>>> Stashed changes
        public float speed;                         // Скорость игрока
        public float sprintMultiplier = 1.5f;       // Множитель скорости от спринта
        public float jumpForce;                     // Сила прыжка

        public int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
            playerIcon.SwitchIcon(currentHealth);
            healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
<<<<<<< Updated upstream
            playerIcon.SwitchIcon(currentHealth);
            healthBar.SetHealth(currentHealth);
=======
            //playerIcon.SwitchIcon(currentHealth);
            //healthBar.SetHealth(currentHealth);
>>>>>>> Stashed changes
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