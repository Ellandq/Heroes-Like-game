using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityDwellingButton : MonoBehaviour
{
    [SerializeField] private Sprite unitButtonDefault; 
    [SerializeField] private Sprite unitButtonHighlight;  
    [SerializeField] private Image unitIcon;
    [SerializeField] private Sprite defaultUnitIcon;
    [SerializeField] private Button thisButton;  
    [SerializeField] private GameObject dwellingUnitCountDisplay;
    [SerializeField] private short buttonIndex;
    [SerializeField] private CityDwellingBuyButton unitBuyButton;

    public bool isSlotEmpty;
    public bool objectSelected;

    private void Start ()
    {
        objectSelected = false;
        DwellingUI.Instance.onDwellingUpdate.AddListener(DeactivateHighlight);
    }

    public void UpdateButton (Sprite unitIcon, string currentUnitCount, short unitID)
    {
        this.unitIcon.sprite = unitIcon;
        thisButton.interactable = true;
        dwellingUnitCountDisplay.SetActive(true);
        dwellingUnitCountDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentUnitCount;
        unitBuyButton.UpdateButton(unitIcon, currentUnitCount, unitID);
    }

    public void DisableButton ()
    {
        unitIcon.sprite = defaultUnitIcon;
        thisButton.interactable = false;
        dwellingUnitCountDisplay.SetActive(false);
        unitBuyButton.DisableButton();
    }

    public void ActivateHighlight ()
    {
        if (objectSelected){
            DwellingUI.Instance.OpenDwellingUI();
        }else{
            DwellingUI.Instance.onDwellingUpdate?.Invoke();
            objectSelected = true;
            thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        }
        
    }

    public void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
        objectSelected = false;
    }
}
