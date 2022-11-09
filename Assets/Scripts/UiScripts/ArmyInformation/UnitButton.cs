using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IDropHandler
{
    [SerializeField] Sprite unitButtonDefault; 
    [SerializeField] Sprite unitButtonHighlight;  
    [SerializeField] Button thisButton;  
    [SerializeField] short slotID;
    public bool isSlotEmpty;

    [SerializeField] private UnitButton buttonToSwap;

    void Start ()
    {
        try{
            ArmyInformation.Instance.onUnitDisplayReload.AddListener(DeactivateHighlight);
        }catch (NullReferenceException){
            Debug.Log("Army Information instance is missing");
        }
        
    }

    public void ActivateHighlight ()
    {
        ArmyInformation.Instance.onUnitDisplayReload?.Invoke();
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        ArmyInformation.Instance.ChangeSelectedUnit(slotID);
    }

    private void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
    }

    public void OnDrop (PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "UnitIcons"){
            if (eventData.pointerDrag != this.gameObject)
            {
                buttonToSwap = eventData.pointerDrag.GetComponentInParent<UnitButton>();
                if (isSlotEmpty){
                    if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                        ArmyInformation.Instance.SplitUnits(buttonToSwap.slotID, slotID);
                    }else{
                        ArmyInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }else 
                {
                    if (ArmyInformation.Instance.AreUnitsSameType(buttonToSwap.slotID, slotID)){
                        if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                            ArmyInformation.Instance.SplitUnits(buttonToSwap.slotID, slotID);
                        }else{
                            ArmyInformation.Instance.AddUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        ArmyInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }
            }
        }
    }
}
