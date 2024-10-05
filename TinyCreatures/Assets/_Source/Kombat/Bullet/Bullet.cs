using _Source.EnemySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : ABullet
{

    [SerializeField] private float damage = 1;

    public void OnCollisionEnter2D(Collision2D collision)
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

