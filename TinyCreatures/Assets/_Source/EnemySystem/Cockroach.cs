using UnityEngine;

namespace _Source.EnemySystem
{
    public class Cockroach : AEnemy
    {
        protected override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
        }
    
        protected override void Attack()
        {
            // Логика ближней атаки
            Debug.Log("Cockroach performs a melee attack!");
        }

        protected override void Die()
        {
            base.Die();
            Debug.Log("Cockroach is dead.");
        }
    }
}