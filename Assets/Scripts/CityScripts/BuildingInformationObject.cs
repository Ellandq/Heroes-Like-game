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
    public ResourceIncome cost;
    public List<BuildingID> additionalRequirements;

    [Header("Building Icon")]
    public Sprite buildingIcon;
}
