using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================================
// The main camera script
//==============================================

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    // Store a referance to all sub camera scripts

    [SerializeField] internal CameraMovement cameraMovement;
    [SerializeField] internal CameraZoom cameraZoom;

    internal bool cameraEnabled;
    internal bool cameraMovementEnabled;

    // Creating a static instance of this class
    private void Start ()
    {
        Instance = this;
        cameraEnabled = false;
        cameraMovementEnabled = false;
    }

    // Enable camera Movment
    public void EnableCamera ()
    {
        cameraEnabled = true;
    }

    public void DisableCamera ()
    {
        cameraEnabled = false;
    }
}
