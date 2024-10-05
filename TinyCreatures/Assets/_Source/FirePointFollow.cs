using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirePointFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 firePointDirection = (mousePos - player.position).normalized;
        
        transform.position = player.position + firePointDirection * 1.5f;

        transform.right = firePointDirection;
    }
}