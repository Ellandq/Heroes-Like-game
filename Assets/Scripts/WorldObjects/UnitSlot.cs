using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class UnitSlot : MonoBehaviour
{
    [Header ("Unit information")]
    public bool slotEmpty = true;
    public UnitObject unitObject;
    public UnitName unitName;
    public string unitNameString = "Empty";
    public int unitID;
    public int howManyUnits;
    public float movementPoints;

    public void ChangeSlotStatus(int _unitId, int _howManyUnits, float _movementPoints)
    { 
        if (_howManyUnits == 0 | _unitId == 0)
        {
            slotEmpty = true;
            unitID = 0;
            howManyUnits = 0;
            movementPoints = 0f;
            unitNameString = "Empty";
            unitName = UnitName.Empty;
            unitObject = null;

            return;
        }else{
            slotEmpty = false;
            unitID = _unitId;
            howManyUnits = _howManyUnits;
            movementPoints = _movementPoints;
            unitObject = UnitManager.Instance.GetUnitObject(unitID);
        }
        
    }

    public void SetSlotStatus(int _unitId, int _howManyUnits)
    {       
        if (_howManyUnits == 0){
            return;
        }
        slotEmpty = false;
        unitID = _unitId; 
        howManyUnits = _howManyUnits;
        unitObject = UnitManager.Instance.GetUnitObject(unitID);
        movementPoints = unitObject.mapMovement * 100; 
    }

    public void RemoveUnits()
    {
        slotEmpty = true;
        unitName = UnitName.Empty;
        unitNameString = "Empty";
        unitObject = null;
        unitID = 0;
        howManyUnits = 0;
        movementPoints = 0f;
    }

    public void AddUnits (int _unitCount)
    {
        howManyUnits += _unitCount;
    }

    public void RestoreMovementPoints ()
    {
        if (unitObject != null){
            movementPoints = unitObject.mapMovement * 100;
        }
    }

    public void RemoveMovementPoints (int movmentPointsToRemove)
    {
        if (!slotEmpty){
            movementPoints -= movmentPointsToRemove;
        }
    }

    public float GetMovementPoints ()
    {
        return movementPoints;
    }
}