using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteLeg : MonoBehaviour
{
    [SerializeField] private GameObject leg;

    private void Start()
    {
        StartCoroutine(SpawnLeg());
    }

    private new IEnumerator SpawnLeg()
    {
        yield return new WaitForSeconds(.1f);
        leg.gameObject.SetActive(true);
    }
}
