using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int health;       // Здоровье игрока
        [SerializeField] private int damage;        // Урон игрока
        [SerializeField] private float speed;       // Скорость игрока

        public void TakeDamage(int damageAmount)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died.");
            // Логика смерти игрока
            Destroy(gameObject);
        }
    }
}