using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] GridCell gridCell;
    public GameObject occupyingObject;

    private int posX;
    private int posZ;
    public int gridPosX;
    public int gridPosZ;

    [Header("Movement information")]
    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable;
    public bool isOccupyingObjectInteratable = false;

    public PathNode cameFromCell;


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    void Start ()
    {
        gridPosX = gridCell.posX;
        gridPosZ = gridCell.posZ;
        isWalkable = !gridCell.isOccupied;
    }

    void FixedUpdate ()
    {
        isWalkable = !gridCell.isOccupied;
        if (!isWalkable){
            isOccupyingObjectInteratable = gridCell.isObjectInteractable;
        }
    }
}
