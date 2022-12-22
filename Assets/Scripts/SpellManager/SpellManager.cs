using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    // Sets the static SpellManager Instance
    private void Awake ()
    {
        Instance = this;
    }
}

public enum Spell{
    
}
