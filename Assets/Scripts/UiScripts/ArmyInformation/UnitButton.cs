using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IDropHandler
{
    [SerializeField] private ArmyInformation armyInformation;

    [Header ("Unit button sprites")]
    [SerializeField] private Sprite unitFrameDefault; 
    [SerializeField] private Sprite unitFrameHighlight;  

    [Header ("Slot information")]
    [SerializeField] private Button thisButton;  
    [SerializeField] private short slotID;
    [SerializeField] public bool isSlotEmpty;

    [SerializeField] private UnitButton buttonToSwap;

    private void Start ()
    {
        armyInformation.onUnitDisplayReload.AddListener(DeactivateHighlight); 
    }

    // Activates a unit highlight
    public void ActivateHighlight ()
    {
        armyInformation.onUnitDisplayReload?.Invoke();
        thisButton.gameObject.GetComponent<Image>().sprite = unitFrameHighlight;
        armyInformation.ChangeSelectedUnit(slotID);
    }

    // Deactivates the highlight
    private void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitFrameDefault;
    }

    // When a dragged object is dropped on this one, it runs a few checks to determine what it should do
    public void OnDrop (PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "UnitIcons"){
            if (eventData.pointerDrag != this.gameObject)
            {
                buttonToSwap = eventData.pointerDrag.GetComponentInParent<UnitButton>();
                if (isSlotEmpty){
                    if (!armyInformation.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                            armyInformation.SplitUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        armyInformation.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }else{
                    if (!armyInformation.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (armyInformation.AreUnitsSameType(buttonToSwap.slotID, slotID)){
                            if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                                armyInformation.SplitUnits(buttonToSwap.slotID, slotID);
                            }else{
                                armyInformation.AddUnits(buttonToSwap.slotID, slotID);
                            }
                        }else{
                            armyInformation.SwapUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        armyInformation.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }
            }
        }
    }
}
