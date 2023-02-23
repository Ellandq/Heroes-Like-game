using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsInformation : MonoBehaviour
{
    [Header ("Strongest Unit Icon")]
    public Sprite armyIcon;

    [Header("Unit slots references")]
    [SerializeField] public List <GameObject> unitSlots;

    public List<GameObject> GetArmyUnits ()
    {
        return unitSlots;
    }

    public float CheckMovementPoints ()
    {
        float movement = 10000f;
        for (int i = 0; i < 7; i++){
            if (!unitSlots[i].GetComponent<UnitSlot>().slotEmpty){
                if (unitSlots[i].GetComponent<UnitSlot>().GetMovementPoints() < movement){
                    movement = unitSlots[i].GetComponent<UnitSlot>().GetMovementPoints();
                }
            }
        }
        return movement;
    }

    public void RestoreMovementPoints()
    {
        for (int i = 0; i < 7; i++){
            if (!unitSlots[i].GetComponent<UnitSlot>().slotEmpty){
                unitSlots[i].GetComponent<UnitSlot>().RestoreMovementPoints();
            }
        }
    }

    public void RemoveMovementPoints(int _movementCost)
    {
        for (int i = 0; i < 7; i++){
            unitSlots[i].GetComponent<UnitSlot>().RemoveMovementPoints(_movementCost);
        }
    }

    // Check if army is empty
    public bool IsArmyEmpty ()
    {
        foreach (GameObject unit in unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    public void SetUnitStatus (int[] unitInfo)
    {
        unitSlots[0].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[0], unitInfo[1]);
        unitSlots[1].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[2], unitInfo[3]);
        unitSlots[2].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[4], unitInfo[5]);
        unitSlots[3].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[6], unitInfo[7]);
        unitSlots[4].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[8], unitInfo[9]);
        unitSlots[5].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[10], unitInfo[11]);
        unitSlots[6].GetComponent<UnitSlot>().SetSlotStatus(unitInfo[12], unitInfo[13]);
        UpdateArmyInformation();
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
        UpdateArmyInformation();
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
        UpdateArmyInformation();
    }

    public void AddUnits (short a, short b)
    {
        unitSlots[b].GetComponent<UnitSlot>().howManyUnits += unitSlots[a].GetComponent<UnitSlot>().howManyUnits;
        unitSlots[a].GetComponent<UnitSlot>().RemoveUnits();
        UpdateArmyInformation();
    }

    public void AddUnits (short a, GameObject otherArmyUnit)
    {
        otherArmyUnit.GetComponent<UnitSlot>().howManyUnits += unitSlots[a].GetComponent<UnitSlot>().howManyUnits;
        unitSlots[a].GetComponent<UnitSlot>().RemoveUnits();
        UpdateArmyInformation();
    }

    public void SplitUnits (short a, short b) // Spliting with itself
    {
        UnitSplitWindow.Instance.PrepareUnitsToSwap(unitSlots[a].GetComponent<UnitSlot>(), unitSlots[b].GetComponent<UnitSlot>(), this, a, b);
        UpdateArmyInformation();
    }

    public void SplitUnits (short a, short b, UnitsInformation otherArmy, GameObject otherArmyUnit) // Spliting with another army
    {
        UnitSplitWindow.Instance.PrepareUnitsToSwap(unitSlots[a].GetComponent<UnitSlot>(), otherArmy.unitSlots[b].GetComponent<UnitSlot>(), this, otherArmy, a, b);
        UpdateArmyInformation();
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

    private void AdjustUnitPosition() {
        for (short i = 0; i < unitSlots.Count; i++) {
            if (unitSlots[i] == null) continue;
            if (unitSlots[i].GetComponent<UnitSlot>().slotEmpty) continue;
            for (short j = 0; j < unitSlots.Count; j++) {
                if (i == j) continue;
                if (unitSlots[j] == null) {
                    if (j < i) {
                        SwapUnitsPosition(i, j);
                    }
                    continue;
                }
                if (unitSlots[j].GetComponent<UnitSlot>().slotEmpty) continue;
                if (unitSlots[i].GetComponent<UnitSlot>().isSlotHero) {
                    if (i < j) {
                        if (unitSlots[j].GetComponent<UnitSlot>().isSlotHero) {
                            if (unitSlots[j].GetComponent<UnitSlot>().heroObject.heroLevel > unitSlots[i].GetComponent<UnitSlot>().heroObject.heroLevel) {
                                SwapUnitsPosition(i, j);
                            }
                        }
                    }
                } else {
                    if (i < j) {
                        if (unitSlots[j].GetComponent<UnitSlot>().isSlotHero) {
                            SwapUnitsPosition(i, j);
                        } else {
                            if (unitSlots[j].GetComponent<UnitSlot>().unitObject.unitTier > unitSlots[i].GetComponent<UnitSlot>().unitObject.unitTier) {
                                SwapUnitsPosition(i, j);
                            } else if (unitSlots[j].GetComponent<UnitSlot>().unitObject.unitTier == unitSlots[i].GetComponent<UnitSlot>().unitObject.unitTier) {
                                if (unitSlots[j].GetComponent<UnitSlot>().howManyUnits > unitSlots[i].GetComponent<UnitSlot>().howManyUnits) {
                                    SwapUnitsPosition(i, j);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateArmyInformation ()
    {
        AdjustUnitPosition();
        if (unitSlots[0].GetComponent<UnitSlot>().isSlotHero){
            armyIcon = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), unitSlots[0].GetComponent<UnitSlot>().unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
        }else{
            armyIcon = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unitSlots[0].GetComponent<UnitSlot>().unitID));
        }
    }
}
