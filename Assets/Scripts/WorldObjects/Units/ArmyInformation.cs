using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyInformation : UnitsInformation
{
    [SerializeField] private Sprite armyIcon;

    public ArmyInformation(short[] unitInfo) : base(unitInfo){}

    #region Movement points manipulation

    public void RemoveMovementPoints(short movementCost) {
        for (int i = 0; i < 7; i++){
            unitSlots[i].RemoveMovementPoints(movementCost);
        }
    }

    #endregion

    #region Unit Manipulation

    public override void SwapUnitsPosition(short a, short b)
    {
        base.SwapUnitsPosition(a, b);
        UpdateArmyInformation();
    }

    public override void SwapUnitsPosition (short a, UnitSlot otherArmyUnit)
    {
        base.SwapUnitsPosition(a, otherArmyUnit);
        UpdateArmyInformation(otherArmyUnit.GetUnitsInformation() as ArmyInformation);
    }

    public override void AddUnits (short a, short b)
    {
        base.AddUnits(a, b);
        UpdateArmyInformation();
    }

    public override void AddUnits (short a, UnitSlot otherArmyUnit)
    {
        base.AddUnits(a, otherArmyUnit);
        UpdateArmyInformation(otherArmyUnit.GetUnitsInformation() as ArmyInformation);
    }

    public override void SplitUnits (short a, short b)
    {
        base.SplitUnits(a, b);
        UpdateArmyInformation();
    }

    public override void SplitUnits (short a, short b, UnitsInformation other)
    {
        base.SplitUnits(a, b, other);
        UpdateArmyInformation(other as ArmyInformation);
    }

    #endregion

    #region Unit Sorting

    private void UpdateArmyInformation ()
    {
        AdjustUnitPosition();
        if (unitSlots[0].IsHero()){
            armyIcon = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), unitSlots[0].GetId() - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
        }else{
            armyIcon = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unitSlots[0].GetId()));
        }
    }

    private void UpdateArmyInformation (ArmyInformation otherArmy)
    {
        otherArmy.UpdateArmyInformation();
        AdjustUnitPosition();
        if (unitSlots[0].IsHero()){
            armyIcon = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), unitSlots[0].GetId() - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
        }else{
            armyIcon = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), unitSlots[0].GetId()));
        }
    }
    
    #endregion

    #region Setters

    public override void SetUnitStatus (short[] unitInfo){
        base.SetUnitStatus(unitInfo);
        UpdateArmyInformation();
    }

    #endregion
}
