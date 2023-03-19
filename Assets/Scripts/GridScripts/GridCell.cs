using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] PathNode pathNode;
    internal int posX;
    internal int posZ;
    private string currentColidingObject;
    private Vector3 currentColidingObjectCoordinates;

    //Saves a referance to the agame object that gets placed on the cell
    public GameObject objectInThisGridSpace = null;
    
    // Saves if the grid space is occpied or not and what type of object it is
    public bool isOccupied = false;
    public bool isObjectInteractable = false;

    // Set the position of this grid cell on the grid
    public void SetPosition(int x, int z)
    {
        posX = x;
        posZ = z;
    }

    // Get the position of this grid space on the grid
    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posZ);
    }

    // Checks what object entered it's trigger
    void  OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "GridCell") return;
        if (objectInThisGridSpace != null) return;
        objectInThisGridSpace = other.gameObject;
        isOccupied = true;
        if (objectInThisGridSpace.tag == "CityEnterance" | objectInThisGridSpace.tag == "Army" 
            | objectInThisGridSpace.tag == "MineEnterance" | objectInThisGridSpace.tag == "Resource"){
            isObjectInteractable = true;
            pathNode.isWalkable = false;
            pathNode.isOccupyingObjectInteratable = true;
            pathNode.occupyingObject = objectInThisGridSpace;
        }
    }

    // Checks whether the leaving object is the same as the occupying one and changes GridCell status accordingly
    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.name == objectInThisGridSpace.name){
            RemoveOccupyingObject();
        }
    }

    // Changes the status of this GridCell to an empty one
    public void RemoveOccupyingObject()
    {
        objectInThisGridSpace = null;
        isOccupied = false;
        isObjectInteractable = false;
        pathNode.isWalkable = true;
        pathNode.isOccupyingObjectInteratable = false;
        pathNode.occupyingObject = null;
    }

    public void AddOccupyingObject (GameObject other){
        objectInThisGridSpace = other.gameObject;
        isOccupied = true;
        if (objectInThisGridSpace.tag == "CityEnterance" | objectInThisGridSpace.tag == "Army" 
            | objectInThisGridSpace.tag == "MineEnterance" | objectInThisGridSpace.tag == "Resource"){
            isObjectInteractable = true;
            pathNode.isWalkable = false;
            pathNode.isOccupyingObjectInteratable = true;
            pathNode.occupyingObject = objectInThisGridSpace;
        }
    }
}
