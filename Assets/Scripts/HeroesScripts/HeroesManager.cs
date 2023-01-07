using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    public static HeroesManager Instance;

    [Header ("Hero Objects")]
    [SerializeField] private List<Hero> heroList;

    // Sets the HeroesManager class instance
    private void Awake ()
    {
        Instance = this;
    }

    public Hero GetHeroObject (HeroTag _heroTag)
    {
        return heroList[(int)_heroTag];
    }
}
public enum HeroSkill{
    
}

public enum HeroSpecialAbility{
    
}

public enum HeroType{
    // Bazaar hero types
    Bazaar_Male_Warrior, Bazaar_Female_Warrior, Bazaar_Male_Mage, Bazaar_Female_Mage, Bazaar_Male_Special, Bazaar_Female_Special,
    // Coalition hero type
    Coalition_Male_Warrior, Coalition_Female_Warrior, Coalition_Male_Mage, Coalition_Female_Mage, Coalition_Male_Special, Coalition_Female_Special,
    // DarkOnes hero type
    DarkOnes_Male_Warrior, DarkOnes_Female_Warrior, DarkOnes_Male_Mage, DarkOnes_Female_Mage, DarkOnes_Male_Special, DarkOnes_Female_Special,
    // Hive hero type
    Hive_Male_Warrior, Hive_Female_Warrior, Hive_Male_Mage, Hive_Female_Mage, Hive_Male_Special, Hive_Female_Special,
    // Magic hero type
    Magic_Male_Warrior, Magic_Female_Warrior, Magic_Male_Mage, Magic_Female_Mage, Magic_Male_Special, Magic_Female_Special,
    // Temple hero type
    Temple_Male_Warrior, Temple_Female_Warrior, Temple_Male_Mage, Temple_Female_Mage, Temple_Male_Special, Temple_Female_Special
}

public enum HeroTag{
    Empty,

    // Bazaar Heroes
    Bazaar_Placeholder_Hero_01, 
    Bazaar_Placeholder_Hero_02, 
    Bazaar_Placeholder_Hero_03, 
    Bazaar_Placeholder_Hero_04, 
    Bazaar_Placeholder_Hero_05, 
    Bazaar_Placeholder_Hero_06, 

    // Coalition Heroes
    Coalition_Placeholder_Hero_01,
    Coalition_Placeholder_Hero_02,
    Coalition_Placeholder_Hero_03,
    Coalition_Placeholder_Hero_04,
    Coalition_Placeholder_Hero_05,
    Coalition_Placeholder_Hero_06,

    // DarkOnes Heroes
    DarkOnes_Placeholder_Hero_01,
    DarkOnes_Placeholder_Hero_02,
    DarkOnes_Placeholder_Hero_03,
    DarkOnes_Placeholder_Hero_04,
    DarkOnes_Placeholder_Hero_05,
    DarkOnes_Placeholder_Hero_06,

    // Hive heroes
    Hive_Placeholder_Hero_01,
    Hive_Placeholder_Hero_02,
    Hive_Placeholder_Hero_03,
    Hive_Placeholder_Hero_04,
    Hive_Placeholder_Hero_05,
    Hive_Placeholder_Hero_06,

    // Magic heroes
    Magic_Placeholder_Hero_01,
    Magic_Placeholder_Hero_02,
    Magic_Placeholder_Hero_03,
    Magic_Placeholder_Hero_04,
    Magic_Placeholder_Hero_05,
    Magic_Placeholder_Hero_06,
    
    // Temple Heroes
    Temple_Placeholder_Hero_01,
    Temple_Placeholder_Hero_02,
    Temple_Placeholder_Hero_03,
    Temple_Placeholder_Hero_04,
    Temple_Placeholder_Hero_05,
    Temple_Placeholder_Hero_06
}