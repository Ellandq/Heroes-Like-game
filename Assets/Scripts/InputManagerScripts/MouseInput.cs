using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] internal Vector3 screenPosition;
    [SerializeField] internal Vector3 worldPosition;
    [SerializeField] LayerMask pointerLayersToHit;
    internal float mouseScrollStatus;
    LayerMask layersToHit;

    public bool mouseButtonPressed_0;
    public bool mouseButtonPressed_1;
    public bool mouseButtonPressed_2;

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
        }else{
            mouseButtonPressed_0 = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            mouseButtonPressed_1 = true;
        }else{
            mouseButtonPressed_1 = false;
        }
        if (Input.GetMouseButton(2))
        {
            mouseButtonPressed_2 = true;
        }else{
            mouseButtonPressed_2 = false;
        }
    }

    public Vector3 GetMouseScreenPosition ()
    {
        return screenPosition;
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
    public GameObject MouseOverWorldObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        
        if(Physics.Raycast(ray, out RaycastHit hitData, 100, pointerLayersToHit))
        {
            Collider gridCellCollider = hitData.collider;
            return gridCellCollider.gameObject;
        }
        return null;
    }

    // Returns true if the mouse is over UI
    public bool IsMouseOverUI ()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
