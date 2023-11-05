using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsInformation : MonoBehaviour, IUnit
{
    private WorldObject connectedObject;
    protected List<UnitSlot> unitSlots;

    public UnitsInformation(){
        unitSlots = new List<UnitSlot>();
        for (short i = 0; i < 7; i++)
        {
            unitSlots.Add(new UnitSlot());
            unitSlots[i].Initialize(this);
        }
        SetUnitStatus(new short[14]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
    }

    public UnitsInformation(short[] unitInfo)
    {
        unitSlots = new List<UnitSlot>();
        for (short i = 0; i < 7; i++)
        {
            unitSlots.Add(new UnitSlot());
            unitSlots[i].Initialize(this);
        }
        SetUnitStatus(unitInfo);
    }

    #region Movement points manipulation

    public void RestoreMovementPoints()
    {
        for (byte i = 0; i < 7; i++)
        {
            if (!unitSlots[i].IsEmpty())
            {
                unitSlots[i].RestoreMovementPoints();
            }
        }
    }

    #endregion

    #region Unit Manipulation

    public virtual void SwapUnits(byte a, byte b)
    {
        short id01 = unitSlots[a].GetId();
        short id02 = unitSlots[b].GetId();
        int unitCount01 = unitSlots[a].GetUnitCount();
        int unitCount02 = unitSlots[b].GetUnitCount();
        float mPoints01 = unitSlots[a].GetMovementPoints();
        float mPoints02 = unitSlots[b].GetMovementPoints();

        unitSlots[a].RemoveUnits();
        unitSlots[b].RemoveUnits();

        unitSlots[a].ChangeSlotStatus(id02, unitCount02, mPoints02);
        unitSlots[b].ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public virtual void SwapUnits(byte a, UnitSlot otherArmyUnit)
    {
        short id01 = unitSlots[a].GetId();
        short id02 = otherArmyUnit.GetId();
        int unitCount01 = unitSlots[a].GetUnitCount();
        int unitCount02 = otherArmyUnit.GetUnitCount();
        float mPoints01 = unitSlots[a].GetMovementPoints();
        float mPoints02 = otherArmyUnit.GetMovementPoints();

        unitSlots[a].RemoveUnits();
        otherArmyUnit.RemoveUnits();

        unitSlots[a].ChangeSlotStatus(id02, unitCount02, mPoints02);
        otherArmyUnit.ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public virtual void AddUnits(byte a, byte b)
    {
        unitSlots[b].AddUnits(unitSlots[a].GetUnitCount());
        unitSlots[a].RemoveUnits();
    }

    public void AddUnits(byte index, short count, short id)
    {
        if (unitSlots[index].IsEmpty())
        {
            unitSlots[index].SetSlotStatus(id, count);
        }
        else
        {
            unitSlots[index].AddUnits(count);
        }
    }

    public virtual void AddUnits(byte a, UnitSlot otherArmyUnit)
    {
        otherArmyUnit.AddUnits(unitSlots[a].GetUnitCount());
        unitSlots[a].RemoveUnits();
    }

    public virtual void AddUnits (UnitSlot slot){
        for (byte i = 0; i < 7; i++){
            if (AreUnitSlotsSameType(i, slot)){
                AddUnits(i, slot);
                return;
            }
        }

        for (byte i = 0; i < 7; i++){
            if (unitSlots[i].IsEmpty()){
                AddUnits(i, slot);
                return;
            }
        }

        Debug.LogError("No available unit slots. ");
    }

    public virtual void SplitUnits(byte a, byte b, UnitsInformation other = null)
    {
        UIManager.Instance.OpenUnitSplitWindow(this, a, b, other);
    }

    #endregion

    #region Unit Sorting

    public void AdjustUnitPosition()
    {
        for (byte i = 0; i < unitSlots.Count; i++)
        {
            if (unitSlots[i] == null) continue;
            if (unitSlots[i].IsEmpty()) continue;
            if (i == 0) continue;
            for (byte j = 0; j < i; j++)
            {
                if (unitSlots[j] == null)
                {
                    SwapUnits(i, j);
                    break;
                }
                if (unitSlots[j].IsEmpty())
                {
                    SwapUnits(i, j);
                    break;
                }
            }
        }

        for (byte i = 0; i < unitSlots.Count; i++)
        {
            if (unitSlots[i] == null) continue;
            if (unitSlots[i].IsEmpty()) continue;
            for (byte j = 0; j < unitSlots.Count; j++)
            {
                if (i == j) continue;
                if (unitSlots[j] == null)
                {
                    if (j < i)
                    {
                        SwapUnits(i, j);
                    }
                    continue;
                }
                if (unitSlots[j].IsEmpty()) continue;
                if (unitSlots[i].IsHero())
                {
                    if (i < j)
                    {
                        if (unitSlots[j].IsHero())
                        {
                            if (unitSlots[j].heroObject.heroLevel > unitSlots[i].heroObject.heroLevel)
                            {
                                SwapUnits(i, j);
                            }
                        }
                    }
                }
                else
                {
                    if (i < j)
                    {
                        if (unitSlots[j].IsHero())
                        {
                            SwapUnits(i, j);
                        }
                        else
                        {
                            if (unitSlots[j].GetUnitTier() > unitSlots[i].GetUnitTier())
                            {
                                SwapUnits(i, j);
                            }
                            else if (unitSlots[j].GetUnitTier() == unitSlots[i].GetUnitTier())
                            {
                                if (unitSlots[j].GetUnitCount() > unitSlots[i].GetUnitCount())
                                {
                                    SwapUnits(i, j);
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

    public virtual void SetUnitStatus(short[] unitInfo)
    {
        for (short i = 0; i < 7; i++)
        {
            unitSlots[i].SetSlotStatus(unitInfo[i * 2], unitInfo[i * 2 + 1]);
        }
    }

    public virtual void SetSlotStatus(byte index, int count, short id)
    {
        unitSlots[index].SetSlotStatus(id, count);
    }

    public virtual void SetUnitCount (byte index, int count){
        unitSlots[index].SetUnitCount(count);
    }

    public virtual void ClearArmy (){
        foreach (UnitSlot slot in unitSlots){
            slot.RemoveUnits();
        }
    }

    #endregion

    #region Getters

    public WorldObject GetConnectedObject() { return connectedObject; }

    public List<UnitSlot> GetUnits() { return unitSlots; }

    public UnitSlot GetUnit(byte id) { return unitSlots[id]; }

    public short GetSameUnitSlotIndex(short id)
    {
        short emptySlotIndex = 7;
        for (short i = 0; i < unitSlots.Count; i++)
        {
            if (unitSlots[i].GetId() == id) return i;
            else if (unitSlots[i].IsEmpty()) emptySlotIndex = i;
        }
        return emptySlotIndex;
    }

    public List<short> GetEmptySlots()
    {
        List<short> list = new List<short>();
        for (short i = 0; i < 7; i++)
        {
            if (unitSlots[i].IsEmpty()) list.Add(i);
        }
        return list;
    }

    public float GetMovementPoints()
    {
        float movement = 10000f;
        for (short i = 0; i < 7; i++)
        {
            if (!unitSlots[i].IsEmpty())
            {
                if (unitSlots[i].GetMovementPoints() < movement)
                {
                    movement = unitSlots[i].GetMovementPoints();
                }
            }
        }
        return movement;
    }

    public bool IsArmyEmpty()
    {
        foreach (UnitSlot unit in unitSlots)
        {
            if (!unit.IsEmpty()) return false;
            else continue;
        }
        return true;
    }

    public bool AreUnitSlotsSameType(byte a, byte b)
    {
        if (unitSlots[a].GetId() == unitSlots[b].GetId()) return true;
        else return false;
    }

    public bool AreUnitSlotsSameType(byte a, UnitSlot otherArmyUnit)
    {
        if (unitSlots[a].GetId() == otherArmyUnit.GetId()) return true;
        else return false;
    }

    #endregion
}
