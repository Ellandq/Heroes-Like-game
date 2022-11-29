using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyButton : MonoBehaviour
{
    [SerializeField] private GameObject armyHighlight;
    [SerializeField] internal Slider movementSlider;
    [SerializeField] private GameObject connectedArmy;

    void Start ()
    {
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlighLogic);
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
        connectedArmy.GetComponentInParent<Army>().onMovementPointsChanged.AddListener(ChangeMovementPointSliderStatus);
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
    private void HighlighLogic ()
    {
        if (connectedArmy != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected.name == (connectedArmy.name)) armyHighlight.SetActive(true);
            else armyHighlight.SetActive(false);
        }else armyHighlight.SetActive(false);
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
        if (movementSlider.gameObject.activeSelf){
            movementSlider.gameObject.SetActive(false);
        }
        connectedArmy = null;
        HighlighLogic();
    }
}
