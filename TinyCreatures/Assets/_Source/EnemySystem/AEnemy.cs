using UnityEngine;
using System.Collections;
using _Source.PlayerSystem;

namespace _Source.EnemySystem
{
    public abstract class AEnemy : MonoBehaviour
    {
        [SerializeField] public float health;               // Здоровье          
        [SerializeField] protected float speed;             // Скорость          
        [SerializeField] protected int damage;              // Урон              
        [SerializeField] protected float attackRange;       // Радиус атак       
        [SerializeField] protected float detectionRadius;   // Радиус видимости  
        [SerializeField] protected LayerMask playerLayer;   // Слой игрока       
        [SerializeField] protected float attackRate = 1f;   // Количество атак в секунду

        private Transform player;
        private bool isPlayerDetected;
        private bool isClimbing;

        private float attackTimer;

        private void Start() // Инициализация, чтоб проходить насквозь игрока и других
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

        protected virtual void DetectPlayer() // Обнаружение игрока
        {
            Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
            isPlayerDetected = detectedPlayer != null;
            
            if (isPlayerDetected)
            {
                player = detectedPlayer.transform;
            }
        }

        protected virtual void MoveTowardsPlayer() // Движение к игроку 
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            Vector2 direction = (player.position - transform.position).normalized;
            
            if (isClimbing)
            {
                direction.y = 1;
                direction.x = 0;
            }
            else
            {
                direction.y = 0;
                direction.x = Mathf.Sign(direction.x);
            }
            
            Vector2 newPosition = rb.position + direction * (speed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                isClimbing = true;
                
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            // Если противник покинул стену, сбрасываем флаг
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                isClimbing = false; // Сбрасываем флаг
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, -1); // Устанавливаем небольшую скорость вниз для спрыгивания
            }
        }

        protected virtual bool IsPlayerInAttackRange()  // Атака по радиусу
        {
            return Vector2.Distance(transform.position, player.position) <= attackRange;
        }

        protected virtual bool CanAttack() // Факт атаки
        {
            if (attackTimer <= 0f)
            {
                attackTimer = 1f / attackRate;
                return true;
            }
            attackTimer -= Time.deltaTime;
            return false;
        }

        protected virtual IEnumerator AttackCoroutine() // Кулдаун атаки
        {
            Attack();
            yield return new WaitForSeconds(1f / attackRate);
        }

        protected virtual void Attack() // Метод атаки
        {
            if (player.TryGetComponent(out Player playerComponent))
            {
                playerComponent.TakeDamage(damage);
            }
        }
        
        public virtual void TakeDamage(float amount) // Получение урона
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }        

        protected virtual void Die() // Умер
        {
            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected() // Гизма 
        {
            Gizmos.color = Color.yellow; // Гизма для обнаружения
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            
            Gizmos.color = Color.red; // Гизма для атаки
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
