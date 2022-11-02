using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownButton : MonoBehaviour
{
    [SerializeField] GameObject townHighlight;
    private GameObject connectedCity;

    void Start ()
    {
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlighLogic);
        }catch (NullReferenceException){
            Debug.Log("Object Selector Instance does not exist.");
        }
    }

    internal void UpdateConnectedCity(GameObject _city)
    {
        connectedCity = _city;
    }

    public void SelectCity ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedCity){
            connectedCity.GetComponentInParent<City>().CityInteraction();
        }else{
            ObjectSelector.Instance.RemoveSelectedObject();
            ObjectSelector.Instance.AddSelectedObject(connectedCity);
        }
    }

    private void HighlighLogic ()
    {
        if (connectedCity != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedCity) townHighlight.SetActive(true);
            else townHighlight.SetActive(false);
        }else townHighlight.SetActive(false);
    }
}
