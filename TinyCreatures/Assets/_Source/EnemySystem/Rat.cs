using System.Collections;
using UnityEngine;

namespace _Source.EnemySystem
{
    public class Rat : AEnemy
    {
        private bool scream;

        public override IEnumerator AttackCoroutine()
        {
            Debug.Log("Я БЕГИТЕ");
            yield return new WaitForSeconds(3f);
            Debug.Log("Даю");
            Attack();
            yield return new WaitForSeconds(1f / attackRate);
        }
    }
}