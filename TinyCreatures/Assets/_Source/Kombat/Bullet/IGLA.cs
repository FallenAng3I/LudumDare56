using _Source.EnemySystem;
using UnityEngine;

public class IGLA : ABullet
{
    [SerializeField] private float damage = 50;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Bug bugEnemy = collider.gameObject.GetComponent<Bug>();
        if (bugEnemy != null)
        {
            bugEnemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        Cockroach cockroach = collider.gameObject.GetComponent<Cockroach>();
        if (cockroach!= null)
        {
            cockroach.TakeDamage(damage);
            Destroy(gameObject);
        }

        Rat rat = collider.gameObject.GetComponent<Rat>();
        if(rat!=null)
        {
            rat.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
