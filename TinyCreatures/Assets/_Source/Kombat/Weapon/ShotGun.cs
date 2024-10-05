using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Aweapon
{
    public override void shoot()
    {
        Debug.Log("PAW PAW");
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
            
            float spread = Random.Range(-spreadAngle, spreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
            
            Vector2 direction = (spreadRotation * firePoint.right).normalized;
            rb.velocity = direction * projectileSpeed;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }
    }
}
