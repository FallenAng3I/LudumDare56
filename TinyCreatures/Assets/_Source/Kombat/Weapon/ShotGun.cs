using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shotgun : Aweapon
{
    [SerializeField] private AmmoCountUI ammoCountUI;
    [SerializeField] private int maxAmmoCount = 8;

    private bool reloading = false;

    public void Awake()
    {
<<<<<<< Updated upstream
        currentAmmo = maxAmmoCount;
        ammoCountUI.SetMaxAmmoCount(maxAmmoCount);
=======
  //      currentAmmo = maxAmmoCount;
  //        ammoCountUI.SetMaxAmmoCount(maxAmmoCount);
>>>>>>> Stashed changes
    }

    public override void Shoot()
    {
        if (currentAmmo > 0 && reloading == false)
        {
            if (canShoot)
            {
                Debug.Log("PAW PAW");
                for (int i = 0; i < pelletCount; i++)
                {
                    GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, quaternion.identity);
                    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

                    float spread = Random.Range(-spreadAngle, spreadAngle);
                    Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);

                    Vector2 direction = (spreadRotation * firePoint.right).normalized;
                    rb.velocity = direction * speedOfFly;
                    StartCoroutine(DestroyAfterDelay(bulletInstance, bulletDestroyTime));
                }
                currentAmmo--;
                StartCoroutine(ShootCoroutine());
            }
        }
        else
        {
            StartCoroutine(Reload());
        }
    }
    private void Update()
    {
        ammoCountUI.SetAmmoCount(currentAmmo);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(3f);
        currentAmmo = +maxAmmoCount;
        reloading = false;
    }
}
