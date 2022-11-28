using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingInformationObject", menuName = "ScriptableObjects/BuildingInformationObject")]
public class BuildingInformationObject : ScriptableObject
{
    [Header("Basic Building Information")]
    public BuildingID buildingID;
    public string buildingName;

    [Header("Building requirements")]
    public int goldCost;
    public int woodCost;
    public int oreCost;
    public int gemCost;
    public int mercuryCost;
    public int sulfurCost;
    public int crystalCost;
    public List<BuildingID> additionalRequirements;

    [Header("Building Icon")]
    public Sprite buildingIcon;
}
