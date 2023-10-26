using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectInteraction
{
    public void Interact<T>(T other);
}
