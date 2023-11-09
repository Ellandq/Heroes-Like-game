using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour
{
    [SerializeField] private ArmyMovementDisplayHandler movementDisplayHandler;

    [Header ("Slot Information")]
    private IUnitHandler connectedObject;

    [Header ("UI References")]
    [SerializeField] private Image slotFrame;
    [SerializeField] private Image objectIcon;

    [Header ("Display Images")]
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite defaultFrame;
    [SerializeField] private Sprite frameHighlighted;

    private void Start (){
        ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlightLogic);
    }

    public void UpdateConnectedObject(IUnitHandler newObject)
    {
        if (movementDisplayHandler != null) movementDisplayHandler.UpdateMovementDisplay(newObject as Army);
        connectedObject = newObject;
        objectIcon.sprite = connectedObject.GetIcon();
        HighlightLogic();
    }

    public void SelectObject (){
        ObjectSelector.Instance.HandleWorldObjects(connectedObject as WorldObject, true);
    }

    private void HighlightLogic (){
        if (connectedObject != null && ObjectSelector.Instance.GetSelectedObject() == connectedObject as WorldObject){
            slotFrame.sprite = frameHighlighted;
        }else{
            slotFrame.sprite = defaultFrame;
        }
    }

    public void ResetButton ()
    {
        if (connectedObject != null){
            slotFrame.sprite = defaultFrame;
            objectIcon.sprite = defaultBackground;
        }
        connectedObject = null;
        HighlightLogic();
    }
}
