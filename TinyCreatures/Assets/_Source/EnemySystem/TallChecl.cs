using _Source.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallChecl : MonoBehaviour
{
    [SerializeField] private GameObject tail;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (tail != null)
            {
                tail.GetComponent<CapsuleCollider2D>().enabled = true;
            }
        }
    }
}