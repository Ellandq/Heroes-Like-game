using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IDropHandler
{
    [SerializeField] private Sprite unitButtonDefault; 
    [SerializeField] private Sprite unitButtonHighlight;  
    [SerializeField] private Button thisButton;  
    [SerializeField] private short slotID;
    public bool isSlotEmpty;

    [SerializeField] private UnitButton buttonToSwap;

    private void Start ()
    {
        try{
            ArmyInformation.Instance.onUnitDisplayReload.AddListener(DeactivateHighlight);
        }catch (NullReferenceException){
            Debug.Log("Army Information instance is missing");
        }
        
    }

    // Activates a unit highlight
    public void ActivateHighlight ()
    {
        ArmyInformation.Instance.onUnitDisplayReload?.Invoke();
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        ArmyInformation.Instance.ChangeSelectedUnit(slotID);
    }

    // Deactivates the highlight
    private void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
    }

    // When a dragged object is dropped on this one, it runs a few checks to determine what it should do
    public void OnDrop (PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "UnitIcons"){
            if (eventData.pointerDrag != this.gameObject)
            {
                buttonToSwap = eventData.pointerDrag.GetComponentInParent<UnitButton>();
                if (isSlotEmpty){
                    if (!ArmyInformation.Instance.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                            ArmyInformation.Instance.SplitUnits(buttonToSwap.slotID, slotID);
                        }else{
                            ArmyInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        ArmyInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }else{
                    if (!ArmyInformation.Instance.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (ArmyInformation.Instance.AreUnitsSameType(buttonToSwap.slotID, slotID)){
                            if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                                ArmyInformation.Instance.SplitUnits(buttonToSwap.slotID, slotID);
                            }else{
                                ArmyInformation.Instance.AddUnits(buttonToSwap.slotID, slotID);
                            }
                        }else{
                            ArmyInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        ArmyInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }
            }
        }
    }
}
