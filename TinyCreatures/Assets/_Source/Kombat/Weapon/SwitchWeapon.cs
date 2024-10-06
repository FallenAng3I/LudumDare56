using _Source.Kombat;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public ShotGun shotgun;
    public Automat automat;
    public GranataLayuncher grenade;

    private void Start()
    {
        shotgun = FindObjectOfType<ShotGun>();
        automat = FindObjectOfType<Automat>();
        grenade = FindObjectOfType<GranataLayuncher>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            automat.GetComponent<Automat>().enabled = true;
            shotgun.GetComponent<ShotGun>().enabled = false;
            grenade.GetComponent<GranataLayuncher>().enabled = false;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            automat.GetComponent<Automat>().enabled = false;
            shotgun.GetComponent<ShotGun>().enabled = true;
            grenade.GetComponent<GranataLayuncher>().enabled = false;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            automat.GetComponent<Automat>().enabled = false;
            shotgun.GetComponent<ShotGun>().enabled = false;
            grenade.GetComponent<GranataLayuncher>().enabled = true;
        }
    }
}
