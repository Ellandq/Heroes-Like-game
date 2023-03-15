using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] InputManager inputManager;

    internal bool isUpPressed;
    internal bool isDownPressed;
    internal bool isLeftPressed;
    internal bool isRightPressed;
    internal bool resetCameraPressed;
    internal bool isLeftShiftPressed;
    internal bool isEscapePressed;
    internal bool isTabPressed;
    internal bool isSpacePressed;

    public UnityEvent onEscPressed;

    private void Update ()
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

        if(Input.GetKey(KeyCode.LeftShift)){
            isLeftShiftPressed = true;
        }else{
            isLeftShiftPressed = false;
        }

        if(Input.GetKey(KeyCode.Tab)){
            isTabPressed = true;
        }else{
            isTabPressed = false;
        }

        if(Input.GetKey(KeyCode.Space)){
            isSpacePressed = true;
        }else{
            isSpacePressed = false;
        }

        if(Input.GetKey(KeyCode.Escape)){
            if (!isEscapePressed) onEscPressed?.Invoke();
            isEscapePressed = true;
        }else{
            isEscapePressed = false;
        }
    }
}