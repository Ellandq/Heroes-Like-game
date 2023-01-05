using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ArmyInterfaceArmyInformation : MonoBehaviour
{
    public static ArmyInterfaceArmyInformation Instance;
    [SerializeField] List <GameObject> units01;
    [SerializeField] List <GameObject> units02;
    [SerializeField] Sprite defaultBackground;
    [SerializeField] GameObject placeHolderArmy;

    public UnitSlot selectedUnit;

    [Header("UI Referances")]
    [SerializeField] List <GameObject> unitInfoSlot;
    [SerializeField] List <Button> unitInfoButtons;
    [SerializeField] List <GameObject> unitCountDisplay;

    [SerializeField] internal GameObject army01;
    [SerializeField] internal GameObject army02;
    private bool interactingWithPlaceholder;
    private List <GridCell> neighbourCells;
    public UnityEvent onArmyInterfaceReload;

    private void Awake ()
    {
        Instance = this;
        selectedUnit = null;
    }

    public void GetArmyUnits(GameObject armyObject)
    {
        CameraManager.Instance.cameraEnabled = false;
        interactingWithPlaceholder = true;
        army01 = armyObject;
        army02 = placeHolderArmy;
        units01 = new List<GameObject>(armyObject.GetComponentInParent<UnitsInformation>().unitSlots);
        units02 = new List<GameObject>(placeHolderArmy.GetComponent<UnitsInformation>().unitSlots);
        this.transform.GetChild(0).gameObject.SetActive(true);
        UpdateUnitDisplay();
        neighbourCells = GameGrid.Instance.GetEmptyNeighbourCell(army01.GetComponent<Army>().gridPosition);
    }

    public void GetArmyUnits(GameObject armyObject, GameObject interactedArmy)
    {
        CameraManager.Instance.cameraEnabled = false;
        interactingWithPlaceholder = false;
        army01 = armyObject;
        army02 = interactedArmy;
        units01 = new List<GameObject>(armyObject.GetComponentInParent<UnitsInformation>().unitSlots);
        units02 = new List<GameObject>(interactedArmy.GetComponentInParent<UnitsInformation>().unitSlots);
        this.transform.GetChild(0).gameObject.SetActive(true);
        UpdateUnitDisplay();
        neighbourCells = GameGrid.Instance.GetEmptyNeighbourCell(army01.GetComponent<Army>().gridPosition);
    }

    private void ClearSelection()
    {
        RemoveButtonHighlights();
        for (int i = 0; i < 7; i++){
            units01[i] = null;
            units02[i] = null;
            placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().RemoveUnits();
        }
        army01 = null;
        army02 = null;
        UpdateUnitDisplay();
        this.transform.GetChild(0).gameObject.SetActive(false);
        UnitSplitWindow.Instance.DisableUnitSwapWindow();
    }

    private void UpdateUnitDisplay ()
    {
        for (int i = 0; i < units01.Count; i++){
            if (units01[i] != null){
                if (!units01[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i].interactable = true;
                    unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units01[i].GetComponent<UnitSlot>().unitID));
                    unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = false;
                    unitCountDisplay[i].SetActive(true);
                    unitCountDisplay[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units01[i].GetComponent<UnitSlot>().howManyUnits);

                }else{
                    unitInfoButtons[i].interactable = false;
                    unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i].SetActive(false);
                }
            }else{
                unitInfoButtons[i].interactable = false;
                unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                unitCountDisplay[i].SetActive(false);
            }
        }  

        for (int i = 0; i < units02.Count; i++){
            if (units02[i] != null){
                if (!units02[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i + 7].interactable = true;
                    unitInfoSlot[i + 7].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units02[i].GetComponent<UnitSlot>().unitID));
                    unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = false;
                    unitCountDisplay[i + 7].SetActive(true);
                    unitCountDisplay[i + 7].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units02[i].GetComponent<UnitSlot>().howManyUnits);

                }else{
                    unitInfoButtons[i + 7].interactable = false;
                    unitInfoSlot[i + 7].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i + 7].SetActive(false);
                }
            }else{
                unitInfoButtons[i + 7].interactable = false;
                unitInfoSlot[i + 7].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                unitCountDisplay[i + 7].SetActive(false);
            }
        } 
    }

    public void RemoveButtonHighlights ()
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnit = null;
    }

    public void RemoveButtonHighlights (Player player)
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnit = null;
    }

    public void ChangeSelectedUnit (short slotID)
    {
        if (slotID < 7){
            selectedUnit = units01[slotID].GetComponent<UnitSlot>();
        }else{
            selectedUnit = units02[slotID - 7].GetComponent<UnitSlot>();
        }
        
    }

    public void SwapUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, army02.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
        
        RefreshElement();
    }

    public void AddUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponentInParent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponentInParent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, army02.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
        RefreshElement();
    }

    public void SplitUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, Convert.ToInt16(b - 7), army02.GetComponentInParent<UnitsInformation>(), army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponentInParent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), b, army01.GetComponentInParent<UnitsInformation>(), army01.GetComponentInParent<UnitsInformation>().unitSlots[a - 7]);
            }else{
                army02.GetComponentInParent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, Convert.ToInt16(b - 7), army02.GetComponent<UnitsInformation>(), army02.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), b, army01.GetComponentInParent<UnitsInformation>(), army01.GetComponentInParent<UnitsInformation>().unitSlots[a - 7]);
            }else{
                army02.GetComponent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
    }

    public bool AreUnitsSameType (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, b)) return true;
                    else return false;
                }else{
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7])) return true;
                    else return false;
                }
            }else if (b < 7){
                if (army02.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b])) return true;
                else return false;
            }else{
                if (army02.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7))) return true;
                else return false;
            }
        }else{
            if (a < 7){
                if (b < 7){
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, b)) return true;
                    else return false;
                }else{
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, army02.GetComponent<UnitsInformation>().unitSlots[b - 7])) return true;
                    else return false;
                }
            }else if (b < 7){
                if (army02.GetComponent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b])) return true;
                else return false;
            }else{
                if (army02.GetComponent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7))) return true;
                else return false;
            }
        }
    }

    public void RefreshElement ()
    {
        ArmyInformation.Instance.RefreshElement();
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    public void ResetElement ()
    {
        if (interactingWithPlaceholder){
            if (!IsPlaceHolderArmyEmpty()){
                if (army01.GetComponent<Army>().IsArmyEmpty()){
                    ReturnUnits();
                }else{
                    CreateNewArmy();
                }
            }
        }else{
            if (IsArmyEmpty(army01)){
                WorldObjectManager.Instance.RemoveArmy(army01);
            }else if(IsArmyEmpty(army02)){
                WorldObjectManager.Instance.RemoveArmy(army02);
            }
        }
        ClearSelection();
        CameraManager.Instance.cameraEnabled = true;
    }

    private bool IsArmyEmpty(GameObject army)
    {
        foreach (GameObject unit in army.GetComponent<Army>().unitsInformation.unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    private bool IsPlaceHolderArmyEmpty ()
    {
        foreach (GameObject unit in placeHolderArmy.GetComponent<UnitsInformation>().unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    private void CreateNewArmy ()
    {
        if (neighbourCells.Count > 0){
            int[] _unitType = new int[7]; 
            int[] _unitCount = new int[7]; 
            float[] _unitMovement = new float[7]; 
            for (int i = 0; i < 7; i++){
                _unitType[i] = placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().unitID;
                _unitCount[i] = placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().howManyUnits;
                _unitMovement[i] = placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().movementPoints;
            }
            WorldObjectManager.Instance.CreateNewArmy(PlayerManager.Instance.currentPlayer.GetComponent<Player>().thisPlayerTag, 
            neighbourCells[0].GetPosition(), _unitType, _unitCount);
        }else{
            Debug.Log("No available spaces");
        }
    }

    private void ReturnUnits ()
    {
        for (int i = 0; i < 7; i++){
            if (placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().slotEmpty) continue;
            army01.GetComponent<Army>().unitsInformation.unitSlots[i].GetComponent<UnitSlot>().ChangeSlotStatus(placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().unitID, 
            placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().howManyUnits, placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().movementPoints);
        }
        ArmyInformation.Instance.RefreshElement();
    }
}