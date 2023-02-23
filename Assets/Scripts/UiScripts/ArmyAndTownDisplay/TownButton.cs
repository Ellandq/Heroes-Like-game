using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownButton : MonoBehaviour
{
    [Header ("Slot Information")]
    [SerializeField] private GameObject connectedCity;
    [SerializeField] private bool slotSelected;
    [SerializeField] public bool slotEmpty;

    [Header ("UI References")]
    [SerializeField] private Image slotFrame;
    [SerializeField] private Image CityIcon;

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

    // Updates the connected town
    internal void UpdateConnectedCity(GameObject _city)
    {
        connectedCity = _city.transform.GetChild(0).gameObject;
        CityIcon.sprite = _city.GetComponentInParent<City>().cityIcon;
        slotEmpty = false;
        HighlightLogic();
    }

    // Selects an City if the button is pressed
    public void SelectCity ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedCity){
            connectedCity.GetComponentInParent<City>().CityInteraction();
        }else{
            ObjectSelector.Instance.RemoveSelectedObject();
            ObjectSelector.Instance.AddSelectedObject(connectedCity);
        }
    }

    // Checks if the highlight should be activated
    private void HighlightLogic ()
    {
        if (connectedCity != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected.name == (connectedCity.name)){
                slotFrame.sprite = defaultFrame;
                slotSelected = true;
            }else{
                slotFrame.sprite = frameHighlighted;
                slotSelected = false;
            }
        }else{
            slotFrame.sprite = defaultFrame;
            slotSelected = false;
        }
    }

    // Resets the button status
    public void ResetCityButton ()
    {
        slotFrame.sprite = defaultFrame;
        CityIcon.sprite = defaultBackground;
        slotSelected = false;
        slotEmpty = true;
        connectedCity = null;
        HighlightLogic();
    }
}
