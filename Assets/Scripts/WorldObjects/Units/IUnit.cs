using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    // Movement point manipulation

    public void RestoreMovementPoints ();

    // Unit manipulation

    public void SwapUnitsPosition (short a, short b);
    public void SwapUnitsPosition (short a, UnitSlot other);
    public void AddUnits (short a, short b);
    public void AddUnits (short a, UnitSlot other);
    public void SplitUnits (short a, short b);
    public void SplitUnits (short a, short b, UnitsInformation other);

    // UnitSorting

    // Setters

    public void SetUnitStatus (int[] unitInfo);

    // Getters

    public WorldObject GetConnectedObject ();

    public List<UnitSlot> GetUnits ();

    public float GetMovementPoints ();

    public bool IsArmyEmpty ();
    public bool AreUnitSlotsSameType (short a, short b);
    public bool AreUnitSlotsSameType (short a, UnitSlot other);
}
