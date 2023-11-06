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
    public ResourceIncome unitCost = new ResourceIncome();
}
