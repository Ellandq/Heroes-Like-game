using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectDisplay
{
    public void ManualRotation(float delta);

    public void StopManualMovement();

    public void RotateDisplay (int movementValue);

    public void UpdateDisplay (bool resetPosition);

    public void ResetDisplay ();

    public void UpdateNewSlot (int moveAmount);
}
