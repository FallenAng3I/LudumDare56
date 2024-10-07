using _Source.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCount;
    [Header("Launcher sounds")]
    [SerializeField] private SoundFXManager soundFXManager;
    [SerializeField] private AudioClip shellInSound;
    [SerializeField, Range(0f, 1f)] private float launcherVolume = 0.2f;

    private Launcher launcher;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            ammoCount.text = "1 / 1";
            soundFXManager.PlaySoundFXClip(shellInSound, transform, launcherVolume);
            launcher = FindObjectOfType<Launcher>();
            launcher.currentAmmo += 1;
            Destroy(gameObject);
        }
    }
}
