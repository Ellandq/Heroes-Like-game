using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    [SerializeField] private List<UnitObject> unitObjects;

    void Awake ()
    {
        Instance = this;
    }

    public UnitObject GetUnitObject (int id){
        return unitObjects[id];
    }

    public UnitObject GetUnitObject (UnitName _unitName){
        return unitObjects[(int)_unitName];
    }
}
