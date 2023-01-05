using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwellingManager : MonoBehaviour
{
    public static DwellingManager Instance;

    [SerializeField] private List<DwellingObject> dwellingObjects;

    private void Awake ()
    {
        Instance = this;
    }

    public DwellingObject GetDwellingObject (CityFraction fraction, int index){
        return dwellingObjects[index + 8 * ((int)fraction - 1)];
    }

    public DwellingObject GetDwellingObject (UnitName unitName){
        return dwellingObjects[(int)unitName];
    }
}
