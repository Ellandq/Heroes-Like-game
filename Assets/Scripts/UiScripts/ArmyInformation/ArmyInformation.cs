using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ArmyInformation : MonoBehaviour
{
    public static ArmyInformation Instance;
    [SerializeField] List <GameObject> units;
    [SerializeField] Sprite defaultBackground;

    public UnitSlot selectedUnit;

    [Header("UI Referances")]
    [SerializeField] List <GameObject> unitInfoSlot;
    [SerializeField] List <Button> unitInfoButtons;
    [SerializeField] List <GameObject> unitCountDisplay;

    internal GameObject selectedArmy;
    private string unitIconsFilePath;

    public UnityEvent onUnitDisplayReload;

    void Start ()
    {
        Instance = this;
        ObjectSelector.Instance.onSelectedObjectChange.AddListener(ChangeSelectedArmy);
        TurnManager.OnNewPlayerTurn += RemoveButtonHighlights;
        selectedUnit = null;

    }

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

    private void GetArmyUnits(GameObject armyObject)
    {
        selectedArmy = armyObject;
        units = new List<GameObject>(armyObject.GetComponentInParent<Army>().unitSlots);
        UpdateUnitDisplay();
    }

    private void GetCityGarrison(GameObject cityObject)
    {
        selectedArmy = cityObject;
        units = new List<GameObject>(cityObject.GetComponent<City>().garrisonSlots);
        UpdateUnitDisplay();
    }

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

    private void UpdateUnitDisplay ()
    {
        for (int i = 0; i < units.Count; i++){
            if (units[i] != null){
                if (!units[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i].interactable = true;
                    unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units[i].GetComponent<UnitSlot>().unitID));
                    unitInfoSlot[i].GetComponentInParent<UnitButton>().isSlotEmpty = false;
                    unitCountDisplay[i].SetActive(true);
                    unitCountDisplay[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units[i].GetComponent<UnitSlot>().howManyUnits);

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

    public void RemoveButtonHighlights ()
    {
        onUnitDisplayReload?.Invoke();
        selectedUnit = null;
    }

    public void RemoveButtonHighlights (Player player)
    {
        onUnitDisplayReload?.Invoke();
        selectedUnit = null;
    }

    public void ChangeSelectedUnit (short slotID)
    {
        selectedUnit = units[slotID].GetComponent<UnitSlot>();
    }

    public void SwapUnits (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            selectedArmy.GetComponentInParent<Army>().SwapUnitsPosition(a, b);
        }else{
            selectedArmy.GetComponentInParent<City>().SwapUnitsPosition(a, b);
        }
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    public void AddUnits (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            selectedArmy.GetComponentInParent<Army>().AddUnits(a, b);
        }else{
            selectedArmy.GetComponentInParent<City>().AddUnits(a, b);
        }
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    public void SplitUnits (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            selectedArmy.GetComponentInParent<Army>().SplitUnits(a, b);
        }else{
            selectedArmy.GetComponentInParent<City>().SplitUnits(a, b);
        }
    }

    public bool AreUnitsSameType (short a, short b)
    {
        if (selectedArmy.tag == "Army"){
            if (selectedArmy.GetComponentInParent<Army>().AreUnitSlotsSameType(a, b))return true;
            else return false;
        }else{
            if (selectedArmy.GetComponentInParent<City>().AreGarrisonSlotsSameType(a, b))return true;
            else return false;
        }
    }

    public void RefreshElement ()
    {
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }
}