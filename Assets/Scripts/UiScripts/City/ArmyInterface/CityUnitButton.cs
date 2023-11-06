using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CityUnitButton : MonoBehaviour, IDropHandler
{
    [Header ("Sprites")]
    [SerializeField] private Sprite unitButtonDefault; 
    [SerializeField] private Sprite unitButtonHighlight;  
    [SerializeField] private Sprite unitBackgroundDefault;

    [Header ("UI References")]
    [SerializeField] private CityArmyInterface cityInterface;
    [SerializeField] private Button unitButton;  
    [SerializeField] private GameObject unitCountDisplay;
    [SerializeField] private Image unitIcon;

    [Header ("Button Information")]
    private byte slotID;
    public bool isSlotEmpty;
    private bool isMouseOver = false;

    private void Start ()
    { 
        cityInterface.onCityReload += DeactivateHighlight;  
    }

    public void ActivateHighlight ()
    {
        cityInterface.onCityReload?.Invoke();
        unitButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        cityInterface.ChangeSelectedUnit(slotID);
    }

    private void DeactivateHighlight ()
    {
        unitButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
    }



    public void OnDrop (PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "ArmyInterfaceUnitIcons"){
            if (eventData.pointerDrag != this.gameObject)
            {
                cityInterface.HandleUnitInteraction(slotID, eventData.pointerDrag.GetComponentInParent<ArmyInterfaceUnitButton>().GetId());
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
}
