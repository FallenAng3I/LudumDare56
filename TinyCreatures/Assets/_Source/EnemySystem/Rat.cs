using System.Collections;
using UnityEngine;

namespace _Source.EnemySystem
{
    public class Rat : AEnemy
    {
        private bool coroutineRunning = false;
        [SerializeField] private Transform rat;
        [SerializeField] private GameObject bulletPrefab; // Префаб пули
        [SerializeField] private Transform firePoint; // Место, откуда будет выпущена пуля
        [SerializeField] private Platform platform; // Ссылка на платформу с игроком

        public override void Update()
        {
            platform = FindObjectOfType<Platform>();
            base.Update();
            if (base.player != null)
            {
                float distanceToPlayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(base.player.position.x, base.player.position.y));
                Debug.Log("Расстояние до игрока: " + distanceToPlayer);
                if (base.player.position.y > rat.position.y)
                {
                    if (!coroutineRunning) // Проверяем, что корутина не запущена
                    {
                        Debug.Log("I have Highground");
                        StartCoroutine(demolish());
                        coroutineRunning = true; // Устанавливаем флаг, что корутина запущена
                    }
                }
                else
                {
                    Debug.Log("I win skywaker");
                    if (IsPlayerInAttackRange() && CanAttack())
                    {
                        StartCoroutine(AttackCoroutine());
                    }
                }
            }
        }

        public override IEnumerator AttackCoroutine()
        {
            Debug.Log("Я БЕГИТЕ");
            yield return new WaitForSeconds(3f);
            Debug.Log("Даю");
            Attack();
            yield return new WaitForSeconds(1f / attackRate);
        }

        public  IEnumerator demolish()
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("начинаю ломать платформу");
            DemolishPlatform();
            coroutineRunning = false;
        }

        private void DemolishPlatform()
        {
            if (platform != null)
            {
                GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Chesse chesse = bulletObject.GetComponent<Chesse>();
                chesse.Target = platform.transform; 
            }
        }
    }
}