using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Source.EnemySystem
{
    public class Bug : AEnemy
    {
        [SerializeField] private Animator bugAnim;

        public override void Update()
        {
            base.Update();
            if (isPlayerDetected)
            {
                bugAnim.SetTrigger("KlopMove");
                MoveTowardsPlayer();
                if (IsPlayerInAttackRange() && CanAttack())
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }
}