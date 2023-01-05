using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CityArmyInterface : MonoBehaviour
{
    public static CityArmyInterface Instance;
    [SerializeField] List <GameObject> units01;
    [SerializeField] List <GameObject> units02;
    [SerializeField] Sprite defaultBackground;
    [SerializeField] GameObject placeholderArmy;

    public UnitSlot selectedUnit;

    [Header("UI Referances")]
    [SerializeField] List <GameObject> unitInfoSlot;
    [SerializeField] List <Button> unitInfoButtons;
    [SerializeField] List <GameObject> unitCountDisplay;

    [SerializeField] internal UnitsInformation army;
    private bool interactingWithPlaceholder;
    private List <GridCell> neighbourCells;
    public UnityEvent onArmyInterfaceReload;

    // Sets the static instance of this class
    private void Awake ()
    {
        Instance = this;
        selectedUnit = null;
    }

    // Checks all available enterences for a selectable army object
    public void GetArmyUnits()
    {
        if (CityManager.Instance.availableEnteranceCells.Count > 0){
            interactingWithPlaceholder = true;
            units01 = new List<GameObject>(CityManager.Instance.currentCityGarrison.unitSlots);
            units02 = new List<GameObject>(placeholderArmy.GetComponent<UnitsInformation>().unitSlots);
            this.transform.GetChild(0).gameObject.SetActive(true);
            UpdateUnitDisplay();
        }else{
            Debug.Log("All enterances are occupied");
        }
    }

    // takes the units from a given army
    public void GetArmyUnits(Army interactedArmy)
    {
        interactingWithPlaceholder = false;
        army = interactedArmy.GetComponent<UnitsInformation>();
        units01 = new List<GameObject>(CityManager.Instance.currentCityGarrison.unitSlots);
        units02 = new List<GameObject>(interactedArmy.unitsInformation.unitSlots);
        this.transform.GetChild(0).gameObject.SetActive(true);
        UpdateUnitDisplay();
    }

    // Resets the UI for army display
    private void ClearSelection()
    {
        RemoveButtonHighlights();
        for (int i = 0; i < 7; i++){
            units01[i] = null;
            units02[i] = null;
            placeholderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().RemoveUnits();
        }
        army = null;
        UpdateUnitDisplay();
        this.transform.GetChild(0).gameObject.SetActive(false);
        UnitSplitWindow.Instance.DisableUnitSwapWindow();
    }

    // Updates the displayed armies
    private void UpdateUnitDisplay ()
    {
        for (int i = 0; i < units01.Count; i++){
            if (units01[i] != null){
                if (!units01[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i].interactable = true;
                    unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units01[i].GetComponent<UnitSlot>().unitID));
                    unitInfoSlot[i].GetComponentInParent<CityUnitButton>().isSlotEmpty = false;
                    unitCountDisplay[i].SetActive(true);
                    unitCountDisplay[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units01[i].GetComponent<UnitSlot>().howManyUnits);

                }else{
                    unitInfoButtons[i].interactable = false;
                    unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i].GetComponentInParent<CityUnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i].SetActive(false);
                }
            }else{
                unitInfoButtons[i].interactable = false;
                unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i].GetComponentInParent<CityUnitButton>().isSlotEmpty = true;
                unitCountDisplay[i].SetActive(false);
            }
        }  

        for (int i = 0; i < units02.Count; i++){
            if (units02[i] != null){
                if (!units02[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i + 7].interactable = true;
                    unitInfoSlot[i + 7].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units02[i].GetComponent<UnitSlot>().unitID));
                    unitInfoSlot[i + 7].GetComponentInParent<CityUnitButton>().isSlotEmpty = false;
                    unitCountDisplay[i + 7].SetActive(true);
                    unitCountDisplay[i + 7].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units02[i].GetComponent<UnitSlot>().howManyUnits);

                }else{
                    unitInfoButtons[i + 7].interactable = false;
                    unitInfoSlot[i + 7].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i + 7].GetComponentInParent<CityUnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i + 7].SetActive(false);
                }
            }else{
                unitInfoButtons[i + 7].interactable = false;
                unitInfoSlot[i + 7].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i + 7].GetComponentInParent<CityUnitButton>().isSlotEmpty = true;
                unitCountDisplay[i + 7].SetActive(false);
            }
        } 
    }

    // Removes all button highlights
    public void RemoveButtonHighlights ()
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnit = null;
    }

    // Removes all button highlights
    public void RemoveButtonHighlights (Player player)
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnit = null;
    }

    // Changes the selected unit based on the gived slot ID
    public void ChangeSelectedUnit (short slotID)
    {
        if (slotID < 7){
            selectedUnit = units01[slotID].GetComponent<UnitSlot>();
        }else{
            selectedUnit = units02[slotID - 7].GetComponent<UnitSlot>();
        }
        
    }

    // Attempts to swap two different units
    public void SwapUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    CityManager.Instance.currentCityGarrison.SwapUnitsPosition(a, b);
                }else{
                    CityManager.Instance.currentCityGarrison.SwapUnitsPosition(a, army.unitSlots[b - 7]);
                }
            }else if (b < 7){
                army.SwapUnitsPosition(Convert.ToInt16(a - 7), CityManager.Instance.currentCityGarrison.unitSlots[b]);
            }else{
                army.SwapUnitsPosition(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    CityManager.Instance.currentCityGarrison.SwapUnitsPosition(a, b);
                }else{
                    CityManager.Instance.currentCityGarrison.SwapUnitsPosition(a, placeholderArmy.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                placeholderArmy.GetComponent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), CityManager.Instance.currentCityGarrison.unitSlots[b]);
            }else{
                placeholderArmy.GetComponent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
        
        RefreshElement();
    }

    // Adds one unit to another
    public void AddUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    CityManager.Instance.currentCityGarrison.AddUnits(a, b);
                }else{
                    CityManager.Instance.currentCityGarrison.AddUnits(a, army.unitSlots[b - 7]);
                }
            }else if (b < 7){
                army.AddUnits(Convert.ToInt16(a - 7), CityManager.Instance.currentCityGarrison.unitSlots[b]);
            }else{
                army.AddUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    CityManager.Instance.currentCityGarrison.AddUnits(a, b);
                }else{
                    CityManager.Instance.currentCityGarrison.AddUnits(a, placeholderArmy.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                placeholderArmy.GetComponent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), CityManager.Instance.currentCityGarrison.unitSlots[b]);
            }else{
                placeholderArmy.GetComponent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
        RefreshElement();
    }

    // Preperes two units to split 
    public void SplitUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    CityManager.Instance.currentCityGarrison.SplitUnits(a, b);
                }else{
                    CityManager.Instance.currentCityGarrison.SplitUnits(a, Convert.ToInt16(b - 7), army, army.unitSlots[b - 7]);
                }
            }else if (b < 7){
                army.SplitUnits(Convert.ToInt16(a - 7), b, CityManager.Instance.currentCityGarrison, CityManager.Instance.currentCityGarrison.unitSlots[a - 7]);
            }else{
                army.SplitUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    CityManager.Instance.currentCityGarrison.SplitUnits(a, b);
                }else{
                    CityManager.Instance.currentCityGarrison.SplitUnits(a, Convert.ToInt16(b - 7), placeholderArmy.GetComponent<UnitsInformation>(), placeholderArmy.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                placeholderArmy.GetComponent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), b, CityManager.Instance.currentCityGarrison, CityManager.Instance.currentCityGarrison.unitSlots[a - 7]);
            }else{
                placeholderArmy.GetComponent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
    }

    // Chcecks if the units are of the same type
    public bool AreUnitsSameType (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    if (CityManager.Instance.currentCityGarrison.AreUnitSlotsSameType(a, b)) return true;
                    else return false;
                }else{
                    if (CityManager.Instance.currentCityGarrison.AreUnitSlotsSameType(a, army.unitSlots[b - 7])) return true;
                    else return false;
                }
            }else if (b < 7){
                if (army.AreUnitSlotsSameType(Convert.ToInt16(a - 7), CityManager.Instance.currentCityGarrison.unitSlots[b])) return true;
                else return false;
            }else{
                if (army.AreUnitSlotsSameType(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7))) return true;
                else return false;
            }
        }else{
            if (a < 7){
                if (b < 7){
                    if (CityManager.Instance.currentCityGarrison.AreUnitSlotsSameType(a, b)) return true;
                    else return false;
                }else{
                    if (CityManager.Instance.currentCityGarrison.AreUnitSlotsSameType(a, placeholderArmy.GetComponent<UnitsInformation>().unitSlots[b - 7])) return true;
                    else return false;
                }
            }else if (b < 7){
                if (placeholderArmy.GetComponent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), CityManager.Instance.currentCityGarrison.unitSlots[b])) return true;
                else return false;
            }else{
                if (placeholderArmy.GetComponent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7))) return true;
                else return false;
            }
        }
    }

    // Refreshes this UI
    public void RefreshElement ()
    {
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    // Resets this UI element
    public void ResetElement ()
    {
        if (interactingWithPlaceholder){
            if (!IsPlaceHolderArmyEmpty()){
                CityManager.Instance.armyCreationStatus = false;
                CreateNewArmy();
            }else{
                CityManager.Instance.armyCreationStatus = true;
            }
        }else{
            if(IsArmyEmpty(army)){
                WorldObjectManager.Instance.RemoveArmy(army.gameObject);
                CityManager.Instance.armyCreationStatus = true;
            }else{
                CityManager.Instance.armyCreationStatus = true;
            }
        }
        ClearSelection();
    }

    // Checks if an Army is empty
    private bool IsArmyEmpty(UnitsInformation army)
    {
        foreach (GameObject unit in army.unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    // Checks if a placeholder army is empty
    private bool IsPlaceHolderArmyEmpty ()
    {
        foreach (GameObject unit in placeholderArmy.GetComponent<UnitsInformation>().unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    // Creates a new army
    private void CreateNewArmy ()
    {
        if (CityManager.Instance.availableEnteranceCells.Count > 0){
            int[] _unitType = new int[7]; 
            int[] _unitCount = new int[7]; 
            float[] _unitMovement = new float[7]; 
            for (int i = 0; i < 7; i++){
                _unitType[i] = placeholderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().unitID;
                _unitCount[i] = placeholderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().howManyUnits;
                _unitMovement[i] = placeholderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().movementPoints;
            }
            WorldObjectManager.Instance.CreateNewArmy(PlayerManager.Instance.currentPlayer.GetComponent<Player>().thisPlayerTag, 
            CityManager.Instance.availableEnteranceCells[0].GetPosition(), _unitType, _unitCount);
        }else{
            Debug.Log("No available spaces");
        }
        CityManager.Instance.armyCreationStatus = true;
    }
}