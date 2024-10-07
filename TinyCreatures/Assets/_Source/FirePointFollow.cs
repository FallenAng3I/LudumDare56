using _Source.Kombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirePointFollow : MonoBehaviour
{
    private Launcher launcher;
    private Shotgun shotgun;
    private Automat automat;
    [SerializeField] private Transform player;

    private void Start()
    {
        launcher = FindObjectOfType<Launcher>();
        shotgun = FindObjectOfType<Shotgun>();
        automat = FindObjectOfType<Automat>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector2 firePointDirection = ((Vector2)mousePos - (Vector2)player.position).normalized;
        transform.right = firePointDirection;
        
    }
}