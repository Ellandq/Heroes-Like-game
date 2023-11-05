using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CityUnitButton : MonoBehaviour, IDropHandler
{
    [Header ("Button Images")]
    [SerializeField] Sprite unitButtonDefault; 
    [SerializeField] Sprite unitButtonHighlight;  

    [Header ("Button information")]
    [SerializeField] Button thisButton;  
    [SerializeField] short slotID;
    public bool isSlotEmpty;

    [SerializeField] private CityUnitButton buttonToSwap;

    // Checks if ArmyInterfaceArmyInformation static instance is set
    private void Start ()
    {
        try{
            //ArmyInterfaceArmyInformation.Instance.onArmyInterfaceReload.AddListener(DeactivateHighlight);
        }catch (NullReferenceException){
            Debug.Log("Army Information instance is missing");
        }
    }

    // Activates this buttons highlight
    public void ActivateHighlight ()
    {
        //ArmyInterfaceArmyInformation.Instance.onArmyInterfaceReload?.Invoke();
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        //ArmyInterfaceArmyInformation.Instance.ChangeSelectedUnit(slotID);
    }

    // Deactivates this buttons highlight
    private void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
    }

    // When an object is dropped on top of this checks if the given object is valid and proceeds accordingly
    public void OnDrop (PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "ArmyInterfaceUnitIcons"){
            if (eventData.pointerDrag != this.gameObject)
            {
                buttonToSwap = eventData.pointerDrag.GetComponentInParent<CityUnitButton>();
                if (isSlotEmpty){
                    if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                        CityArmyInterface.Instance.SplitUnits(buttonToSwap.slotID, slotID);
                    }else{
                        CityArmyInterface.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }else 
                {
                    if (CityArmyInterface.Instance.AreUnitsSameType(buttonToSwap.slotID, slotID)){
                        if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                            CityArmyInterface.Instance.SplitUnits(buttonToSwap.slotID, slotID);
                        }else{
                            CityArmyInterface.Instance.AddUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        CityArmyInterface.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }
            }
        }
    }
}
