using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MouseOneInteraction : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    private float startTime, endTime;

    void Start ()
    {

        //startTime = 0f;
        //endTime = 0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            if (MouseHeld())
            {
                //Debug.Log("Mouse One was held");
            }
            else
            {
                //Debug.Log("Mouse One was clicked");
            }
        }
        
    }
    public bool MouseHeld()
    {
        return true;
    }
}
