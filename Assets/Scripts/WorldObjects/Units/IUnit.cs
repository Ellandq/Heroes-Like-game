using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IUnit
{
    // Movement point manipulation

    public void RestoreMovementPoints ();

    // Unit manipulation

    public void SwapUnits (byte a, byte b);
    public void SwapUnits (byte a, UnitSlot other);
    public void AddUnits (byte a, byte b);
    public void AddUnits (byte a, UnitSlot other);
    public void SplitUnits (byte a, byte b, UnitsInformation other);

    // UnitSorting

    // Setters

    public void SetUnitStatus (short[] unitInfo);

    // Getters

    public WorldObject GetConnectedObject ();

    public List<UnitSlot> GetUnits ();

    public float GetMovementPoints ();

    public bool IsArmyEmpty ();
    public bool AreUnitSlotsSameType (byte a, byte b);
    public bool AreUnitSlotsSameType (byte a, UnitSlot other);
}
