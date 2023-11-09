using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitHandler
{
    public UnitsInformation GetUnitsInformation();

    public Sprite GetIcon ();
}
