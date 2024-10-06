using UnityEngine;

namespace _Source.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int health;        // Здоровье игрока
        public float speed;                         // Скорость игрока
        public float sprintMultiplier = 1.5f;       // Множитель скорости от спринта
        public float jumpForce;                     // Сила прыжка
        public float slideSpeed = 5f;               // Скорость скольжения по наклонной
        //[SerializeField] private int damage;        // Урон игрока

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