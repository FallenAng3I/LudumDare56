using System.Collections;
using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int health;                         // Здоровье игрока
        public float speed;                                          // Скорость игрока
        public float jumpForce;                                      // Сила прыжка    
        
        [SerializeField] private float invulnerabilityDuration; // Кулдаун неуязвимости 
        private bool isInvulnerable;

        public void TakeDamage(int damageAmount)
        {
            if (isInvulnerable) return;

            health -= damageAmount;
            Debug.Log("Player took damage: " + damageAmount);
            if (health <= 0)
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
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }
        
        private void Die()
        {
            Debug.Log("Player died.");
            Destroy(gameObject);
        }        
    }
}