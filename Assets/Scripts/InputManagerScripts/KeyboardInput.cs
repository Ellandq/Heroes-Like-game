using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] InputManager inputManager;

    internal bool isUpPressed;
    internal bool isDownPressed;
    internal bool isLeftPressed;
    internal bool isRightPressed;
    internal bool resetCameraPressed;
    //internal bool isTabKeyPressed;
    //internal bool isSpaceKeyPressed

    void Update ()
    {
        if(Input.GetKey(KeyCode.A)){
            isLeftPressed = true;
        }
        else{
            isLeftPressed = false;
        }

        if(Input.GetKey(KeyCode.D)){
            isRightPressed = true;
        }
        else{
            isRightPressed = false;
        }

        if(Input.GetKey(KeyCode.W)){
            isUpPressed = true;
        }
        else{
            isUpPressed = false;
        }

        if(Input.GetKey(KeyCode.S)){
            isDownPressed = true;
        }
        else{
            isDownPressed = false;
        }

        if(Input.GetKey(KeyCode.R)){
            resetCameraPressed = true;
        }
        else{
            resetCameraPressed = false;
        }
    }
}