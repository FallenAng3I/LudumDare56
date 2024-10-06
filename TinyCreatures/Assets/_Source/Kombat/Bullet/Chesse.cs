using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesse : ABullet
{
    public float speed = 10f;
    private Transform target;
    private Platform platform; 
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    private void Update()
    {
        platform = FindObjectOfType<Platform>();
        if (platform != null)
        {
            target = platform.GetComponent<Transform>();
            if (target != null)
            {
                Vector3 moveDirection = (target.position - transform.position).normalized;
                transform.Translate(moveDirection * speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.position) < 0.5f)
                {
                    Destroy(target.gameObject);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
