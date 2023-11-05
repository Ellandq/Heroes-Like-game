using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownButton : MonoBehaviour
{
    [Header ("Slot Information")]
    [SerializeField] private City connectedCity;

    [Header ("UI References")]
    [SerializeField] private Image slotFrame;
    [SerializeField] private Image CityIcon;

    [Header ("Display Images")]
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite defaultFrame;
    [SerializeField] private Sprite frameHighlighted;

    private void Start (){
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlightLogic);
        }catch (NullReferenceException){
            Debug.Log("Object Selector Instance does not exist.");
        }  
    }

    // Updates the connected town
    public void UpdateConnectedCity(City city){
        connectedCity = city;
        CityIcon.sprite = city.GetCitySprite();
        HighlightLogic();
    }

    // Selects an City if the button is pressed
    public void SelectCity (){
        ObjectSelector.Instance.HandleWorldObjects(connectedCity);
    }

    // Checks if the highlight should be activated
    private void HighlightLogic (){
        if (connectedCity != null && ObjectSelector.Instance.GetSelectedCity() == connectedCity){
            slotFrame.sprite = frameHighlighted;
        }else{
            slotFrame.sprite = defaultFrame;
        }
    }

    // Resets the button status
    public void ResetCityButton ()
    {
        slotFrame.sprite = defaultFrame;
        CityIcon.sprite = defaultBackground;
        connectedCity = null;
        HighlightLogic();
    }
}
