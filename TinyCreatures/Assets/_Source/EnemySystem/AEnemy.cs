using UnityEngine;

namespace _Source.EnemySystem
{
    public abstract class AEnemy : MonoBehaviour
    {
        [SerializeField] protected int health;              // Здоровье          
        [SerializeField] protected float speed;             // Скорость          
        [SerializeField] protected int damage;              // Урон              
        [SerializeField] protected float attackRange;       // Радиус атак       
        [SerializeField] protected float detectionRadius;   // Радиус видимости  
        [SerializeField] protected LayerMask playerLayer;   // Слой игрока       
        protected Transform player;
        protected bool isPlayerDetected;

        protected virtual void Update()
        {
            DetectPlayer();
            if (isPlayerDetected)
            {
                MoveTowardsPlayer();
            }
        }

        protected virtual void DetectPlayer()
        {
            Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
            isPlayerDetected = detectedPlayer != null;
            
            if (isPlayerDetected)
            {
                player = detectedPlayer.transform;
            }
        }

        protected virtual void MoveTowardsPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * (speed * Time.deltaTime);
        }

        protected virtual void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        protected virtual void Attack()
        {
            // Логика атаки
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}
