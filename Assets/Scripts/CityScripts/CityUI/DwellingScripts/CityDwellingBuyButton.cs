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
    private int availableSlotIndex;

    private void Start ()
    {
        objectSelected = false;
    }

    public void UpdateButton (Sprite _unitIcon, string currentUnitCount, short _unitID)
    {
        unitID = _unitID;
        unitIcon.sprite = _unitIcon;
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
        availableSlotIndex = CityManager.Instance.currentCity.GetSameUnitSlotIndex(unitID);
        if (availableSlotIndex == 7){
            if (DwellingUI.Instance.emptyCitySlots.Count > 0){
                availableSlotIndex = DwellingUI.Instance.emptyCitySlots[0];
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
        unitsAvailableToBuy = CityManager.Instance.currentCity.cityDwellingInformation.CalculateUnitsAvailableToBuy(buttonIndex);
    }

    public void BuyUnits (int _unitCount)
    {
        if (_unitCount > 0){
            CityManager.Instance.currentCity.cityDwellingInformation.BuyUnits(buttonIndex, _unitCount);
            DwellingUI.Instance.UpdateDwellingDisplay();
            CityManager.Instance.cityResourceDisplay.UpdateDisplay();
            CityManager.Instance.currentCity.AddUnits(unitID, _unitCount, availableSlotIndex);
        }else{
            Debug.Log("No units are available");
        }
        UpdateAvailableUnits();
    }
}
