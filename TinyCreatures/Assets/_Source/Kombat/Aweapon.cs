using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aweapon : MonoBehaviour , IShooting
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float projectileSpeed = 5f;

    public virtual void shoot()
    {
        
    }
}
