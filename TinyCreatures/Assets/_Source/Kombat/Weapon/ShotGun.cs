using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shotgun : Aweapon
{
    [SerializeField] private AmmoCountUI ammoCountUI;
    [SerializeField] private int maxAmmoCount = 8;
    [SerializeField] private Animator gunAnimator;

    [Header("Shotgun sounds")]
    [SerializeField] private SoundFXManager soundFXManager;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip pumpSound;
    [SerializeField] private AudioClip loadShellSound;
    [SerializeField, Range(0f, 1f)] private float shotGunVolume;

    private bool isAnim=false;

    private bool reloading = false;

    public void Awake()
    {
        currentAmmo = maxAmmoCount;
        ammoCountUI.SetMaxAmmoCount(maxAmmoCount);
    }

    public override void Shoot()
    {
        if (currentAmmo > 0 && reloading == false)
        {
            if (canShoot&& isAnim==false)
            {
                StartCoroutine(ShotSoundCoroutine());
                isAnim = true;
                gunAnimator.SetTrigger("Shoot");
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
            gunAnimator.SetTrigger("ReloadShot");
        }
    }
    private void Update()
    {
        ammoCountUI.SetAmmoCount(currentAmmo);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.R) && reloading==false)
        {
            StartCoroutine(Reload());
            gunAnimator.SetTrigger("ReloadShot");
        }
    }

    private IEnumerator Reload()
    {
        reloading = true;

        StartCoroutine(HandleReloadSound());

        yield return new WaitForSeconds(1.8f);
        currentAmmo = +maxAmmoCount;
        reloading = false;
        gunAnimator.SetTrigger("IdleShot");
    }

    protected override IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(1f);
        isAnim = false;
        gunAnimator.SetTrigger("IdleShot");
        yield return base.ShootCoroutine();
    }

    private IEnumerator ShotSoundCoroutine()
    {
        soundFXManager.PlaySoundFXClip(shotSound, transform, shotGunVolume);
        yield return new WaitForSeconds(shotSound.length);
        soundFXManager.PlaySoundFXClip(pumpSound, transform, shotGunVolume);
    }

    private IEnumerator HandleReloadSound()
    {
        float reloadSoundInterval = loadShellSound.length; // Интервал между звуками
        float elapsedTime = 0f; // Счётчик времени для контроля интервала

        while (reloading) // Пока идёт перезарядка
        {
            // Проигрываем звук, если прошёл интервал между ними
            if (elapsedTime >= reloadSoundInterval)
            {
                soundFXManager.PlaySoundFXClip(loadShellSound, transform, shotGunVolume);
                elapsedTime = 0f; // Сбрасываем таймер после проигрывания звука
            }

            // Увеличиваем таймер на время, прошедшее с последнего кадра
            elapsedTime += Time.deltaTime;

            // Ждём один кадр перед следующим циклом
            yield return null;
        }
    }

}
