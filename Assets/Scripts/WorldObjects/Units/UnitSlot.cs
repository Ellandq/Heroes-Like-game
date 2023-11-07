using System;
using UnityEngine;
using System.Linq;

public class UnitSlot
{
    [Header("Slot Information")]
    private bool isEmpty = true;
    private bool isHero;
    private short unitId;
    private float movementPoints;

    [Header("Unit Information")]
    private UnitObject unitObject;
    private UnitName unitName;
    private int unitCount;

    [Header("Hero Information")]
    public Hero heroObject;
    public HeroTag heroTag;

    private UnitsInformation unitsInformation;

    public void Initialize(UnitsInformation unitsInformation)
    {
        this.unitsInformation = unitsInformation;
    }

    public void ChangeSlotStatus(short unitId, int unitCount, float movementPoints)
    {
        if (unitId > Enum.GetValues(typeof(UnitName)).Cast<int>().Max())
        {
            isHero = true;
            isEmpty = false;
            this.unitId = unitId;
            this.unitCount = 1;
            this.movementPoints = movementPoints;

            unitName = UnitName.Empty;
            unitObject = null;

            heroTag = (HeroTag)(unitId - Enum.GetValues(typeof(UnitName)).Cast<int>().Max());
            heroObject = HeroesManager.Instance.GetHeroObject(heroTag);
        }
        else if (unitCount == 0 || unitId == 0)
        {
            isHero = false;
            isEmpty = true;
            this.unitId = 0;
            this.unitCount = 0;
            this.movementPoints = 0f;

            unitName = UnitName.Empty;
            unitObject = null;

            heroTag = HeroTag.Empty;
            heroObject = null;

            return;
        }
        else
        {
            isHero = false;
            isEmpty = false;
            this.unitId = unitId;
            this.unitCount = unitCount;
            this.movementPoints = movementPoints;

            unitName = (UnitName)unitId;
            unitObject = UnitManager.Instance.GetUnitObject(unitId);

            heroTag = HeroTag.Empty;
            heroObject = null;
        }
    }

    public void SetSlotStatus(short unitId, int unitCount)
    {
        if (unitCount == 0)
        {
            return;
        }
        if (unitId > Enum.GetValues(typeof(UnitName)).Cast<int>().Max())
        {
            isHero = true;
            isEmpty = false;
            this.unitId = unitId;
            this.unitCount = 1;

            unitName = UnitName.Empty;
            unitObject = null;

            heroTag = (HeroTag)(unitId - Enum.GetValues(typeof(UnitName)).Cast<int>().Max());
            heroObject = HeroesManager.Instance.GetHeroObject(heroTag);

            movementPoints = heroObject.mapMovement * 100;
        }
        else
        {
            isEmpty = false;
            this.unitId = unitId;
            this.unitCount = unitCount;

            unitName = (UnitName)unitId;
            unitObject = UnitManager.Instance.GetUnitObject(unitId);

            heroTag = HeroTag.Empty;
            heroObject = null;

            movementPoints = unitObject.mapMovement * 100;
        }
    }

    public void SetUnitCount(int count)
    {
        if (count == 0) RemoveUnits();
        else unitCount = count;
    }

    public void RemoveUnits()
    {
        isEmpty = true;
        isHero = false;

        unitName = UnitName.Empty;
        unitObject = null;

        heroTag = HeroTag.Empty;
        heroObject = null;

        unitId = 0;
        unitCount = 0;
        movementPoints = 0f;
    }

    public void AddUnits(int unitCount, float movementPoints = -1)
    {
        this.unitCount += unitCount;
        if (movementPoints > -1) this.movementPoints = Mathf.Min(this.movementPoints, movementPoints);
        
    }

    public void RestoreMovementPoints()
    {
        if (unitObject != null)
        {
            movementPoints = unitObject.mapMovement * 100;
        }
        else if (heroObject != null)
        {
            movementPoints = heroObject.mapMovement * 100;
        }
    }

    public void RemoveMovementPoints(int movementPointsToRemove)
    {
        if (!isEmpty)
        {
            movementPoints -= movementPointsToRemove;
        }
    }

    public short GetUnitTier() { return unitObject.unitTier; }

    public short GetId() { return unitId; }

    public int GetUnitCount() { return unitCount; }

    public float GetMovementPoints(){ return movementPoints; }

    public bool IsHero() { return isHero; }

    public bool IsEmpty() { return isEmpty; }

    public UnitName GetUnitName() { return unitName; }

    public UnitsInformation GetUnitsInformation() { return unitsInformation; }
}
