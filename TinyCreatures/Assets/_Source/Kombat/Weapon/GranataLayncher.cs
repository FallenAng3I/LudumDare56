using System.Collections;
using Unity.Mathematics;
using UnityEngine;
public class GrenadeLauncher : Aweapon
{
    private bool canShoot = true;
    public override void shoot()
    {
        GameObject newProjectile = Instantiate(bulletPrefab, firePoint.position, quaternion.identity);
        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * projectileSpeed;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }
    }
    private IEnumerator ShootCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(Rate);
        canShoot = true;
    }
}

