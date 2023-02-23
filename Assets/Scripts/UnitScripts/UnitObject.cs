using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitObject", menuName = "ScriptableObjects/UnitObject")]
public class UnitObject : ScriptableObject
{
    [Header ("Unit Information")]
    public UnitName unitName;
    public string unitNameString;
    public short unitTier;

    [Header ("Unit stats")]
    public short health;
    public short minDamage;
    public short maxDamage;
    public short attack;
    public short defense;
    public short rangeAttack;
    public short rangeDefense;
    public short mana;
    public short ammunition;
    public short speed;
    public short movement;
    public short experiance;
    public short mapMovement;

    public List <UnitSkills> unitSkillList;
}
