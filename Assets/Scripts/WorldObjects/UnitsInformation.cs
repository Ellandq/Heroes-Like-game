using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsInformation : MonoBehaviour
{
    [Header("Unit slots references")]
    [SerializeField] public List <GameObject> unitSlots;

    public List<GameObject> GetArmyUnits ()
    {
        return unitSlots;
    }

    public void SwapUnitsPosition (short a, short b)
    {
        int id01 = unitSlots[a].GetComponent<UnitSlot>().unitID;
        int id02 = unitSlots[b].GetComponent<UnitSlot>().unitID;
        int unitCount01 = unitSlots[a].GetComponent<UnitSlot>().howManyUnits;
        int unitCount02 = unitSlots[b].GetComponent<UnitSlot>().howManyUnits;
        float mPoints01 = unitSlots[a].GetComponent<UnitSlot>().movementPoints;
        float mPoints02 = unitSlots[b].GetComponent<UnitSlot>().movementPoints;
        unitSlots[a].GetComponent<UnitSlot>().RemoveUnits();
        unitSlots[b].GetComponent<UnitSlot>().RemoveUnits();

        unitSlots[a].GetComponent<UnitSlot>().ChangeSlotStatus(id02, unitCount02, mPoints02);
        unitSlots[b].GetComponent<UnitSlot>().ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public void SwapUnitsPosition (short a, GameObject otherArmyUnit)
    {
        int id01 = unitSlots[a].GetComponent<UnitSlot>().unitID;
        int id02 = otherArmyUnit.GetComponent<UnitSlot>().unitID;
        int unitCount01 = unitSlots[a].GetComponent<UnitSlot>().howManyUnits;
        int unitCount02 = otherArmyUnit.GetComponent<UnitSlot>().howManyUnits;
        float mPoints01 = unitSlots[a].GetComponent<UnitSlot>().movementPoints;
        float mPoints02 = otherArmyUnit.GetComponent<UnitSlot>().movementPoints;
        unitSlots[a].GetComponent<UnitSlot>().RemoveUnits();
        otherArmyUnit.GetComponent<UnitSlot>().RemoveUnits();

        unitSlots[a].GetComponent<UnitSlot>().ChangeSlotStatus(id02, unitCount02, mPoints02);
        otherArmyUnit.GetComponent<UnitSlot>().ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public void AddUnits (short a, short b)
    {
        unitSlots[b].GetComponent<UnitSlot>().howManyUnits += unitSlots[a].GetComponent<UnitSlot>().howManyUnits;
        unitSlots[a].GetComponent<UnitSlot>().RemoveUnits();
    }

    public void AddUnits (short a, GameObject otherArmyUnit)
    {
        otherArmyUnit.GetComponent<UnitSlot>().howManyUnits += unitSlots[a].GetComponent<UnitSlot>().howManyUnits;
        unitSlots[a].GetComponent<UnitSlot>().RemoveUnits();
    }

    public void SplitUnits (short a, short b) // Spliting with itself
    {
        UnitSplitWindow.Instance.PrepareUnitsToSwap(unitSlots[a].GetComponent<UnitSlot>(), unitSlots[b].GetComponent<UnitSlot>(), this, a, b);
    }

    public void SplitUnits (short a, short b, UnitsInformation otherArmy, GameObject otherArmyUnit) // Spliting with another army
    {
        UnitSplitWindow.Instance.PrepareUnitsToSwap(unitSlots[a].GetComponent<UnitSlot>(), otherArmy.unitSlots[b].GetComponent<UnitSlot>(), this, otherArmy, a, b);
    }

    public bool AreUnitSlotsSameType (short a, short b)
    {
        if (unitSlots[a].GetComponent<UnitSlot>().unitID == unitSlots[b].GetComponent<UnitSlot>().unitID) return true;
        else return false;
    }

    public bool AreUnitSlotsSameType (short a, GameObject otherArmyUnit)
    {
        if (unitSlots[a].GetComponent<UnitSlot>().unitID == otherArmyUnit.GetComponent<UnitSlot>().unitID) return true;
        else return false;
    }
}
