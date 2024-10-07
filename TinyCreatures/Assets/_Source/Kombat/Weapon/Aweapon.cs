using System.Collections;
using UnityEngine;

public class Aweapon : MonoBehaviour , IShooting
{
    [SerializeField] protected Transform firePoint;                 // Точка стрельбы       
    [SerializeField] protected GameObject bulletPrefab;             // Префаб пули          
    [SerializeField] public int currentAmmo;                     // Вместимость магазина 
    [SerializeField] protected float Rate;                          // Скорострельность     
    [SerializeField, Range(1f, 25f)] protected int pelletCount;     // Количество пуль      
    [SerializeField] protected float bulletDestroyTime = 2f;        // Срок жизни пули      
    [SerializeField] protected float spreadAngle = 5f;              // Разброс ???          
    [SerializeField] protected float speedOfFly = 5f;               // Скорость полёта пули 
    
    [SerializeField] protected bool canShoot;

    public virtual void Shoot()
    {
        
    }
    protected IEnumerator ShootCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(Rate);
        canShoot = true;
    }
    protected IEnumerator DestroyAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
