using _Source.Kombat;
using UnityEngine;

public class SwitchGuns : MonoBehaviour
{
    [SerializeField] private SwitchWeaponIcons switchImgs;
    public Shotgun shotgun;
    public Automat automat;
    public Launcher grenade;

    [Header("Switch gun sounds")]
    [SerializeField] private SoundFXManager soundFXManager;
    [SerializeField] private AudioClip shotgunPullSound;
    [SerializeField] private AudioClip grenadelauncherPullSound;
    [SerializeField] private AudioClip flamethrowerPullSound;
    [SerializeField] private float switchSoundVolume = 0.5f;

    private enum WeaponType { None, Flamethrower, Shotgun, GrenadeLauncher }
    private WeaponType currentWeapon = WeaponType.Flamethrower;  // Текущее активное оружие

    private void Start()
    {
        shotgun = FindObjectOfType<Shotgun>();
        automat = FindObjectOfType<Automat>();
        grenade = FindObjectOfType<Launcher>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != WeaponType.Flamethrower)
        {
            // Переключаем на огнемёт
            SwitchToWeapon(WeaponType.Flamethrower, flamethrowerPullSound);
            switchImgs.EnableFlamethrowerImg();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != WeaponType.Shotgun)
        {
            // Переключаем на дробовик
            SwitchToWeapon(WeaponType.Shotgun, shotgunPullSound);
            switchImgs.EnableShotgunImg();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeapon != WeaponType.GrenadeLauncher)
        {
            // Переключаем на гранатомёт
            SwitchToWeapon(WeaponType.GrenadeLauncher, grenadelauncherPullSound);
            switchImgs.EnableGrenadelauncherImg();
        }
    }

    private void SwitchToWeapon(WeaponType newWeapon, AudioClip pullSound)
    {
        // Отключаем текущее оружие
        DisableAllWeapons();

        // Включаем новое оружие в зависимости от типа
        switch (newWeapon)
        {
            case WeaponType.Flamethrower:
                automat.GetComponent<Automat>().enabled = true;
                break;
            case WeaponType.Shotgun:
                shotgun.GetComponent<Shotgun>().enabled = true;
                break;
            case WeaponType.GrenadeLauncher:
                grenade.GetComponent<Launcher>().enabled = true;
                break;
        }

        // Проигрываем звук для выбранного оружия
        soundFXManager.PlaySoundFXClip(pullSound, transform, switchSoundVolume);

        // Обновляем текущее оружие
        currentWeapon = newWeapon;
    }

        private void DisableAllWeapons()
    {
        automat.GetComponent<Automat>().enabled = false;
        shotgun.GetComponent<Shotgun>().enabled = false;
        grenade.GetComponent<Launcher>().enabled = false;
    }
}
