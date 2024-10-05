using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Source.Kombat
{ 
    public class Automat : Aweapon
    { 
        private bool canShoot=true;
        private void Update()
        {
            if (Input.GetAxis("Fire1") != 0)
            {
                shoot();
            }
        }
        public override void shoot()
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
                    rb.velocity = direction * projectileSpeed;
                    StartCoroutine(DestroyAfterDelay(bulletInstance, bulletDestroyTime));
                }
                StartCoroutine(ShootCoroutine());
            }
        }
        private IEnumerator ShootCoroutine()
        {
            canShoot = false;
            yield return new WaitForSeconds(Rate);
            canShoot = true;
        }
        private IEnumerator DestroyAfterDelay(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(bullet);
        }
    }
}
