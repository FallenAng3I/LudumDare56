using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteLeg : MonoBehaviour
{
    [SerializeField] private GameObject leg;
    [SerializeField] private GameObject bugs;

    private void Start()
    {
        StartCoroutine(SpawnLeg());
    }

    private new IEnumerator SpawnLeg()
    {
        yield return new WaitForSeconds(.1f);
        leg.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        bugs.gameObject.SetActive(true);
        
        
    }
}
