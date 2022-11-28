using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DwellingObject", menuName = "ScriptableObjects/DwellingObject")]
public class DwellingObject : ScriptableObject
{
    [Header ("Basic Dwelling Information")]
    public UnitName unit;
    public short unitWeeklyGain;
    public Sprite unitIcon;

    // [Header ("World Object Dwelling Information")]
}
