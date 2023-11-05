using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    [Header ("Resource display referances")]
    [SerializeField] private List<TextMeshProUGUI> resourceDisplay;

    public void UpdateDisplay ()
    {
        int[] rs = PlayerManager.Instance.GetCurrentPlayer().GetAvailableResources().GetResources();

        for (int i = 0; i < 7; i++){
            resourceDisplay[i].text = Convert.ToString(rs[i]);
        }
    }
}
