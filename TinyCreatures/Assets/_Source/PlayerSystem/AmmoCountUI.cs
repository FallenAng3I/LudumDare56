using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shotgunAmmo;

    private int maxShotgunAmmo;

    public void SetMaxAmmoCount(int maxAmmoCount)
    {
        maxShotgunAmmo = maxAmmoCount;
    }

    public void SetAmmoCount(int currentAmmo)
    {
        shotgunAmmo.text = $"{currentAmmo} / {maxShotgunAmmo}";
    }
}
