using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class UnitSlot : MonoBehaviour
{
    public bool slotEmpty = true;
    public string unitName = "Empty";
    public int howManyUnits;
    string unitInformationFilePath, unitFileName;
    string [] unitInformationArray;
    string [] splitUnitInfo;
    string [] unitSplitName;
    string currentReadUnit;

    public void ChangeSlotStatus(int _unitId, int _howManyUnits)
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
}