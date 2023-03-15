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
    [SerializeField] ArmyInterfaceArmyInformation armyInterface;
    private bool isMouseOver = false;
    private IEnumerator checkMouseOverCoroutine;

    [Header ("Sprites")]
    [SerializeField] private Sprite unitButtonDefault; 
    [SerializeField] private Sprite unitButtonHighlight;  
    [SerializeField] private Sprite unitBackgroundDefault;

    [Header ("UI References")]
    [SerializeField] private Button unitButton;  
    [SerializeField] private GameObject unitCountDisplay;
    [SerializeField] private Image unitIcon;

    [Header ("Button Information")]
    [SerializeField] private short slotID;
    [SerializeField] public bool isSlotEmpty;

    [SerializeField] private ArmyInterfaceUnitButton buttonToSwap;

    private void Start ()
    { 
        armyInterface.onArmyInterfaceReload.AddListener(DeactivateHighlight);  
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
            if (eventData.pointerDrag != this.gameObject)
            {
                buttonToSwap = eventData.pointerDrag.GetComponentInParent<ArmyInterfaceUnitButton>();
                if (isSlotEmpty){
                    if (!armyInterface.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                            armyInterface.SplitUnits(buttonToSwap.slotID, slotID);
                        }else{
                            armyInterface.SwapUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        armyInterface.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }else{
                    if (!armyInterface.AreUnitsHeroes(buttonToSwap.slotID, slotID)){
                        if (armyInterface.AreUnitsSameType(buttonToSwap.slotID, slotID)){
                            if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                                armyInterface.SplitUnits(buttonToSwap.slotID, slotID);
                            }else{
                                armyInterface.AddUnits(buttonToSwap.slotID, slotID);
                            }
                        }else{
                            armyInterface.SwapUnits(buttonToSwap.slotID, slotID);
                        }
                    }else{
                        armyInterface.SwapUnits(buttonToSwap.slotID, slotID);
                    }
                }
            }
        }
    }

    public void UpdateButton(GameObject unit){
        if (unit != null){
            if (!unit.GetComponent<UnitSlot>().slotEmpty){
                unitButton.interactable = true;
                // Check if the selected unit is a hero
                if (unit.GetComponent<UnitSlot>().isSlotHero){
                    unitIcon.sprite = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), unit.GetComponent<UnitSlot>().unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
                    isSlotEmpty = false;
                    unitCountDisplay.SetActive(false);
                }else{
                    unitIcon.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unit.GetComponent<UnitSlot>().unitID));
                    isSlotEmpty = false;
                    unitCountDisplay.SetActive(true);
                    unitCountDisplay.GetComponentInChildren<TMP_Text>().text = Convert.ToString(unit.GetComponent<UnitSlot>().howManyUnits);                       
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
}
