using UnityEngine;

namespace _Source.EnemySystem
{
    public class Cockroach : AEnemy
    {
        [SerializeField] private Animator bugAnim;

        public override void Update()
        {
            base.Update();
            if (isPlayerDetected)
            {
                bugAnim.SetTrigger("BugMove");
                MoveTowardsPlayer();
                if (IsPlayerInAttackRange() && CanAttack())
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }
}