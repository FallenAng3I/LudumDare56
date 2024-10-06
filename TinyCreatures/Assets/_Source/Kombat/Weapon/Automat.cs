using UnityEngine;
using Random = UnityEngine.Random;

namespace _Source.Kombat
{ 
    public class Automat : Aweapon
    { 
        private void Update()
        {
            if (Input.GetAxis("Fire1") != 0)
            {
                Shoot();
            }
        }
        public override void Shoot()
        {
            if (canShoot)
            {
                Debug.Log("piy piy");
                for (int i = 0; i < pelletCount; i++)
                {
                    GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

                    float spread = Random.Range(-spreadAngle, spreadAngle);
                    Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);

                    Vector2 direction = (spreadRotation * firePoint.right).normalized;
                    rb.velocity = direction * speedOfFly;
                    StartCoroutine(DestroyAfterDelay(bulletInstance, bulletDestroyTime));
                }
                StartCoroutine(ShootCoroutine());
            }
        }
     
    }
}
