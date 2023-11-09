using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [Header("Values")]
    [SerializeField] internal Vector3 screenPosition;
    [SerializeField] internal Vector3 worldPosition;

    [Header ("States")]
    internal float mouseScrollStatus;
    internal bool mouseButtonPressed_0;
    internal bool mouseButtonPressed_1;
    internal bool mouseButtonPressed_2;

    [Header ("Events")]
    public Action OnLeftMouseButtonDown;
    public Action OnRightMouseButtonDown;
    public Action OnMiddleMouseButtonDown;

    private void Update ()
    {
        screenPosition = Input.mousePosition;  
        mouseScrollStatus = Input.mouseScrollDelta.y;
        GetMouseInput();
    }

    // Checks what mouse buttons are pressed
    private void GetMouseInput ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonPressed_0 = true;
            OnLeftMouseButtonDown?.Invoke();
        }else{
            mouseButtonPressed_0 = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            mouseButtonPressed_1 = true;
            OnRightMouseButtonDown?.Invoke();
        }else{
            mouseButtonPressed_1 = false;
        }
        if (Input.GetMouseButton(2))
        {
            mouseButtonPressed_2 = true;
            OnMiddleMouseButtonDown?.Invoke();
        }else{
            mouseButtonPressed_2 = false;
        }
    }

    public Vector3 GetMousePosition3D (){
        return screenPosition;
    }

    public Vector2 GetMousePosition2D (){
        return new Vector2(screenPosition.x, screenPosition.y);
    }

    // Returns the mouse world position
    public Vector3 MouseWorldPosition(LayerMask layersToHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        
        if(Physics.Raycast(ray, out RaycastHit hitData, 100, layersToHit))
        {
            return hitData.point;
        }
        else
        {
            return new Vector3(0, 10, 0);
        }
    }

    // Returns the object that the mouse is over
    public GameObject GetMouseOverWorldObject(LayerMask layersToHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        
        if(Physics.Raycast(ray, out RaycastHit hitData, 100, layersToHit)){
            return hitData.transform.gameObject;
        }
        return null;
    }

    // Returns true if the mouse is over UI
    public bool IsMouseOverUI ()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
