using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityDwellingBuyButton : MonoBehaviour
{
    [SerializeField] Sprite unitButtonDefault; 
    [SerializeField] Sprite unitButtonHighlight;  
    [SerializeField] Image unitIcon;
    [SerializeField] Sprite defaultUnitIcon;
    [SerializeField] Button thisButton;  
    [SerializeField] GameObject dwellingUnitCountDisplay;
    [SerializeField] private short buttonIndex;
    private short unitID;
    public short unitsAvailableToBuy;
    public bool objectSelected;
    private byte availableSlotIndex;

    private void Start ()
    {
        objectSelected = false;
    }

    public void UpdateButton (Sprite unitIcon, string currentUnitCount, short unitID)
    {
        this.unitID = unitID;
        this.unitIcon.sprite = unitIcon;
        dwellingUnitCountDisplay.SetActive(true);
        dwellingUnitCountDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentUnitCount;
        UpdateAvailableUnits();
        CheckAvailableSlots();
    }

    public void DisableButton ()
    {
        unitIcon.sprite = defaultUnitIcon;
        thisButton.interactable = false;
        dwellingUnitCountDisplay.SetActive(false);
    }

    public void ActivateHighlight ()
    {
        if (objectSelected){
            if (unitsAvailableToBuy != 0){
                BuyUnits(unitsAvailableToBuy);
            }
        }else{
            DwellingUI.Instance.DeactivateBuyButtonHighlights(buttonIndex);
            objectSelected = true;
            thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        }
    }

    public void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
        objectSelected = false;
    }

    private void CheckAvailableSlots ()
    {
        availableSlotIndex = CityManager.Instance.GetCity().GetSameUnitSlotIndex(unitID);
        if (availableSlotIndex == 7){
            if (DwellingUI.Instance.emptyCitySlots.Count > 0){
                availableSlotIndex = CityManager.Instance.GetCity().GetEmptyGarrisonSlotCount()[0];
                thisButton.interactable = true;
            }else{
                thisButton.interactable = false;
            }
        }else{
            thisButton.interactable = true;
        }
    }

    private void UpdateAvailableUnits()
    {
        unitsAvailableToBuy = CityManager.Instance.GetCity().GetBuildingHandler().GetCityDwellingInformation().CalculateUnitsAvailableToBuy(buttonIndex);
    }

    public void BuyUnits (int unitCount)
    {
        if (unitCount > 0){
            CityManager.Instance.GetCity().GetBuildingHandler().GetCityDwellingInformation().BuyUnits(buttonIndex, unitCount);
            DwellingUI.Instance.UpdateDwellingDisplay();
            CityManager.Instance.UpdateResourceDisplay();
            CityManager.Instance.GetCity().GetUnitsInformation().AddUnits(availableSlotIndex, unitCount, unitID);
        }else{
            Debug.Log("No units are available");
        }
        UpdateAvailableUnits();
    }
}
