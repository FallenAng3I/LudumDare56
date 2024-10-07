using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Launcher : Aweapon
{
    private GameObject newProjectile;
    [SerializeField] private GameObject boom;
    [SerializeField] private TextMeshProUGUI ammoCount;
    private Transform boomPlace;

    private void Awake()
    {
        ammoCount.text = "1 / 1";
    }
    public override void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (canShoot)
            {
                newProjectile = Instantiate(bulletPrefab, firePoint.position, quaternion.identity);
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = firePoint.right * speedOfFly;
                //ammoCount.text = "0 / 1";
                currentAmmo--;
                StartCoroutine(ShootCoroutine());
                StartCoroutine(DestroyAfterDelay(newProjectile, bulletDestroyTime));
            }
        }
        else
        {
            Debug.Log("i need more bullets trdtrdtdr");

        }
    }
    private void Update()
    {
        if (canShoot)
        {
            ammoCount.text = "1 / 1";
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    
    private new IEnumerator DestroyAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
        boomPlace = newProjectile.GetComponent<Transform>();
        GameObject exp = Instantiate(boom, boomPlace.position, quaternion.identity);
        Destroy(exp, 1f);
    }
}