using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPlayerIcon : MonoBehaviour
{
    [SerializeField] private GameObject fullHealthIcon;
    [SerializeField] private GameObject midHealthIcon;
    [SerializeField] private GameObject lowHealthIcon;
    
    public void SwitchIcon(int health)
    {
        if (health >= 60)
        {
            fullHealthIcon.SetActive(true);
            midHealthIcon.SetActive(false);
            lowHealthIcon.SetActive(false);
        }
        else if (health >= 30 && health < 60)
        {
            fullHealthIcon.SetActive(false);
            midHealthIcon.SetActive(true);
            lowHealthIcon.SetActive(false);
        }
        else
        {
            fullHealthIcon.SetActive(false);
            midHealthIcon.SetActive(false);
            lowHealthIcon.SetActive(true);
        }
    }
}
