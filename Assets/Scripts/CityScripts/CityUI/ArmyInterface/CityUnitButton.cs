using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CityUnitButton : MonoBehaviour, IDropHandler
{
    [SerializeField] Sprite unitButtonDefault; 
    [SerializeField] Sprite unitButtonHighlight;  
    [SerializeField] Button thisButton;  
    [SerializeField] short slotID;
    public bool isSlotEmpty;

    [SerializeField] private CityUnitButton buttonToSwap;

    void Start ()
    {
        try{
            ArmyInterfaceArmyInformation.Instance.onArmyInterfaceReload.AddListener(DeactivateHighlight);
        }catch (NullReferenceException){
            Debug.Log("Army Information instance is missing");
        }
        
    }

    public void ActivateHighlight ()
    {
        ArmyInterfaceArmyInformation.Instance.onArmyInterfaceReload?.Invoke();
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        ArmyInterfaceArmyInformation.Instance.ChangeSelectedUnit(slotID);
    }

    private void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
    }

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
