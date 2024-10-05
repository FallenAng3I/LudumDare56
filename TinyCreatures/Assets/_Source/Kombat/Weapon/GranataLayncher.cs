using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GranataLayuncher : Aweapon
{
    private GameObject newProjectile;
    [SerializeField] private GameObject boom;
    private Transform boomPlace;
    public override void Shoot()
    {
        if (canShoot)
        {
            newProjectile = Instantiate(bulletPrefab, firePoint.position, quaternion.identity);
            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * projectileSpeed;
            StartCoroutine(ShootCoroutine());
            StartCoroutine(DestroyAfterDelay(newProjectile, bulletDestroyTime));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    
    private IEnumerator DestroyAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
        boomPlace = newProjectile.GetComponent<Transform>();
        GameObject exp = Instantiate(boom, boomPlace.position, quaternion.identity);
        Destroy(exp, 1f);
    }
}

