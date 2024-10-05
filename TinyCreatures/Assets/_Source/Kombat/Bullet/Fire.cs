using _Source.EnemySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : ABullet
{
    [SerializeField] private float damage = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bug bugEnemy = collision.gameObject.GetComponent<Bug>();
        if (bugEnemy != null)
        {
            bugEnemy.TakeDamage(damage/2);
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
            rat.TakeDamage(damage/2);
            Destroy(gameObject);
        }
    }
}
