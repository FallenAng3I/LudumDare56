using System.Collections;
using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SwitchPlayerIcon playerIcon;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] public int maxHealth = 100;            // Здоровье игрока               
        public int currentHealth;                         // Актуальное здоровье игрока    
        public float speed;                               // Скорость игрока               
        public float sprintMultiplier = 1.5f;             // Множитель скорости от спринта 
        public float jumpForce;                           // Сила прыжка                   
        
        [SerializeField] private float invulnerabilityCD; // Кулдаун неуязвимости          
        private bool isInvulnerable;

        private void Start()
        {
            currentHealth = maxHealth;
            playerIcon.SwitchIcon(currentHealth);
            healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damageAmount)
        {
            if (isInvulnerable) return;
            currentHealth -= damageAmount;
            

            playerIcon.SwitchIcon(currentHealth);
            healthBar.SetHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityCoroutine());
            }
        }
        
        private IEnumerator InvulnerabilityCoroutine()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityCD);
            isInvulnerable = false;
        }

        private void Die()
        {
            Debug.Log("Player died.");
            Destroy(gameObject);
        }
    }
}