using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSplitWindow : MonoBehaviour
{
    [Header ("Unit Icon")]
    [SerializeField] private Image unitIcon;

    [Header("Unit Count Objects")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI unitCount01;
    [SerializeField] private TextMeshProUGUI unitCount02;
    [SerializeField] private int totalUnitCount;
    private UnitsInformation selectedArmy;
    private UnitsInformation interactedArmy;
    private City interactedCity;
    private City selectedGarrison;
    private short unitID_1;
    private short unitID_2;

    private SplitWith splitWith;

    // Unit Swap from Army to Itself
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, UnitsInformation connectedArmy, short id01, short id02)
    {
        splitWith = SplitWith.sameArmy;
        selectedArmy = connectedArmy;
        unitID_1 = id01;
        unitID_2 = id02;
        gameObject.SetActive(true);

        // Update Unit Sprites
        unitIcon.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unit01.unitID));

        // Update Slider Values
        totalUnitCount = unit01.howManyUnits + unit02.howManyUnits;
        slider.maxValue = totalUnitCount;
        slider.minValue = 1;
        slider.value = unit01.howManyUnits;

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Unit Swap with another army
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, UnitsInformation connectedArmy, UnitsInformation armyInteractedWith, short id01, short id02)
    {
        splitWith = SplitWith.otherArmy;
        selectedArmy = connectedArmy;
        interactedArmy = armyInteractedWith;
        unitID_1 = id01;
        unitID_2 = id02;
        gameObject.SetActive(true);

        // Update Unit Sprites
        unitIcon.sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unit01.unitID));

        // Update Slider Values
        totalUnitCount = unit01.howManyUnits + unit02.howManyUnits;
        slider.maxValue = totalUnitCount;
        slider.minValue = 1;
        slider.value = unit01.howManyUnits;

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    public void UpdateDisplayedValues ()
    {
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Finalize the unit split depending on the interacting objects
    public void ActualizeUnitSplit ()
    {
        switch (splitWith){

            case SplitWith.sameArmy:
                selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
                if (totalUnitCount > slider.value){
                    if (selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                        selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().unitID, 
                        Convert.ToInt32(totalUnitCount - slider.value), selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                    }else{
                        selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                    }
                }else{
                    selectedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
                }
            break;

            case SplitWith.otherArmy:
                selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
                if (totalUnitCount > slider.value){
                    if (interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                        interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().unitID, 
                        Convert.ToInt32(totalUnitCount - slider.value), selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                    }else{
                        interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                    }
                }else{
                    interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
                }
            break;

            default:
            throw new ArgumentOutOfRangeException(nameof(splitWith), splitWith, null);
        }

        if (GameManager.Instance.IsSceneOpened()){
            CityArmyInterface.Instance.RefreshElement();
            totalUnitCount = 0;
            selectedArmy = null;
            selectedGarrison = null;
            gameObject.SetActive(false);
        }else{
            selectedArmy.AdjustUnitPosition();
            if (interactedArmy != null) interactedArmy.AdjustUnitPosition();
            
            UIManager.Instance.RefreshCurrentArmyDisplay();
            UIManager.Instance.RefreshArmyInterface();
            totalUnitCount = 0;
            selectedArmy = null;
            selectedGarrison = null;
            gameObject.SetActive(false);

            
        }
    }

    public void DisableUnitSplitWindow ()
    {
        totalUnitCount = 0;
        selectedArmy = null;
        selectedGarrison = null;
        gameObject.SetActive(false);
    }

    private enum SplitWith{
        sameArmy,
        otherArmy,
    }
}
