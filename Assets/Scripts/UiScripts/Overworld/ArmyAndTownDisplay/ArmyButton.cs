using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyButton : MonoBehaviour
{
    [Header ("Slot Information")]
    [SerializeField] private Army connectedArmy;

    [Header ("UI References")]
    [SerializeField] private Slider movementSlider;
    [SerializeField] private Image slotFrame;
    [SerializeField] private Image armyIcon;

    [Header ("Display Images")]
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite defaultFrame;
    [SerializeField] private Sprite frameHighlighted;

    private void Start ()
    {
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlightLogic);
        }catch (NullReferenceException){
            Debug.Log("Object Selector Instance does not exist.");
        }  
    }

    // Updates the connected army
    public void UpdateConnectedArmy(Army army)
    {
        if (connectedArmy != null)  connectedArmy.onMovementPointsChanged.RemoveAllListeners();
        connectedArmy = army;
        movementSlider.maxValue = connectedArmy.GetMaxMovementPoints();
        movementSlider.value = connectedArmy.GetMovementPoints();
        movementSlider.transform.parent.gameObject.SetActive(true);
        armyIcon.sprite = army.GetUnitsInformation().GetArmyIcon();
        connectedArmy.onMovementPointsChanged.AddListener(ChangeMovementPointSliderStatus);
        HighlightLogic();
    }

    // Selects an army if the button is pressed
    public void SelectArmy (){
        ObjectSelector.Instance.HandleWorldObjects(connectedArmy, true);
    }

    // Checks if the highlight should be activated
    private void HighlightLogic (){
        if (connectedArmy != null && ObjectSelector.Instance.GetSelectedArmy() == connectedArmy){
            slotFrame.sprite = frameHighlighted;
        }else{
            slotFrame.sprite = defaultFrame;
        }
    }

    // Changes the movement point slider value
    private void ChangeMovementPointSliderStatus(){
        movementSlider.value = connectedArmy.GetMovementPoints();
    }

    // Resets the button status
    public void ResetArmyButton ()
    {
        if (connectedArmy != null)  connectedArmy.onMovementPointsChanged.RemoveAllListeners();
        if (movementSlider.transform.parent.gameObject.activeSelf){
            movementSlider.transform.parent.gameObject.SetActive(false);
            slotFrame.sprite = defaultFrame;
            armyIcon.sprite = defaultBackground;
        }
        connectedArmy = null;
        HighlightLogic();
    }
}
