using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================================
// The main camera script
//==============================================

public class CameraManager : MonoBehaviour
{
    // Store a referance to all sub camera scripts

    [SerializeField]
    internal CameraMovement cameraMovement;

    [SerializeField]
    internal CameraZoom cameraZoom;

    internal bool cameraEnabled = false;

    public void EnableCamera ()
    {
        cameraEnabled = true;
    }

    public void DisableCamera ()
    {
        cameraEnabled = false;
    }
}
