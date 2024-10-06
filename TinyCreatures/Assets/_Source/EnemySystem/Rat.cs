using UnityEngine;

namespace _Source.EnemySystem
{
    public class Rat : AEnemy
    {
        protected override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
        }
    
        protected override void Attack()
        {
            // Логика ближней атаки
            Debug.Log("Rat performs a melee attack!");
        }

        protected override void Die()
        {
            base.Die();
            Debug.Log("Rat is dead.");
        }
    }
}