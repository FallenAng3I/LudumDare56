using UnityEngine;

namespace _Source.EnemySystem
{
    public class Bug : AEnemy
    {
        protected override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
        }

        protected override void Attack()
        {
            // Логика ближней атаки
            Debug.Log("Bug performs a melee attack!");
        }

        protected override void Die()
        {
            base.Die();
            Debug.Log("Bug is dead.");
        }
    }
}