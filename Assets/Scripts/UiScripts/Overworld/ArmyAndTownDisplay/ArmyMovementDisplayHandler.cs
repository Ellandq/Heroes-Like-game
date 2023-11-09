using System;
using UnityEngine;
using UnityEngine.UI;

public class ArmyMovementDisplayHandler : MonoBehaviour
{
    private Army connectedArmy; 

    [Header ("UI References")]
    [SerializeField] private Slider movementSlider;

    public void UpdateMovementDisplay (Army newArmy){
        if (connectedArmy != null)  connectedArmy.onMovementPointsChanged.RemoveAllListeners();
        connectedArmy = newArmy;
        movementSlider.maxValue = connectedArmy.GetMaxMovementPoints();
        movementSlider.value = connectedArmy.GetMovementPoints();
        movementSlider.transform.parent.gameObject.SetActive(true);

        connectedArmy.onMovementPointsChanged.AddListener(ChangeMovementPointSliderStatus);
    }

    private void ChangeMovementPointSliderStatus(){
        movementSlider.value = connectedArmy.GetMovementPoints();
    }

    public void ResetArmyButton ()
    {
        if (connectedArmy != null)  connectedArmy.onMovementPointsChanged.RemoveAllListeners();
        if (movementSlider.transform.parent.gameObject.activeSelf){
            movementSlider.transform.parent.gameObject.SetActive(false);
        }
        connectedArmy = null;
    }
}
