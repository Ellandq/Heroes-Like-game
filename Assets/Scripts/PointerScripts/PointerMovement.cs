using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMovement : MonoBehaviour
{ 
    [SerializeField] LayerMask pointerLayersToHit;
    [SerializeField] GameObject inputManager;
    MouseInput mouseInput;

    void Awake()
    {
        mouseInput = inputManager.GetComponent<MouseInput>();
    }
    void Update()
    {
        transform.position = mouseInput.MouseWorldPosition(pointerLayersToHit);
    }
}
