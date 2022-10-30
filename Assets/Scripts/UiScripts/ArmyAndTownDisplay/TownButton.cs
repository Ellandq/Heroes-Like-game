using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownButton : MonoBehaviour
{
    [SerializeField] GameObject townHighlight;
    private GameObject connectedCity;

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

    void FixedUpdate ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedCity){
            townHighlight.SetActive(true);
        }else townHighlight.SetActive(false);
    }
}
