using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automat : Aweapon
{
    public override void shoot()
    {
        base.shoot();
        Debug.Log("piy piy");
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }
    }
}
