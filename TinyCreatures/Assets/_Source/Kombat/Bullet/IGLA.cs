using _Source.EnemySystem;
using UnityEngine;

public class IGLA : ABullet
{
    [SerializeField] private float damage = 1;
    //TODO Сделать для игрока 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bug bugEnemy = collision.gameObject.GetComponent<Bug>();
        if (bugEnemy != null)
        {
            bugEnemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        Cockroach cockroach = collision.gameObject.GetComponent<Cockroach>();
        if (cockroach!= null)
        {
            cockroach.TakeDamage(damage);
            Destroy(gameObject);
        }

        Rat rat = collision.gameObject.GetComponent<Rat>();
        if(rat!=null)
        {
            rat.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
