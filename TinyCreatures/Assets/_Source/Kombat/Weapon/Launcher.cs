using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Launcher : Aweapon
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
            rb.velocity = firePoint.right * speedOfFly;
            StartCoroutine(ShootCoroutine());
            //StartCoroutine(DestroyAfterDelay(newProjectile, bulletDestroyTime));
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    
    public void DetonateGrenade()
    {
        boomPlace = newProjectile.GetComponent<Transform>();
        GameObject exp = Instantiate(boom, boomPlace.position, quaternion.identity);
        Destroy(exp, 1f);
    }
}
