using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class ArmyInterface: MonoBehaviour
{
    [Header("UI Referances")]
    [SerializeField] List <ArmyInterfaceUnitButton> unitButtons;

    [Header ("Intefrace Information")]
    private UnitsInformation uInfo01;
    private UnitsInformation uInfo02;
    private List <GridCell> neighbourCells;
    private byte? selectedUnitIndex;
    private bool interactingWithPlaceholder;

    [Header("Events")]
    public Action onArmyInterfaceReload;

    public void ArmyInterfaceSetup(Army army, Army other = null)
    {
        uInfo01 = army.GetUnitsInformation();
        if (other == null){
            interactingWithPlaceholder = true;
            uInfo02 = new UnitsInformation();
        }else{
            interactingWithPlaceholder = false;
            uInfo02 = other.GetUnitsInformation();
        }
        
        neighbourCells = GameGrid.Instance.GetEmptyNeighbourCell(army.GetGridPosition());
        UpdateUnitDisplay();
    }

    private void ClearSelection()
    {
        RemoveButtonHighlights();
        ResetUnitSlots();
        UpdateUnitDisplay();
    }

    private void ResetUnitSlots()
    {
        uInfo01 = null;
        uInfo02 = null;
    }

    private void UpdateUnitDisplay()
    {
        for (byte i = 0; i < 7; i++){
            unitButtons[i].UpdateButton(uInfo01.GetUnit(i));
            unitButtons[i + 7].UpdateButton(uInfo02.GetUnit(i));
        }
    }

    public void RefreshElement()
    {
        UIManager.Instance.RefreshCurrentArmyDisplay();
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    public void ResetElement()
    {
        if (interactingWithPlaceholder){
            if (!uInfo02.IsArmyEmpty()){
                if (uInfo01.IsArmyEmpty()){
                    ReturnUnits();
                }else{
                    CreateNewArmy();
                }
            }
        }else{
            if (uInfo01.IsArmyEmpty()){
                WorldObjectManager.Instance.RemoveObject(uInfo01.GetConnectedObject());
            }else if(uInfo02.IsArmyEmpty()){
                WorldObjectManager.Instance.RemoveObject(uInfo02.GetConnectedObject());
            }
        }
        ClearSelection();
    }

    public void RemoveButtonHighlights()
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnitIndex = null;
    }

    public void ChangeSelectedUnit(byte slotID)
    {
        selectedUnitIndex = slotID;
    }

    private void CreateNewArmy()
    {
        if (neighbourCells.Count > 0){
            int[] unitType = new int[7];
            int[] unitCount = new int[7];
            float[] unitMovement = new float[7];
            for (byte i = 0; i < 7; i++){
                unitType[i] = uInfo02.GetUnit(i).GetId();
                unitCount[i] = uInfo02.GetUnit(i).GetUnitCount();
                unitMovement[i] = uInfo02.GetUnit(i).GetMovementPoints();
            }
            WorldObjectManager.Instance.CreateNewArmy(PlayerManager.Instance.GetCurrentPlayerTag(), neighbourCells[0].GetPosition(), unitType, unitCount);
        }else{
            Debug.Log("No available spaces");
            ReturnUnits();
        }
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
