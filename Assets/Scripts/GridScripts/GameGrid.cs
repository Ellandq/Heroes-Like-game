using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Pathfinding pathfinding;
    public int gridLength;
    public int gridWidth;
    public float gridSpaceSize = 5f;

    [SerializeField] private GameObject gridCellPrefab;
    public GameObject[,] gameGrid;
    public PathNode [,] pathNodeArray;

    // Creates the grid when the game starts
    public void CreateGrid(int _gridLength, int _gridWidth)
    {
        gridLength = _gridLength;
        gridWidth = _gridWidth;
        gameGrid = new GameObject[gridLength, gridWidth];
        pathNodeArray = new PathNode[gridLength, gridWidth];

        if (gridCellPrefab == null )
        {
            Debug.LogError("Error: Grid Cell Prefab on the Game grid is not assigned");
        }

        // Create the grid
        for (int z = 0; z < gridLength; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // Create a new GridSpace object for each cell
                gameGrid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.identity);
                gameGrid[x, z].GetComponent<GridCell>().SetPosition(x, z);
                gameGrid[x, z].transform.parent = transform;
                gameGrid[x, z].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Z: " + z.ToString() + ")";
                pathNodeArray[x, z] = (gameGrid[x, z].GetComponent<PathNode>());
            }
        }
        pathfinding = new Pathfinding(this);
    }

    // Gets the grid position from world position
    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int z = Mathf.FloorToInt(worldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, gridWidth);
        z = Mathf.Clamp(z, 0, gridLength);

        return new Vector2Int(x, z);
    }

    // Gets the world position of a grid position
    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float z = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, z);
    }

    public GridCell GetGridCellInformation (Vector2Int _gridPos)
    {
        return transform.Find("Grid Space ( X: " + _gridPos.x + " , Z: " + _gridPos.y + ")").gameObject.GetComponent<GridCell>();    //Grid Space ( X: 0 , Z: 0)
    }

    public PathNode GetPathNodeInformation (Vector2Int _gridPos)
    {
        return transform.Find("Grid Space ( X: " + _gridPos.x + " , Z: " + _gridPos.y + ")").gameObject.GetComponent<PathNode>();    //Grid Space ( X: 0 , Z: 0)
    }

    public int GetGridLength ()
    {
        return gridLength;
    }

    public int GetGridWidth ()
    {
        return gridWidth;
    }
}
