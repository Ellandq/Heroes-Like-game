using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ArmyInformation armyInformation;
    private bool isMouseOver = false;
    private IEnumerator checkMouseOverCoroutine;

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
        //  Checks if the dropped object is a unitIcon
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "UnitIcons"){
            // Checks if the dropped object isn't this object
            if (eventData.pointerDrag != this.gameObject)
            {
                buttonToSwap = eventData.pointerDrag.GetComponentInParent<UnitButton>();
                if (isSlotEmpty){   // Logic for when this slot is empty
                    if (!armyInformation.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                            armyInformation.SplitUnits(buttonToSwap.slotID, slotID);
                        }
                    }
                }else{ 
                    if (!armyInformation.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (armyInformation.AreUnitsSameType(buttonToSwap.slotID, slotID)){
                            if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                                armyInformation.SplitUnits(buttonToSwap.slotID, slotID);
                            }else{
                                armyInformation.AddUnits(buttonToSwap.slotID, slotID);
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if (!isSlotEmpty){
            isMouseOver = true;
            checkMouseOverCoroutine = CheckMouseOver();
            StartCoroutine(checkMouseOverCoroutine);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        isMouseOver = false;
        if (checkMouseOverCoroutine != null)
        {
            StopCoroutine(checkMouseOverCoroutine);
        }
    }

    private IEnumerator CheckMouseOver()
    {
        yield return new WaitForSeconds(1f); // Wait for one and a half second
        if (isMouseOver){
            Debug.Log("Mouse is still over UI element");
        }
    }
}
