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
        Vector2 lookDirection = mousePos - player.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        player.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}