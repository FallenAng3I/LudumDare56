using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeaponIcons : MonoBehaviour
{
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject flamethrower;
    [SerializeField] private GameObject grenadelauncher;

    public void EnableShotgunImg()
    {
        shotgun.SetActive(true);
        flamethrower.SetActive(false);
        grenadelauncher.SetActive(false);
    }
    public void EnableFlamethrowerImg()
    {
        shotgun.SetActive(false);
        flamethrower.SetActive(true);
        grenadelauncher.SetActive(false);
    }
    public void EnableGrenadelauncherImg()
    {
        shotgun.SetActive(false);
        flamethrower.SetActive(false);
        grenadelauncher.SetActive(true);
    }

}
