using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSplitWindow : MonoBehaviour
{
    [Header ("Split Information")]
    private UnitsInformation uInfo01;
    private UnitsInformation uInfo02;
    private byte id01;
    private byte id02;
    private int totalUnitCount;

    [Header("UI References")]
    [SerializeField] private Image unitIcon;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI unitCount01;
    [SerializeField] private TextMeshProUGUI unitCount02;
    

    public void PrepareUnitsToSwap (UnitsInformation uInfo01, byte id01, byte id02, UnitsInformation uInfo02 = null){
        this.uInfo01 = uInfo01;
        this.uInfo02 = uInfo02;
        this.id01 = id01;
        this.id02 = id02;

        gameObject.SetActive(true);

        // Update Sprite
        unitIcon.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), uInfo01.GetUnit(id01).GetId()));

        // Update Slider
        if (uInfo02 == null){
            totalUnitCount = uInfo01.GetUnit(id01).GetUnitCount();
            slider.maxValue = totalUnitCount;
            slider.minValue = 1;
            slider.value = totalUnitCount - 1;
        }else{
            totalUnitCount = uInfo01.GetUnit(id01).GetUnitCount() + uInfo02.GetUnit(id02).GetUnitCount();
            slider.maxValue = totalUnitCount;
            slider.minValue = 1;
            slider.value = uInfo01.GetUnit(id01).GetUnitCount();
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    public void UpdateDisplayedValues (){
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Finalize the unit split depending on the interacting objects
    public void ActualizeUnitSplit ()
    {
        uInfo01.SetUnitCount(id01, Convert.ToInt32(slider.value));
        if (uInfo02 == null){
            if (uInfo01.GetUnit(id02).IsEmpty()){
                uInfo01.SetSlotStatus(id02, Convert.ToInt32(totalUnitCount - slider.value), uInfo01.GetUnit(id01).GetId());
            }else{
                uInfo01.SetUnitCount(id02, Convert.ToInt32(totalUnitCount - slider.value));
            }
        }else{
            if (uInfo02.GetUnit(id02).IsEmpty()){
                uInfo02.SetSlotStatus(id02, Convert.ToInt32(totalUnitCount - slider.value), uInfo01.GetUnit(id01).GetId());
            }else{
                uInfo02.SetUnitCount(id02, Convert.ToInt32(totalUnitCount - slider.value));
            }
        }
        DisableUnitSplitWindow();
    }

    public void DisableUnitSplitWindow ()
    {
        UIManager.Instance.FinalizeUnitSplit();
        totalUnitCount = 0;
        uInfo01 = null;
        uInfo02 = null;
        gameObject.SetActive(false);
    }
}
