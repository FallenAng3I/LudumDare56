using UnityEngine;
using Random = UnityEngine.Random;

namespace _Source.Kombat
{ 
    public class Automat : Aweapon
    {
        [Header("Flamethrower sounds")]
        [SerializeField] private SoundFXManager soundFXManager;
        [SerializeField] private AudioClip startFireSound; // Звук начала огня
        [SerializeField] private AudioClip loopFireSound;  // Звук для зацикливания
        [SerializeField, Range(0f, 1f)] private float flamethrowerVolume;

        private bool isFiring = false;
        private AudioSource loopAudioSource;

        private void Update()
        {
            if (Input.GetAxis("Fire1") != 0)
            {
                if (!isFiring)
                {
                    StartFlamethrower();
                }
                Shoot();
            }
            else if (isFiring)
            {
                StopFlamethrower();
            }
        }

        private void StartFlamethrower()
        {
            isFiring = true;

            // Проигрываем звук начала огня через SoundFXManager
            soundFXManager.PlaySoundFXClip(startFireSound, transform, flamethrowerVolume);

            // Запускаем зацикленный звук огня через SoundFXManager
            Invoke(nameof(StartLoopingFireSound), startFireSound.length);
        }

        private void StartLoopingFireSound()
        {
            if (isFiring)
            {
                loopAudioSource = soundFXManager.PlayLoopingSoundFXClip(loopFireSound, transform, flamethrowerVolume);
            }
        }

        private void StopFlamethrower()
        {
            isFiring = false;

            // Останавливаем зацикленный звук через SoundFXManager
            soundFXManager.StopLoopingSoundFXClip(loopAudioSource);
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
