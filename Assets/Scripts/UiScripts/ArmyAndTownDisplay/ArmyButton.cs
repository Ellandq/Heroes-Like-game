using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyButton : MonoBehaviour
{
    [Header ("Slot Information")]
    [SerializeField] private GameObject connectedArmy;
    [SerializeField] private bool slotSelected;
    [SerializeField] public bool slotEmpty;

    [Header ("UI References")]
    [SerializeField] internal Slider movementSlider;
    [SerializeField] private Image slotFrame;
    [SerializeField] private Image armyIcon;

    [Header ("Display Images")]
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite defaultFrame;
    [SerializeField] private Sprite frameHighlighted;

    void Start ()
    {
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlightLogic);
        }catch (NullReferenceException){
            Debug.Log("Object Selector Instance does not exist.");
        }  
    }

    // Updates the connected army
    internal void UpdateConnectedArmy(GameObject _army)
    {
        if (connectedArmy != null)  connectedArmy.GetComponentInParent<Army>().onMovementPointsChanged.RemoveAllListeners();
        connectedArmy = _army.transform.GetChild(0).gameObject;
        movementSlider.maxValue = connectedArmy.GetComponentInParent<Army>().maxMovementPoints;
        movementSlider.value = connectedArmy.GetComponentInParent<Army>().movementPoints;
        movementSlider.transform.parent.gameObject.SetActive(true);
        armyIcon.sprite = _army.GetComponentInParent<Army>().unitsInformation.armyIcon;
        slotEmpty = false;
        connectedArmy.GetComponentInParent<Army>().onMovementPointsChanged.AddListener(ChangeMovementPointSliderStatus);
        HighlightLogic();
    }

    // Selects an army if the button is pressed
    public void SelectArmy ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedArmy){
            connectedArmy.GetComponentInParent<Army>().ArmyInteraction();
        }else{
            ObjectSelector.Instance.RemoveSelectedObject();
            ObjectSelector.Instance.AddSelectedObject(connectedArmy);
        }
    }

    // Checks if the highlight should be activated
    private void HighlightLogic ()
    {
        if (connectedArmy != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected.name == (connectedArmy.name)){
                slotFrame.sprite = frameHighlighted;
                slotSelected = true;
            }else{
                slotFrame.sprite = defaultFrame;
                slotSelected = false;
            }
        }else{
            slotFrame.sprite = defaultFrame;
            slotSelected = false;
        }
    }

    // Changes the movement point slider value
    private void ChangeMovementPointSliderStatus()
    {
        movementSlider.value = connectedArmy.GetComponentInParent<Army>().movementPoints;
    }

    // Resets the button status
    public void ResetArmyButton ()
    {
        if (connectedArmy != null)  connectedArmy.GetComponentInParent<Army>().onMovementPointsChanged.RemoveAllListeners();
        if (movementSlider.transform.parent.gameObject.activeSelf){
            movementSlider.transform.parent.gameObject.SetActive(true);
            slotFrame.sprite = defaultFrame;
            armyIcon.sprite = defaultBackground;
            slotSelected = false;
        }
        slotEmpty = true;
        connectedArmy = null;
        HighlightLogic();
    }
}
