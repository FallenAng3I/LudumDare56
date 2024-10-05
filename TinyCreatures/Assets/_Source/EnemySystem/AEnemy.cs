using System;
using UnityEngine;
using System.Collections;
using _Source.PlayerSystem;

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
        [SerializeField] protected float attackRate = 1f;   // Количество атак в секунду

        protected Transform player;
        protected bool isPlayerDetected;

        private float attackTimer;

        private void Start()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"));
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
        }

        protected virtual void Update()
        {
            DetectPlayer();
            if (isPlayerDetected)
            {
                MoveTowardsPlayer();
                if (IsPlayerInAttackRange() && CanAttack())
                {
                    StartCoroutine(AttackCoroutine());
                }
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

        protected virtual bool IsPlayerInAttackRange()
        {
            return Vector2.Distance(transform.position, player.position) <= attackRange;
        }

        protected virtual bool CanAttack()
        {
            if (attackTimer <= 0f)
            {
                attackTimer = 1f / attackRate;
                return true;
            }
            attackTimer -= Time.deltaTime;
            return false;
        }

        protected virtual IEnumerator AttackCoroutine()
        {
            Attack();
            yield return new WaitForSeconds(1f / attackRate);
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
            if (player.TryGetComponent<Player>(out Player playerComponent))
            {
                playerComponent.TakeDamage(damage);
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow; // Гизма для обнаружения
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            
            Gizmos.color = Color.red; // Гизма для атаки
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
