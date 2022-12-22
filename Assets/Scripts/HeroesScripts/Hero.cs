using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroObject", menuName = "ScriptableObjects/HeroObject")]
public class Hero : ScriptableObject
{
    [Header ("Basic Information:")]
    [SerializeField] public string heroName;
    [SerializeField] public HeroType heroType;
    [SerializeField] public HeroTag heroTag;

    [Header ("Hero level information:")]
    [SerializeField] public short heroLevel;
    [SerializeField] public long heroExperiance;

    [Header ("Hero Statistics:")]
    [SerializeField] public short maxHealth;
    [SerializeField] public short health;
    [SerializeField] public short minDamage;
    [SerializeField] public short maxDamage;
    [SerializeField] public short attack;
    [SerializeField] public short defense;
    [SerializeField] public short rangeAttack;
    [SerializeField] public short rangeDefense;
    [SerializeField] public short mana;
    [SerializeField] public short ammunition;
    [SerializeField] public short speed;
    [SerializeField] public short movement;
    [SerializeField] public short experiance;
    [SerializeField] public short mapMovement;

    [Header ("Hero Abilities:")]
    [SerializeField] public List<HeroSkill> skills;
    [SerializeField] public HeroSpecialAbility specialAbility;

    [Header ("Hero SpellBook:")]
    [SerializeField] public List<Spell> spellBook;

    [Header ("Inventory")]
    [SerializeField] public List<ArtifactTag> heroInventory;
    [SerializeField] public List<ArtifactTag> backpack;

    [Header ("Adjusted hero Statistics:")]
    [System.NonSerialized] public short adjusted_maxHealth;
    [System.NonSerialized] public short adjusted_minDamage;
    [System.NonSerialized] public short adjusted_maxDamage;
    [System.NonSerialized] public short adjusted_attack;
    [System.NonSerialized] public short adjusted_defense;
    [System.NonSerialized] public short adjusted_rangeAttack;
    [System.NonSerialized] public short adjusted_rangeDefense;
    [System.NonSerialized] public short adjusted_mana;
    [System.NonSerialized] public short adjusted_ammunition;
    [System.NonSerialized] public short adjusted_speed;
    [System.NonSerialized] public short adjusted_movement;
    [System.NonSerialized] public short adjusted_experiance;
    [System.NonSerialized] public short adjusted_mapMovement;

    public void ResetHero ()
    {
        // Resets level
        heroLevel = 1;
        heroExperiance = 0;

        // Resets Health
        maxHealth = 100;
        health = maxHealth;

        // Resets damage
        minDamage = 16;
        maxDamage = Convert.ToInt16(Math.Ceiling(minDamage * 1.5));

        // Resets attack and defense
        attack = 10;
        defense = 10;
        rangeAttack = 0;
        rangeDefense = 10;

        // Resets mana and ammo
        mana = 10;
        ammunition = 0;
        speed = 6;
        movement = 22;

        // Resets the hero spellbook
        spellBook.Clear();

        // Resets the hero inventory
        heroInventory.Clear();
        backpack.Clear();

    }
}
