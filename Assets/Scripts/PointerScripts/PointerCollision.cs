using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerCollision : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] GameGrid gameGrid;
    GridCell gridCell;
    
    MouseInput mouseInput;
    
    [Header("Colliding object information")]
    public Vector3 currentColidingObjectCoordinates;
    public GameObject currentCollidingObject;

    [Header("Grid Information")]
    public Vector2 currentGridPosition;
    public bool isCellOccupied = false;
    

    void Awake()
    {
        mouseInput = inputManager.GetComponent<MouseInput>();
    }
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }

    void Update()
    {
        currentCollidingObject = mouseInput.MouseOverWorldObject();
        if (currentCollidingObject != null)
        {
            currentColidingObjectCoordinates = currentCollidingObject.transform.position;
            currentGridPosition = gameGrid.GetGridPosFromWorld(currentColidingObjectCoordinates);
        }
    }
}