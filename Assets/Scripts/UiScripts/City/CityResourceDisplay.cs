using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityResourceDisplay : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> resourcesDisplay; 

    public void UpdateDisplay ()
    {
        ResourceIncome res = PlayerManager.Instance.GetCurrentPlayer().GetAvailableResources();
        for (int i = 0; i < 7; i++){
            resourcesDisplay[i].text = Convert.ToString(res[i]);
        }  
    }
}
