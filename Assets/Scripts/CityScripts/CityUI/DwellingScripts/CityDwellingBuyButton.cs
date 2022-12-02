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

    public short unitsAvailableToBuy;
    public bool objectSelected;

    private void Start ()
    {
        objectSelected = false;
    }

    public void UpdateButton (Sprite _unitIcon, string currentUnitCount)
    {
        unitIcon.sprite = _unitIcon;
        thisButton.interactable = true;
        dwellingUnitCountDisplay.SetActive(true);
        dwellingUnitCountDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentUnitCount;
        unitsAvailableToBuy = CityManager.Instance.currentCity.cityDwellingInformation.CalculateUnitsAvailableToBuy(buttonIndex);
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
            BuyUnits(unitsAvailableToBuy);
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

    public void BuyUnits (int _unitCount)
    {
        CityManager.Instance.currentCity.cityDwellingInformation.BuyUnits(buttonIndex, _unitCount);
        DwellingUI.Instance.UpdateDwellingDisplay();
        CityManager.Instance.cityResourceDisplay.UpdateDisplay();
    }
}
