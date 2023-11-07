using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ArmyInterfaceUnitButton : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header ("Sprites")]
    [SerializeField] private Sprite unitButtonDefault; 
    [SerializeField] private Sprite unitButtonHighlight;  
    [SerializeField] private Sprite unitBackgroundDefault;

    [Header ("UI References")]
    [SerializeField] private ArmyInterface armyInterface;
    [SerializeField] private Button unitButton;  
    [SerializeField] private GameObject unitCountDisplay;
    [SerializeField] private Image unitIcon;

    [Header ("Button Information")]
    private byte slotID;
    public bool isSlotEmpty;
    private bool isMouseOver = false;

    private IEnumerator checkMouseOverCoroutine;

    private void Start ()
    { 
        armyInterface.onArmyInterfaceReload += DeactivateHighlight;  
    }

    public void ActivateHighlight ()
    {
        armyInterface.onArmyInterfaceReload?.Invoke();
        unitButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        armyInterface.ChangeSelectedUnit(slotID);
    }

    private void DeactivateHighlight ()
    {
        unitButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
    }

    public void OnDrop (PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "ArmyInterfaceUnitIcons"){
            if (eventData.pointerDrag != gameObject)
            {
                armyInterface.HandleUnitInteraction(slotID, eventData.pointerDrag.GetComponentInParent<ArmyInterfaceUnitButton>().GetId());
            }
        }
    }

    public void UpdateButton(UnitSlot unit){
        if (unit != null){
            if (!unit.IsEmpty()){
                unitButton.interactable = true;
                if (unit.IsHero()){
                    unitIcon.sprite = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), unit.GetId() - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
                    isSlotEmpty = false;
                    unitCountDisplay.SetActive(false);
                }else{
                    unitIcon.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unit.GetId()));
                    isSlotEmpty = false;
                    unitCountDisplay.SetActive(true);
                    unitCountDisplay.GetComponentInChildren<TMP_Text>().text = Convert.ToString(unit.GetUnitCount());                       
                }
            }else{
                unitButton.interactable = false;
                unitIcon.sprite = unitBackgroundDefault;
                isSlotEmpty = true;
                unitCountDisplay.SetActive(false);
            }
        }else{
            unitButton.interactable = false;
            unitIcon.sprite = unitBackgroundDefault;
            isSlotEmpty = true;
            unitCountDisplay.SetActive(false);
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
        if (isMouseOver)
        {
            Debug.Log("Mouse is still over UI element");

        }
    }

    public byte GetId(){ return slotID; }
}
