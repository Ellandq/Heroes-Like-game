using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    public static HeroesManager Instance;


    // Sets the HeroesManager class instance
    private void Awake ()
    {
        Instance = this;
    }

}
public enum HeroSkill{

}

public enum HeroSpecialAbility{

}

public enum HeroType{
    // Bazaar hero types
    Bazaar_Male_Warrior, Bazaar_Female_Warrior, Bazaar_Male_Mage, Bazaar_Female_Mage,
    // Coalition hero type
    Coalition_Male_Warrior, Coalition_Female_Warrior, Coalition_Male_Mage, Coalition_Female_Mage,
    // DarkOnes hero type
    DarkOnes_Male_Warrior, DarkOnes_Female_Warrior, DarkOnes_Male_Mage, DarkOnes_Female_Mage,
    // Hive hero type
    Hive_Male_Warrior, Hive_Female_Warrior, Hive_Male_Mage, Hive_Female_Mage,
    // Magic hero type
    Magic_Male_Warrior, Magic_Female_Warrior, Magic_Male_Mage, Magic_Female_Mage,
    // Temple hero type
    Temple_Male_Warrior, Temple_Female_Warrior, Temple_Male_Mage, Temple_Female_Mage
}

public enum HeroTag{
    Bazaar_Placeholder_Hero, 
    Coalition_Placeholder_Hero,
    DarkOnes_Placeholder_Hero,
    Hive_Placeholder_Hero,
    Magic_Placeholder_Hero,
    Temple_Placeholder_Hero
}