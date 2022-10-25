using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyHighlight : MonoBehaviour
{
    [SerializeField] GameObject selectedObject;
    private Vector3 highlightAdjustedPosition;


    void Update ()
    {
        if (selectedObject != null) MoveHighlight();
    }

    public void SetHighlitedObject(GameObject _selectedObject){
        selectedObject = _selectedObject;
    }

    public void MoveHighlight ()
    {
        highlightAdjustedPosition = selectedObject.transform.position;
        highlightAdjustedPosition.y = 0.51f;
        this.gameObject.transform.position = highlightAdjustedPosition;
    }
}
