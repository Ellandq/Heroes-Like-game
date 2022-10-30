using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyButton : MonoBehaviour
{
    [SerializeField] GameObject armyHighlight;
    private GameObject connectedArmy;

    internal void UpdateConnectedArmy(GameObject _army)
    {
        connectedArmy = _army;
    }

    public void SelectArmy ()
    {
        ObjectSelector.Instance.RemoveSelectedObject();
        ObjectSelector.Instance.ArmySelectionLogic(connectedArmy);
    }

    void FixedUpdate ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected == connectedArmy){
            armyHighlight.SetActive(true);
        }else armyHighlight.SetActive(false);
    }
}
