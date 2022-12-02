using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DwellingObject", menuName = "ScriptableObjects/DwellingObject")]
public class DwellingObject : ScriptableObject
{
    [Header ("Basic Dwelling Information")]
    public UnitName unit;
    public float unitWeeklyGain;
    public short dwellingIndex;
    public Sprite unitIcon;

    [Header ("Single Unit Cost")]
    public short goldCost;
    public short woodCost;
    public short oreCost;
    public short gemCost;
    public short mercuryCost;
    public short sulfurCost;
    public short crystalCost;
}
