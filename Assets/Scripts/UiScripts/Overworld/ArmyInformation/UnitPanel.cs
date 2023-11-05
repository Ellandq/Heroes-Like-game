using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

[System.Serializable]
public class UnitPanel : MonoBehaviour
{
    [SerializeField] private byte index;
    [SerializeField] private Image unitImage;
    [SerializeField] private Image bannerImage;
    [SerializeField] private Button button;
    [SerializeField] private UnitButton unitButton;
    [SerializeField] private TMP_Text unitCountDisplay;

    public void UpdateUnitPanel (UnitSlot slot){
        if (!slot.IsEmpty()){
            unitButton.isSlotEmpty = false;
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
            unitButton.isSlotEmpty = true;
            button.interactable = false;
            unitImage.gameObject.SetActive(false);
            if (index != 0){
                bannerImage.sprite = Resources.Load<Sprite>("UI/UnitDisplay/UnitBannerClosed");
            }
            unitImage.sprite = Resources.Load<Sprite>("UI/UnitDsiplay/UnitBackground");
            unitButton.isSlotEmpty = true;
            unitCountDisplay.transform.parent.gameObject.SetActive(false);
        }
    }

    public void ChangeHighlightStatus (bool status){
        if (status) AddHighlight();
        else RemoveHighlight();
    }

    public void RemoveHighlight (){
        unitButton.DeactivateHighlight();
    }

    public void AddHighlight (){
        unitButton.ActivateHighlight();
    }
}
