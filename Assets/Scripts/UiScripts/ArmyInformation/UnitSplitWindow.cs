using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSplitWindow : MonoBehaviour
{
    public static UnitSplitWindow Instance;

    [Header("Unit Icons")]
    [SerializeField] Image unitIcon01;
    [SerializeField] Image unitIcon02;

    [Header("Unit Count Objects")]
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI unitCount01;
    [SerializeField] TextMeshProUGUI unitCount02;
    private int totalUnitCount;
    private Army selectedArmy;
    private City selectedGarrison;
    private short unitID_1;
    private short unitID_2;

    private void Start ()
    {
        Instance = this;
        totalUnitCount = 0;
        selectedArmy = null;
        selectedGarrison = null;
    }

    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, Army connectedArmy, short id01, short id02)
    {
        selectedArmy = connectedArmy;
        unitID_1 = id01;
        unitID_2 = id02;
        transform.GetChild(0).gameObject.SetActive(true);

        // Update Unit Sprites
        unitIcon01.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unit01.unitID));
        unitIcon02.sprite = unitIcon01.sprite;

        // Update Slider Values
        totalUnitCount = unit01.howManyUnits + unit02.howManyUnits;
        slider.maxValue = totalUnitCount;
        slider.minValue = 1;
        slider.value = unit01.howManyUnits;

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, City connectedGarrison, short id01, short id02)
    {
        selectedGarrison = connectedGarrison;
        unitID_1 = id01;
        unitID_2 = id02;
        transform.GetChild(0).gameObject.SetActive(true);

        // Update Unit Sprites
        unitIcon01.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unit01.unitID));
        unitIcon02.sprite = unitIcon01.sprite;

        // Update Slider Values
        totalUnitCount = unit01.howManyUnits + unit02.howManyUnits;
        slider.maxValue = totalUnitCount;
        slider.minValue = 1;
        slider.value = unit01.howManyUnits;

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    public void UpdateDisplayedValues ()
    {
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    public void ActualizeUnitSplit ()
    {
        if (selectedArmy != null){
            selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
            if (totalUnitCount > slider.value){
                if (selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                    selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().unitID, Convert.ToInt32(totalUnitCount - slider.value), selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                }else{
                    selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                }
                
            }else{
                selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
            }
        }else if (selectedGarrison != null){
            selectedGarrison.garrisonSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
            if (totalUnitCount > slider.value){
                if (selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                    selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(selectedGarrison.garrisonSlots[unitID_1].GetComponent<UnitSlot>().unitID, Convert.ToInt32(totalUnitCount - slider.value), selectedGarrison.garrisonSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                }else{
                    selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                }
            }else{
                selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
            }
        }
        ArmyInformation.Instance.RefreshElement();
        totalUnitCount = 0;
        selectedArmy = null;
        selectedGarrison = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
