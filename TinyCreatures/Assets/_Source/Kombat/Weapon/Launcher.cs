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
    [SerializeField] private Animator gunAnimator;
    private bool isAnim=false;

    [Header("Launcher sounds")]
    [SerializeField] private SoundFXManager soundFXManager;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip explodeSound;
    [SerializeField] private AudioClip latchOpenSound;
    [SerializeField, Range(0f, 1f)] private float launcherVolume = 0.2f;

    private void Awake()
    {
        ammoCount.text = "0 / 1";
    }
    public override void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (canShoot && isAnim==false )
            {
                isAnim = true;
                gunAnimator.SetTrigger("LaunchGren");
                newProjectile = Instantiate(bulletPrefab, firePoint.position, quaternion.identity);
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = firePoint.right * speedOfFly;
                ammoCount.text = "0 / 1";
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

    protected override IEnumerator ShootCoroutine()
    {
        soundFXManager.PlaySoundFXClip(shotSound, transform, launcherVolume);
        gunAnimator.SetTrigger("ReloadGren");
        yield return new WaitForSeconds(.8f);
        isAnim = false;
        yield return base.ShootCoroutine();
        gunAnimator.SetTrigger("IdleGren");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    
    private new IEnumerator DestroyAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
        soundFXManager.PlaySoundFXClip(explodeSound, transform, launcherVolume/2);
        boomPlace = newProjectile.GetComponent<Transform>();
        GameObject exp = Instantiate(boom, boomPlace.position, quaternion.identity);
        Destroy(exp, 1f);
    }
}