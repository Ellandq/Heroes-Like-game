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
    [SerializeField] private Image unitIcon01;
    [SerializeField] private Image unitIcon02;

    [Header("Unit Count Objects")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI unitCount01;
    [SerializeField] private TextMeshProUGUI unitCount02;
    private int totalUnitCount;
    private Army selectedArmy;
    private Army interactedArmy;
    [SerializeField] private PlaceHolderArmy placeHolderArmy;
    private City selectedGarrison;
    private short unitID_1;
    private short unitID_2;

    private SplitWith splitWith;

    private void Start ()
    {
        Instance = this;
        totalUnitCount = 0;
        selectedArmy = null;
        interactedArmy = null;
        selectedGarrison = null;
    }

    // Unit Swap for a single Army 
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, Army connectedArmy, short id01, short id02)
    {
        splitWith = SplitWith.sameArmy;
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

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Unit Swap with another army
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, Army connectedArmy, Army armyInteractedWith, short id01, short id02)
    {
        splitWith = SplitWith.otherArmy;
        selectedArmy = connectedArmy;
        interactedArmy = armyInteractedWith;
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

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Unit Swap with a placeholder army 
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, Army connectedArmy, PlaceHolderArmy armyInteractedWith, short id01, short id02)
    {
        splitWith = SplitWith.placeholderArmy;
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

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Unit Swap within a placeholder army 
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, short id01, short id02)
    {
        splitWith = SplitWith.inPlaceholderArmy;
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

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Unit Swap from a placeholder army 
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, PlaceHolderArmy placeholder, Army armyInteractedWith, short id01, short id02)
    {
        splitWith = SplitWith.fromPlaceholderArmy;
        interactedArmy = armyInteractedWith;
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

        if (slider.value == totalUnitCount){
            slider.value -= 1;
        }

        // Update displayed unit count
        unitCount01.text = Convert.ToString(slider.value);
        unitCount02.text = Convert.ToString(totalUnitCount - slider.value);
    }

    // Unit Swap for a city garrison 
    public void PrepareUnitsToSwap (UnitSlot unit01, UnitSlot unit02, City connectedGarrison, short id01, short id02)
    {
        splitWith = SplitWith.sameCity;
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

        #region Same Army
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
        #endregion

        #region Other Army
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
        #endregion

        #region With Placeholder Army
            case SplitWith.placeholderArmy:
                selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
                if (totalUnitCount > slider.value){
                    if (placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                        placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().unitID, 
                        Convert.ToInt32(totalUnitCount - slider.value), selectedArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                    }else{
                        placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                    }
                }else{
                    placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
                }
            break;
        #endregion

        #region Within Placeholder Army
            case SplitWith.inPlaceholderArmy:
                placeHolderArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
                if (totalUnitCount > slider.value){
                    if (placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                        placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(placeHolderArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().unitID, 
                        Convert.ToInt32(totalUnitCount - slider.value), placeHolderArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                    }else{
                        placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                    }
                }else{
                    placeHolderArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
                }
            break;
        #endregion

        #region From Placeholder Army
            case SplitWith.fromPlaceholderArmy:
                placeHolderArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
                if (totalUnitCount > slider.value){
                    if (interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                        interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(placeHolderArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().unitID, 
                        Convert.ToInt32(totalUnitCount - slider.value), placeHolderArmy.unitSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                    }else{
                        interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                    }
                }else{
                    interactedArmy.unitSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
                }
            break;
        #endregion
        
        #region Same City
            case SplitWith.sameCity:
                selectedGarrison.garrisonSlots[unitID_1].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(slider.value);
                if (totalUnitCount > slider.value){
                    if (selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().slotEmpty){
                        selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().ChangeSlotStatus(selectedGarrison.garrisonSlots[unitID_1].GetComponent<UnitSlot>().unitID, 
                        Convert.ToInt32(totalUnitCount - slider.value), selectedGarrison.garrisonSlots[unitID_1].GetComponent<UnitSlot>().movementPoints);
                    }else{
                        selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().howManyUnits = Convert.ToInt32(totalUnitCount - slider.value);
                    }
                }else{
                    selectedGarrison.garrisonSlots[unitID_2].GetComponent<UnitSlot>().RemoveUnits();
                }
            break;
        #endregion

        #region Other City
            case SplitWith.otherCity:

            break;
        #endregion

        #region Placeholder City
            case SplitWith.placeholderCity:

            break;
        #endregion

        #region Within Placeholder City
            case SplitWith.inPlaceholderCity:

            break;
        #endregion

        #region From Placeholder City
            case SplitWith.fromPlaceholderCity:

            break;
        #endregion

            default:
            throw new ArgumentOutOfRangeException(nameof(splitWith), splitWith, null);
        }

        ArmyInformation.Instance.RefreshElement();
        ArmyInterfaceArmyInformation.Instance.RefreshElement();
        totalUnitCount = 0;
        selectedArmy = null;
        selectedGarrison = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void DisableUnitSwapWindow ()
    {
        totalUnitCount = 0;
        selectedArmy = null;
        selectedGarrison = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private enum SplitWith{
        sameArmy,
        otherArmy,
        sameCity,
        otherCity,
        placeholderArmy,
        inPlaceholderArmy,
        fromPlaceholderArmy,
        placeholderCity,
        inPlaceholderCity,
        fromPlaceholderCity
    }
}
