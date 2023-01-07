using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class ArmyInformation : MonoBehaviour
{
    public static ArmyInformation Instance;
    [SerializeField] private List <GameObject> units;
    [SerializeField] private Sprite defaultBackground;

    public UnitSlot selectedUnit;

    [Header("UI Referances")]
    [SerializeField] private List <GameObject> unitInfoSlot;
    [SerializeField] private List <Button> unitInfoButtons;
    [SerializeField] private List <GameObject> unitCountDisplay;

    internal GameObject selectedArmy;
    private string unitIconsFilePath;

    public UnityEvent onUnitDisplayReload;

    private void Start ()
    {
        Instance = this;
        ObjectSelector.Instance.onSelectedObjectChange.AddListener(ChangeSelectedArmy);
        TurnManager.OnNewPlayerTurn += RemoveButtonHighlights;
        selectedUnit = null;

    }

    // Changes the selected army
    private void ChangeSelectedArmy ()
    {
        if (ObjectSelector.Instance.lastObjectSelected != null ){
            if (ObjectSelector.Instance.lastObjectSelected.tag == "Army"){
                GetArmyUnits(ObjectSelector.Instance.lastObjectSelected);
            }else if (ObjectSelector.Instance.lastObjectSelected.tag == "City"){
                GetCityGarrison(ObjectSelector.Instance.lastObjectSelected);
            }else{
                Debug.Log(ObjectSelector.Instance.lastObjectSelected.tag);
                ClearSelection();
            }
        }else{
            ClearSelection();
        }
    }

    // Takes the unit list from an army GameObject
    private void GetArmyUnits(GameObject armyObject)
    {
        selectedArmy = armyObject;
        units = new List<GameObject>(armyObject.GetComponentInParent<UnitsInformation>().unitSlots);
        UpdateUnitDisplay();
    }

    // Takes the unit list from a city GameObject
    private void GetCityGarrison(GameObject cityObject)
    {
        selectedArmy = cityObject;
        units = new List<GameObject>(cityObject.GetComponent<City>().garrisonSlots);
        UpdateUnitDisplay();
    }

    // Clears the current displayed units
    private void ClearSelection()
    {
        RemoveButtonHighlights();
        units[0] = null;
        units[1] = null;
        units[2] = null;
        units[3] = null;
        units[4] = null;
        units[5] = null;
        units[6] = null;
        UpdateUnitDisplay();
    }

    // Updates the unit display
    private void UpdateUnitDisplay ()
    {
        for (int i = 0; i < units.Count; i++){
            if (units[i] != null){
                if (!units[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i].interactable = true;
                    // Check if the selected unit is a hero
                    if (units[i].GetComponent<UnitSlot>().isSlotHero){
                        unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), units[i].GetComponent<UnitSlot>().unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
                        unitInfoSlot[i].GetComponentInParent<UnitButton>().isSlotEmpty = false;
                        unitCountDisplay[i].SetActive(false);
                    }else{
                        unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units[i].GetComponent<UnitSlot>().unitID));
                        unitInfoSlot[i].GetComponentInParent<UnitButton>().isSlotEmpty = false;
                        unitCountDisplay[i].SetActive(true);
                        unitCountDisplay[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units[i].GetComponent<UnitSlot>().howManyUnits);
                    }

                }else{
                    unitInfoButtons[i].interactable = false;
                    unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i].GetComponentInParent<UnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i].SetActive(false);
                }
            }else{
                unitInfoButtons[i].interactable = false;
                unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i].GetComponentInParent<UnitButton>().isSlotEmpty = true;
                unitCountDisplay[i].SetActive(false);
            }
        }  
    }

    // Check if any of the units on given positions are heroes
    public bool AreUnitsHeroes(int id01, int id02)
    {
        if (units[id01].GetComponent<UnitSlot>().isSlotHero || units[id02].GetComponent<UnitSlot>().isSlotHero){
            return true;
        }else{
            return false;
        }
    }

    // Removes all button highlighs
    public void RemoveButtonHighlights ()
    {
        onUnitDisplayReload?.Invoke();
        selectedUnit = null;
    }

    // Removes all button highlights on a new turn
    public void RemoveButtonHighlights (Player player)
    {
        onUnitDisplayReload?.Invoke();
        selectedUnit = null;
    }

    // Changes the selected unit
    public void ChangeSelectedUnit (short slotID)
    {
        selectedUnit = units[slotID].GetComponent<UnitSlot>();
    }

    // Swaps units with given id's
    public void SwapUnits (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            selectedArmy.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, b);
        }else{
            selectedArmy.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, b);
        }
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    // Adds one unit to another
    public void AddUnits (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            selectedArmy.GetComponentInParent<UnitsInformation>().AddUnits(a, b);
        }else{
            selectedArmy.GetComponentInParent<UnitsInformation>().AddUnits(a, b);
        }
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    // Splits two given units and or a unit with an empty cell
    public void SplitUnits (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            selectedArmy.GetComponentInParent<UnitsInformation>().SplitUnits(a, b);
        }else{
            selectedArmy.GetComponentInParent<UnitsInformation>().SplitUnits(a, b);
        }
    }

    // Checks if the given units are of the same type
    public bool AreUnitsSameType (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            if (selectedArmy.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, b))return true;
            else return false;
        }else{
            if (selectedArmy.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, b))return true;
            else return false;
        }
    }

    // Refreshes the UI element
    public void RefreshElement ()
    {
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }
}