using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aweapon : MonoBehaviour , IShooting
{
    [SerializeField] protected float spreadAngle = 5f;
    [SerializeField] protected float bulletDestroyTime = 2f;
    [SerializeField, Range(1f, 25f)] protected int pelletCount;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float projectileSpeed = 5f;
    [SerializeField] protected float Rate;

    public virtual void shoot()
    {
        
    }
}
