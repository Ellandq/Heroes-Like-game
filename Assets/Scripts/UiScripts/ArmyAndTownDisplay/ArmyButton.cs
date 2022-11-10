using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyButton : MonoBehaviour
{
    [SerializeField] GameObject armyHighlight;
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

    internal void UpdateConnectedArmy(GameObject _army)
    {
        if (connectedArmy != null)  connectedArmy.GetComponentInParent<Army>().onMovementPointsChanged.RemoveAllListeners();
        connectedArmy = _army.transform.GetChild(0).gameObject;
        movementSlider.maxValue = connectedArmy.GetComponentInParent<Army>().maxMovementPoints;
        movementSlider.value = connectedArmy.GetComponentInParent<Army>().movementPoints;
        connectedArmy.GetComponentInParent<Army>().onMovementPointsChanged.AddListener(ChangeMovementPointSliderStatus);
    }

    public void SelectArmy ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedArmy){
            connectedArmy.GetComponentInParent<Army>().ArmyInteraction();
        }else{
            ObjectSelector.Instance.RemoveSelectedObject();
            ObjectSelector.Instance.AddSelectedObject(connectedArmy);
        }
    }

    private void HighlighLogic ()
    {
        if (connectedArmy != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected.name == (connectedArmy.name)) armyHighlight.SetActive(true);
            else armyHighlight.SetActive(false);
        }else armyHighlight.SetActive(false);
    }

    private void ChangeMovementPointSliderStatus()
    {
        movementSlider.value = connectedArmy.GetComponentInParent<Army>().movementPoints;
    }
}
