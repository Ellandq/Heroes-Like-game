using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsInformation : MonoBehaviour, IUnit
{
    private WorldObject connectedObject;
    protected List <UnitSlot> unitSlots;

    public UnitsInformation (short[] unitInfo){
        unitSlots = new List<UnitSlot>(); 
        for (short i = 0; i < 7; i++){
            unitSlots.Add(new UnitSlot());
            unitSlots[i].Initialize(this);
        }
        SetUnitStatus(unitInfo);
    }

    #region Movement points manipulation

    public void RestoreMovementPoints() {
        for (short i = 0; i < 7; i++){
            if (!unitSlots[i].IsEmpty()){
                unitSlots[i].RestoreMovementPoints();
            }
        }
    }

    #endregion

    #region Unit Manipulation

    public virtual void SwapUnitsPosition(short a, short b)
    {
        short id01 = unitSlots[a].GetId();
        short id02 = unitSlots[b].GetId();
        short unitCount01 = unitSlots[a].GetUnitCount();
        short unitCount02 = unitSlots[b].GetUnitCount();
        float mPoints01 = unitSlots[a].GetMovementPoints();
        float mPoints02 = unitSlots[b].GetMovementPoints();

        unitSlots[a].RemoveUnits();
        unitSlots[b].RemoveUnits();

        unitSlots[a].ChangeSlotStatus(id02, unitCount02, mPoints02);
        unitSlots[b].ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public virtual void SwapUnitsPosition (short a, UnitSlot otherArmyUnit)
    {
        short id01 = unitSlots[a].GetId();
        short id02 = otherArmyUnit.GetId();
        short unitCount01 = unitSlots[a].GetUnitCount();
        short unitCount02 = otherArmyUnit.GetUnitCount();
        float mPoints01 = unitSlots[a].GetMovementPoints();
        float mPoints02 = otherArmyUnit.GetMovementPoints();

        unitSlots[a].RemoveUnits();
        otherArmyUnit.RemoveUnits(); 

        unitSlots[a].ChangeSlotStatus(id02, unitCount02, mPoints02);
        otherArmyUnit.ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public virtual void AddUnits (short a, short b)
    {
        unitSlots[b].AddUnits(unitSlots[a].GetUnitCount());
        unitSlots[a].RemoveUnits();
    }

    public void AddUnits (short index, short count, short id){
        if (unitSlots[index].IsEmpty()){
            unitSlots[index].SetSlotStatus(id, count);
        }else{
            unitSlots[index].AddUnits(count);
        }
    }

    public virtual void AddUnits (short a, UnitSlot otherArmyUnit)
    {
        otherArmyUnit.AddUnits(unitSlots[a].GetUnitCount());
        unitSlots[a].RemoveUnits();
    }

    public virtual void SplitUnits (short a, short b) // Spliting with itself
    {
        UIManager.Instance.OpenUnitSplitWindow(unitSlots[a], unitSlots[b], this, a, b);
    }

    public virtual void SplitUnits (short a, short b, UnitsInformation other) // Spliting with another army
    {
        UIManager.Instance.OpenUnitSplitWindow(unitSlots[a], other.unitSlots[b], this, other, a, b);
    }

    #endregion

    #region Unit Sorting

    public void AdjustUnitPosition() {
        // Move any non-empty slots to the first available empty slot
        for (short i = 0; i < unitSlots.Count; i++) {
            if (unitSlots[i] == null) continue;
            if (unitSlots[i].IsEmpty()) continue;
            if (i == 0) continue;
            for (short j = 0; j < i; j++) {
                if (unitSlots[j] == null) {
                    SwapUnitsPosition(i, j);
                    break;
                }
                if (unitSlots[j].IsEmpty()) {
                    SwapUnitsPosition(i, j);
                    break;
                }
            }
        }

        // Sort the units based on their level, tier, and quantity

        for (short i = 0; i < unitSlots.Count; i++) {
            if (unitSlots[i] == null) continue;
            if (unitSlots[i].IsEmpty()) continue;
            for (short j = 0; j < unitSlots.Count; j++) {
                if (i == j) continue;
                if (unitSlots[j] == null) {
                    if (j < i) {
                        SwapUnitsPosition(i, j);
                    }
                    continue;
                }
                if (unitSlots[j].IsEmpty()) continue;
                if (unitSlots[i].IsHero()) {
                    if (i < j) {
                        if (unitSlots[j].IsHero()) {
                            if (unitSlots[j].heroObject.heroLevel > unitSlots[i].heroObject.heroLevel) {
                                SwapUnitsPosition(i, j);
                            }
                        }
                    }
                } else {
                    if (i < j) {
                        if (unitSlots[j].IsHero()) {
                            SwapUnitsPosition(i, j);
                        } else {
                            if (unitSlots[j].GetUnitTier() > unitSlots[i].GetUnitTier()) {
                                SwapUnitsPosition(i, j);
                            } else if (unitSlots[j].GetUnitTier() == unitSlots[i].GetUnitTier()) {
                                if (unitSlots[j].GetUnitCount() > unitSlots[i].GetUnitCount()) {
                                    SwapUnitsPosition(i, j);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    #endregion

    #region Setters

    public virtual void SetUnitStatus (short[] unitInfo){
        for (short i = 0; i < 7; i++){
            unitSlots[i].SetSlotStatus(unitInfo[i * 2], unitInfo[i * 2 + 1]);
        }
    }

    public virtual void SetSlotStatus (short index, short count, short id){

    }

    #endregion

    #region Getters

    public WorldObject GetConnectedObject () { return connectedObject; }

    public List<UnitSlot> GetUnits () { return unitSlots; }

    public short GetSameUnitSlotIndex (short id){
        short emptySlotIndex = 7;
        for (short i = 0; i < unitSlots.Count; i++){
            if (unitSlots[i].GetId() == id) return i;
            else if (unitSlots[i].IsEmpty()) emptySlotIndex = i;
        }
        return emptySlotIndex;
    }

    public List<short> GetEmptySlots(){
        List<short> list = new List<short>();
        for (short i = 0; i < 7; i++){
            if (unitSlots[i].IsEmpty()) list.Add(i);
        }
        return list;
    }

    public float GetMovementPoints(){
        float movement = 10000f;
        for (short i = 0; i < 7; i++){
            if (!unitSlots[i].IsEmpty()){
                if (unitSlots[i].GetMovementPoints() < movement){
                    movement = unitSlots[i].GetMovementPoints();
                }
            }
        }
        return movement;
    }

    public bool IsArmyEmpty (){
        foreach (UnitSlot unit in unitSlots){
            if (!unit.IsEmpty()) return false;
            else continue;
        }
        return true;
    }

    public bool AreUnitSlotsSameType (short a, short b)
    {
        if (unitSlots[a].GetId() == unitSlots[b].GetId()) return true;
        else return false;
    }

    public bool AreUnitSlotsSameType (short a, UnitSlot otherArmyUnit)
    {
        if (unitSlots[a].GetId() == otherArmyUnit.GetId()) return true;
        else return false;
    }

    #endregion
}
