using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;
using System.ComponentModel;

public class ArmyDisplayInformation : MonoBehaviour
{
    public static ArmyDisplayInformation Instance;

    [Header ("Units information")]
    private UnitsInformation units;

    [Header ("UI Sprites")]
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite bannerOpen;
    [SerializeField] private Sprite bannerClosed;

    [Header("UI Referances")]
    [SerializeField] private List <UnitPanel> panels;

    private void Awake (){
        Instance = this;   
    }

    private void Start (){
        ObjectSelector.Instance.onSelectedObjectChange.AddListener(ChangeSelectedUnits);
        units = null;
    }

    // Changes the selected army
    private void ChangeSelectedUnits (){
        UnitsInformation units = ObjectSelector.Instance.GetSelectedObjectUnits();
        if (units!= null){
            UpdateUnitDisplay();
        }else{
            ClearSelection();
        }
    }

    // Clears the current displayed units
    private void ClearSelection(){
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    // Updates the unit display
    private void UpdateUnitDisplay (){
        int index = 0;
        foreach (UnitSlot slot in units.GetUnits()){
            panels[index].UpdateUnitPanel(slot);
            index++;
        }
    }

    // Removes all button highlighs
    public void RemoveButtonHighlights (){
        foreach (UnitPanel panel in panels){
            panel.RemoveHighlight();
        }
    }

    // Swaps units with given id's
    public void SwapUnits (byte a, byte b){
        if (units != null){
            if (AreUnitsSameType(a, b)) AddUnits(a, b);
            else units.SwapUnitsPosition(a, b);
        }
        RefreshElement();
    }

    // Adds one unit to another
    public void AddUnits (short a, short b){
        units.AddUnits(a, b);
        RefreshElement();
    }

    // Splits two given units and or a unit with an empty cell
    public void SplitUnits (short a, short b){
        units.SplitUnits(a, b);
    }

    // Checks if the given units are of the same type
    public bool AreUnitsSameType (short a, short b){
        return units.AreUnitSlotsSameType(a, b);
    }

    // Refreshes the UI element
    public void RefreshElement ()
    {
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }
}