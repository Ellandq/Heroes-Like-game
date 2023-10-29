using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyHighlight : MonoBehaviour
{
    [SerializeField] private GameObject highlight;
    private bool isHighlightActive;

    public void EnableHighlight (bool status = true) {
        isHighlightActive = status;
        highlight.SetActive(status);
    }
    
    public bool IsHighlightActive (){ return isHighlightActive; }


}
