using _Source.Kombat;
using UnityEngine;

public class SwitchGuns : MonoBehaviour
{
    [SerializeField] private SwitchWeaponIcons switchImgs;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private Automat automat;
    [SerializeField] private Launcher grenade;
    [SerializeField] private Animator gunAnimator;

    private void Start()
    {
        shotgun = FindObjectOfType<Shotgun>();
        automat = FindObjectOfType<Automat>();
        grenade = FindObjectOfType<Launcher>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            automat.GetComponent<Automat>().enabled = true;
            shotgun.GetComponent<Shotgun>().enabled = false;
            grenade.GetComponent<Launcher>().enabled = false;
            switchImgs.EnableFlamethrowerImg();
            gunAnimator.SetTrigger("Flame");
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            automat.GetComponent<Automat>().enabled = false;
            shotgun.GetComponent<Shotgun>().enabled = true;
            grenade.GetComponent<Launcher>().enabled = false;
            switchImgs.EnableShotgunImg();
            gunAnimator.SetTrigger("Shot");
       
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            automat.GetComponent<Automat>().enabled = false;
            shotgun.GetComponent<Shotgun>().enabled = false;
            grenade.GetComponent<Launcher>().enabled = true;
            switchImgs.EnableGrenadelauncherImg();
            gunAnimator.SetTrigger("Gren");
        }
    }
}
