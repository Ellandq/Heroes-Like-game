using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================================
// The main camera script
//==============================================

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    // Store a referance to all sub input scripts

    [SerializeField]
    internal GridInteractionManager gridInteractionManager;

    [SerializeField]
    internal MouseInput mouseInput;

    [SerializeField]
    internal KeyboardInput keyboardInput;

    void Awake ()
    {
        Instance = this;
    }

}
