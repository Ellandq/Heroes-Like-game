using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownButton : MonoBehaviour
{
    [SerializeField] private GameObject townHighlight;
    private GameObject connectedCity;

    void Start ()
    {
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlighLogic);
        }catch (NullReferenceException){
            Debug.Log("Object Selector Instance does not exist.");
        }
    }

    // Updates the connected city
    internal void UpdateConnectedCity(GameObject _city)
    {
        connectedCity = _city;
    }

    // Selects a City if the button is pressed
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
    private void HighlighLogic ()
    {
        if (connectedCity != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedCity) townHighlight.SetActive(true);
            else townHighlight.SetActive(false);
        }else townHighlight.SetActive(false);
    }
}
