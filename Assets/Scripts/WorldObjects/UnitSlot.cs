using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class UnitSlot : MonoBehaviour
{
    public bool slotEmpty = true;
    public string unitName = "Empty";
    public int unitID;
    public int howManyUnits;
    public float movementPoints;
    private string unitInformationFilePath, unitFileName;
    private string [] unitInformationArray;
    private string [] splitUnitInfo;
    private string [] unitSplitName;
    private string currentReadUnit;

    public void ChangeSlotStatus(int _unitId, int _howManyUnits, float _movementPoints)
    {
        unitFileName = "UnitInformation.txt";
        unitInformationFilePath = Application.dataPath + "/" + "GameInformation/" + unitFileName;
        
        if (_howManyUnits == 0)
        {
            return;
        }
        if (File.Exists(unitInformationFilePath))
        {
            unitInformationArray = File.ReadAllLines (unitInformationFilePath);
            currentReadUnit = unitInformationArray[_unitId]; 
            splitUnitInfo = currentReadUnit.Split(' ');
            unitSplitName = splitUnitInfo[0].Split(':');
            movementPoints = _movementPoints;

            try {
                unitName = unitSplitName[0] + " " + unitSplitName[1];
            }
            catch (IndexOutOfRangeException)
            {
                unitName = splitUnitInfo[0];
            }            
        }
        else{
            Debug.Log("File does not exist");
            return;
        }
        howManyUnits = _howManyUnits;
        slotEmpty = false;
    }

    public void SetSlotStatus(int _unitId, int _howManyUnits)
    {
        unitFileName = "UnitInformation.txt";
        unitInformationFilePath = Application.dataPath + "/" + "GameInformation/" + unitFileName;
        
        if (_howManyUnits == 0)
        {
            return;
        }
        if (File.Exists(unitInformationFilePath))
        {
            unitInformationArray = File.ReadAllLines (unitInformationFilePath);
            currentReadUnit = unitInformationArray[_unitId]; 
            splitUnitInfo = currentReadUnit.Split(' ');
            unitSplitName = splitUnitInfo[0].Split(':');
            movementPoints = 22;

            try {
                unitName = unitSplitName[0] + " " + unitSplitName[1];
            }
            catch (IndexOutOfRangeException)
            {
                unitName = splitUnitInfo[0];
            }  
            unitID = _unitId;          
        }
        else{
            Debug.Log("File does not exist");
            return;
        }
        howManyUnits = _howManyUnits;
        slotEmpty = false;
    }
}