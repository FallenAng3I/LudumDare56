using _Source.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    private Launcher launcher;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            launcher = FindObjectOfType<Launcher>();
            launcher.currentAmmo += 1;
            Destroy(gameObject);
        }
    }
}
