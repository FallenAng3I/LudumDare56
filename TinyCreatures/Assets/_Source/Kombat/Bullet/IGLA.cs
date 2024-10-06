using _Source.EnemySystem;
using UnityEngine;

public class IGLA : ABullet
{
    [SerializeField] private float damage = 1;
    
    private void OnTriggerEnter2D(Collider2D colizeum)
    {
        Bug bugEnemy = colizeum.gameObject.GetComponent<Bug>();
        if (bugEnemy != null)
        {
            bugEnemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        Cockroach cockroach = colizeum.gameObject.GetComponent<Cockroach>();
        if (cockroach!= null)
        {
            cockroach.TakeDamage(damage);
            Destroy(gameObject);
        }

        Rat rat = colizeum.gameObject.GetComponent<Rat>();
        if(rat!=null)
        {
            rat.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
