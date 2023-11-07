using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

[System.Serializable]
public class UnitPanel : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;
    private IEnumerator checkMouseOverCoroutine;

    [SerializeField] private byte index;
    [SerializeField] private Image unitImage;
    [SerializeField] private Image bannerImage;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text unitCountDisplay;

    private UnitSlot connectedSlot;

    public void UpdateUnitPanel (UnitSlot slot){
        connectedSlot = slot;
        if (!slot.IsEmpty()){
            button.interactable = true;
            unitImage.gameObject.SetActive(true);
            if (index == 0){
                bannerImage.sprite = Resources.Load<Sprite>("UI/UnitDisplay/UnitBannerOpened");
            }

            if (slot.IsHero()){
                unitImage.sprite = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), slot.GetId() - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
                unitCountDisplay.transform.parent.gameObject.SetActive(false);
            }else{
                unitImage.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), slot.GetId()));
                unitCountDisplay.transform.parent.gameObject.SetActive(true);
                unitCountDisplay.text = Convert.ToString(slot.GetUnitCount());
            }
        }else{
            button.interactable = false;
            unitImage.gameObject.SetActive(false);
            if (index != 0){
                bannerImage.sprite = Resources.Load<Sprite>("UI/UnitDisplay/UnitBannerClosed");
            }
            unitImage.sprite = Resources.Load<Sprite>("UI/UnitDsiplay/UnitBackground");
            unitCountDisplay.transform.parent.gameObject.SetActive(false);
        }
    }

    public void ChangeHighlightStatus (bool status){
        if (status) AddHighlight();
        else RemoveHighlight();
    }

    public void RemoveHighlight (){
        button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UnitsDisplay/IconFrame");
    }

    public void AddHighlight (){
        button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UnitsDisplay/IconFrameHighlight");
    }

    // When a dragged object is dropped on this one, it runs a few checks to determine what it should do
    public void OnDrop (PointerEventData eventData)
    {
        //  Checks if the dropped object is a unitIcon
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "UnitIcons"){
            // Checks if the dropped object isn't this object
            if (eventData.pointerDrag != this.gameObject)
            {
                UnitPanel buttonToSwap = eventData.pointerDrag.GetComponentInParent<UnitPanel>();
                if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                    ArmyDisplayInformation.Instance.SwapUnits(buttonToSwap.GetId(), GetId());
                }else{
                    ArmyDisplayInformation.Instance.SplitUnits(buttonToSwap.GetId(), GetId());
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if (!connectedSlot.IsEmpty()){
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

    public byte GetId () { return index; }
}
