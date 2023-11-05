using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;
    private IEnumerator checkMouseOverCoroutine;

    [Header ("Slot information")]
    [SerializeField] private Button button;  
    [SerializeField] private byte slotID;
    [SerializeField] public bool isSlotEmpty;

    // Activates a unit highlight
    public void ActivateHighlight (){
        button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UnitDisplay/IconFrameHighlight");
    }

    // Deactivates the highlight
    public void DeactivateHighlight (){
        button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UnitDisplay/IconFrame");
    }

    // When a dragged object is dropped on this one, it runs a few checks to determine what it should do
    public void OnDrop (PointerEventData eventData)
    {
        //  Checks if the dropped object is a unitIcon
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "UnitIcons"){
            // Checks if the dropped object isn't this object
            if (eventData.pointerDrag != this.gameObject)
            {
                UnitButton buttonToSwap = eventData.pointerDrag.GetComponentInParent<UnitButton>();
                if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                    ArmyDisplayInformation.Instance.SwapUnits(buttonToSwap.slotID, slotID);
                }else{
                    ArmyDisplayInformation.Instance.SplitUnits(buttonToSwap.slotID, slotID);
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
        yield return new WaitForSeconds(1f);
        if (isMouseOver){
            Debug.Log("Mouse is still over UI element");
        }
    }

    public byte GetId () { return slotID; }
}
