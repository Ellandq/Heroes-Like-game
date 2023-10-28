using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsInformation : MonoBehaviour, IUnit
{
    private WorldObject connectedObject;
    protected List <UnitSlot> unitSlots;

    public UnitsInformation (int[] unitInfo){
        unitSlots = new List<UnitSlot>(); 
        for (int i = 0; i < 7; i++){
            unitSlots.Add(new UnitSlot());
            unitSlots[i].Initialize(this);
        }
        SetUnitStatus(unitInfo);
    }

    #region Movement points manipulation

    public void RestoreMovementPoints() {
        for (int i = 0; i < 7; i++){
            if (!unitSlots[i].slotEmpty){
                unitSlots[i].RestoreMovementPoints();
            }
        }
    }

    #endregion

    #region Unit Manipulation

    public virtual void SwapUnitsPosition(short a, short b)
    {
        int id01 = unitSlots[a].unitID;
        int id02 = unitSlots[b].unitID;
        int unitCount01 = unitSlots[a].howManyUnits;
        int unitCount02 = unitSlots[b].howManyUnits;
        float mPoints01 = unitSlots[a].movementPoints;
        float mPoints02 = unitSlots[b].movementPoints;

        unitSlots[a].RemoveUnits();
        unitSlots[b].RemoveUnits();

        unitSlots[a].ChangeSlotStatus(id02, unitCount02, mPoints02);
        unitSlots[b].ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public virtual void SwapUnitsPosition (short a, UnitSlot otherArmyUnit)
    {
        int id01 = unitSlots[a].unitID;
        int id02 = otherArmyUnit.unitID;
        int unitCount01 = unitSlots[a].howManyUnits;
        int unitCount02 = otherArmyUnit.howManyUnits;
        float mPoints01 = unitSlots[a].movementPoints;
        float mPoints02 = otherArmyUnit.movementPoints;

        unitSlots[a].RemoveUnits();
        otherArmyUnit.RemoveUnits(); 

        unitSlots[a].ChangeSlotStatus(id02, unitCount02, mPoints02);
        otherArmyUnit.ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public virtual void AddUnits (short a, short b)
    {
        unitSlots[b].howManyUnits += unitSlots[a].howManyUnits;
        unitSlots[a].RemoveUnits();
    }

    public virtual void AddUnits (short a, UnitSlot otherArmyUnit)
    {
        otherArmyUnit.howManyUnits += unitSlots[a].howManyUnits;
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
            if (unitSlots[i].slotEmpty) continue;
            if (i == 0) continue;
            for (short j = 0; j < i; j++) {
                if (unitSlots[j] == null) {
                    SwapUnitsPosition(i, j);
                    break;
                }
                if (unitSlots[j].slotEmpty) {
                    SwapUnitsPosition(i, j);
                    break;
                }
            }
        }

        // Sort the units based on their level, tier, and quantity

        for (short i = 0; i < unitSlots.Count; i++) {
            if (unitSlots[i] == null) continue;
            if (unitSlots[i].slotEmpty) continue;
            for (short j = 0; j < unitSlots.Count; j++) {
                if (i == j) continue;
                if (unitSlots[j] == null) {
                    if (j < i) {
                        SwapUnitsPosition(i, j);
                    }
                    continue;
                }
                if (unitSlots[j].slotEmpty) continue;
                if (unitSlots[i].isSlotHero) {
                    if (i < j) {
                        if (unitSlots[j].isSlotHero) {
                            if (unitSlots[j].heroObject.heroLevel > unitSlots[i].heroObject.heroLevel) {
                                SwapUnitsPosition(i, j);
                            }
                        }
                    }
                } else {
                    if (i < j) {
                        if (unitSlots[j].isSlotHero) {
                            SwapUnitsPosition(i, j);
                        } else {
                            if (unitSlots[j].unitObject.unitTier > unitSlots[i].unitObject.unitTier) {
                                SwapUnitsPosition(i, j);
                            } else if (unitSlots[j].unitObject.unitTier == unitSlots[i].unitObject.unitTier) {
                                if (unitSlots[j].howManyUnits > unitSlots[i].howManyUnits) {
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

    public virtual void SetUnitStatus (int[] unitInfo){
        for (int i = 0; i < 7; i++){
            unitSlots[i].SetSlotStatus(unitInfo[i * 2], unitInfo[i * 2 + 1]);
        }
    }

    #endregion

    #region Getters

    public WorldObject GetConnectedObject () { return connectedObject; }

    public List<UnitSlot> GetUnits () { return unitSlots; }

    public float GetMovementPoints (){
        float movement = 10000f;
        for (int i = 0; i < 7; i++){
            if (!unitSlots[i].slotEmpty){
                if (unitSlots[i].GetMovementPoints() < movement){
                    movement = unitSlots[i].GetMovementPoints();
                }
            }
        }
        return movement;
    }

    public bool IsArmyEmpty (){
        foreach (UnitSlot unit in unitSlots){
            if (!unit.slotEmpty) return false;
            else continue;
        }
        return true;
    }

    public bool AreUnitSlotsSameType (short a, short b)
    {
        if (unitSlots[a].unitID == unitSlots[b].unitID) return true;
        else return false;
    }

    public bool AreUnitSlotsSameType (short a, UnitSlot otherArmyUnit)
    {
        if (unitSlots[a].unitID == otherArmyUnit.unitID) return true;
        else return false;
    }

    #endregion
}
