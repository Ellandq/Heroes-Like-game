using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CityArmyInterface : MonoBehaviour
{
    [Header ("UI References")]
    [SerializeField] private List<CityUnitButton> unitButtons;

    [Header ("InterfaceInformation")]
    private UnitsInformation uInfo01;
    private UnitsInformation uInfo02;
    private List <GridCell> enteranceCells;
    private byte? selectedUnitIndex;
    private byte selectedArmy;
    private byte selectedEnterance;
    private bool interactingWithPlaceholder;

    [Header ("Events")]
    public Action onCityReload;

    public void Awake (){
        selectedArmy = 0;
        selectedEnterance = 0;
    }

    public void CityInterfaceSetup()
    {
        uInfo01 = CityManager.Instance.GetCity().GetUnitsInformation();
        List<Army> armies = CityManager.Instance.GetArmyList();
        enteranceCells = CityManager.Instance.GetAvailableEnteranceCells();
        if (armies.Count > 0){
            interactingWithPlaceholder = false;
            uInfo02 = armies[selectedArmy].GetUnitsInformation();
        }else if (enteranceCells.Count > 0){
            interactingWithPlaceholder = true;
            uInfo02 = new UnitsInformation();
        }else{
            Debug.Log("All enterances are occupied");
        }
        transform.GetChild(0).gameObject.SetActive(true);
        UpdateUnitDisplay();
    }

    // Resets the UI for army display
    private void ClearSelection()
    {
        RemoveButtonHighlights();
        uInfo01 = null;
        uInfo02 = null;
        UpdateUnitDisplay();
        transform.GetChild(0).gameObject.SetActive(false);
        UIManager.Instance.DisableUnitSplitWindow();
    }

    private void UpdateUnitDisplay()
    {
        for (byte i = 0; i < 7; i++){
            unitButtons[i].UpdateButton(uInfo01.GetUnit(i));
            unitButtons[i + 7].UpdateButton(uInfo02.GetUnit(i));
        }
    }

    public void RemoveButtonHighlights ()
    {
        onCityReload?.Invoke();
        selectedUnitIndex = null;
    }

    public void ChangeSelectedUnit(byte slotID)
    {
        selectedUnitIndex = slotID;
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
            if (!uInfo02.IsArmyEmpty()){
                CityManager.Instance.armyCreationStatus = false;
                CreateNewArmy();
            }else{
                CityManager.Instance.armyCreationStatus = true;
            }
        }else{
            if(uInfo01.IsArmyEmpty()){
                WorldObjectManager.Instance.RemoveObject(CityManager.Instance.GetArmyList()[selectedArmy]);
                CityManager.Instance.armyCreationStatus = true;
            }else{
                CityManager.Instance.armyCreationStatus = true;
            }
        }
        ClearSelection();
    }

    // Creates a new army
    private void CreateNewArmy ()
    {
        if (CityManager.Instance.availableEnteranceCells.Count > 0){
            int[] unitType = new int[7]; 
            int[] unitCount = new int[7]; 
            float[] unitMovement = new float[7]; 
            List<UnitSlot> slots = uInfo02.GetUnits();
            for (int i = 0; i < 7; i++){
                unitType[i] = slots[i].GetId();
                unitCount[i] = slots[i].GetUnitCount();
                unitMovement[i] = slots[i].GetMovementPoints();
            }
            WorldObjectManager.Instance.CreateNewArmy(CityManager.Instance.GetCity().GetPlayerTag(), 
            enteranceCells[selectedEnterance].GetPosition(), unitType, unitCount);
        }else{
            Debug.Log("No available spaces");
        }
        CityManager.Instance.armyCreationStatus = true;
    }

    private void ReturnUnits()
    {
        for (byte i = 0; i < 7; i++){
            if (uInfo02.GetUnit(i).IsEmpty()) continue;
            uInfo01.AddUnits(uInfo02.GetUnit(i));
        }
        UIManager.Instance.RefreshCurrentArmyDisplay();
    }

    public void HandleUnitInteraction (byte id01, byte id02){
        if (GetUnit(id01).IsEmpty()){
            if (!(GetUnit(id01).IsHero() || GetUnit(id02).IsHero())){
                if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                    SplitUnits(id01, id02);
                }else{
                    SwapUnits(id01, id02);
                }
            }else{
                SwapUnits(id01, id02);
            }
        }else if (!(GetUnit(id01).IsHero() || GetUnit(id02).IsHero())){
            if (GetUnit(id01).GetUnitName() == GetUnit(id02).GetUnitName()){
                if (InputManager.Instance.keyboardInput.isLeftShiftPressed){
                    SplitUnits(id01, id02);
                }else{
                    AddUnits(id01, id02);
                }
            }else{
                SwapUnits(id01, id02);
            }
        }else{
            SwapUnits(id01, id02);
        }
    }

    private UnitSlot GetUnit (byte id){
        if (id > 6){
            return uInfo02.GetUnit((byte)(id - 7));
        }
        return uInfo01.GetUnit(id);
    }

    private void SplitUnits (byte id01, byte id02){
        if (id01 > 6){
            if (id02 > 6){
                uInfo02.SplitUnits((byte)(id01 - 7), (byte)(id02 - 7));
            }else{
                uInfo02.SplitUnits((byte)(id01 - 7), id02, uInfo01);
            }
        }else{
            if (id02 > 6){
                uInfo01.SplitUnits(id01, (byte)(id02 - 7), uInfo02);
            }else{
                uInfo01.SplitUnits(id01, id02);
            }
        }
    }

    private void SwapUnits (byte id01, byte id02){
        if (id01 > 6){
            if (id02 > 6){
                uInfo02.SwapUnits((byte)(id01 - 7), (byte)(id02 - 7));
            }else{
                uInfo02.SwapUnits((byte)(id01 - 7), GetUnit(id02));
            }
        }else{
            if (id02 > 6){
                uInfo01.SwapUnits(id01, GetUnit(id02));
            }else{
                uInfo01.SwapUnits(id01, id02);
            }
        }
    }

    private void AddUnits (byte id01, byte id02){
        if (id01 > 6){
            if (id02 > 6){
                uInfo02.AddUnits((byte)(id01 - 7), (byte)(id02 - 7));
            }else{
                uInfo02.AddUnits((byte)(id01 - 7), GetUnit(id02));
            }
        }else{
            if (id02 > 6){
                uInfo01.AddUnits(id01, GetUnit(id02));
            }else{
                uInfo01.AddUnits(id01, id02);
            }
        }
    }
}