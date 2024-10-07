using _Source.Kombat;
using UnityEngine;

public class SwitchGuns : MonoBehaviour
{
    [SerializeField] private SwitchWeaponIcons switchImgs;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private Automat automat;
    [SerializeField] private Launcher grenade;
    [SerializeField] private Animator gunAnimator;
    

    [Header("Switch gun sounds")]
    [SerializeField] private SoundFXManager soundFXManager;
    [SerializeField] private AudioClip shotgunPullSound;
    [SerializeField] private AudioClip grenadelauncherPullSound;
    [SerializeField] private AudioClip flamethrowerPullSound;
    [SerializeField] private float switchSoundVolume = 0.5f;
    
    private enum WeaponType { None, Flamethrower, Shotgun, GrenadeLauncher }
    private WeaponType currentWeapon = WeaponType.Flamethrower;  // ������� �������� ������
    private void Start()
    {
        shotgun = FindObjectOfType<Shotgun>();
        automat = FindObjectOfType<Automat>();
        grenade = FindObjectOfType<Launcher>();
        gunAnimator = FindObjectOfType<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != WeaponType.Flamethrower)
        {
            // ����������� �� ������
            SwitchToWeapon(WeaponType.Flamethrower, flamethrowerPullSound);
            switchImgs.EnableFlamethrowerImg();
            gunAnimator.SetTrigger("IdleFlame");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != WeaponType.Shotgun)
        {
            // ����������� �� ��������
            SwitchToWeapon(WeaponType.Shotgun, shotgunPullSound);
            switchImgs.EnableShotgunImg();
            gunAnimator.SetTrigger("IdleShot");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeapon != WeaponType.GrenadeLauncher)
        {
            // ����������� �� ���������
            SwitchToWeapon(WeaponType.GrenadeLauncher, grenadelauncherPullSound);
            switchImgs.EnableGrenadelauncherImg();
            gunAnimator.SetTrigger("IdleGren");
        }
    }

    private void SwitchToWeapon(WeaponType newWeapon, AudioClip pullSound)
    {
        // ��������� ������� ������
        DisableAllWeapons();

        // �������� ����� ������ � ����������� �� ����
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

        // ����������� ���� ��� ���������� ������
        soundFXManager.PlaySoundFXClip(pullSound, transform, switchSoundVolume);

        // ��������� ������� ������
        currentWeapon = newWeapon;
    }

        private void DisableAllWeapons()
    {
        automat.GetComponent<Automat>().enabled = false;
        shotgun.GetComponent<Shotgun>().enabled = false;
        grenade.GetComponent<Launcher>().enabled = false;
    }
}
