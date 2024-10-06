using _Source.Kombat;
using UnityEngine;

public class SwitchGuns : MonoBehaviour
{
    [SerializeField] private SwitchWeaponIcons switchImgs;
    public Shotgun shotgun;
    public Automat automat;
    public Launcher grenade;

    private void Start()
    {
        shotgun = FindObjectOfType<Shotgun>();
        automat = FindObjectOfType<Automat>();
        grenade = FindObjectOfType<Launcher>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            automat.GetComponent<Automat>().enabled = true;
            shotgun.GetComponent<Shotgun>().enabled = false;
            grenade.GetComponent<Launcher>().enabled = false;
            switchImgs.EnableFlamethrowerImg();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            automat.GetComponent<Automat>().enabled = false;
            shotgun.GetComponent<Shotgun>().enabled = true;
            grenade.GetComponent<Launcher>().enabled = false;
            switchImgs.EnableShotgunImg();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            automat.GetComponent<Automat>().enabled = false;
            shotgun.GetComponent<Shotgun>().enabled = false;
            grenade.GetComponent<Launcher>().enabled = true;
            switchImgs.EnableGrenadelauncherImg();
        }
    }
}
