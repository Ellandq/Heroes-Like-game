using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;
    public Pathfinding pathfinding;
    public int gridLength;
    public int gridWidth;
    public float gridSpaceSize = 5f;

    [SerializeField] private GameObject gridCellPrefab;
    public GameObject[,] gameGrid;
    public PathNode [,] pathNodeArray;

    void Start ()
    {
        Instance = this;
    }

    // Creates the grid when the game starts
    public void CreateGrid(Vector2Int gridSize)
    {
        gridLength = gridSize.x;
        gridWidth = gridSize.y;
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
        pathfinding = new Pathfinding();
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
    
    // Returns a selected GridCell
    public GridCell GetGridCellInformation (Vector2Int _gridPos)
    {
        return gameGrid[_gridPos.x, _gridPos.y].gameObject.GetComponent<GridCell>();    //Grid Space ( X: 0 , Z: 0)
    }
    
    // Returns a selected PathNode
    public PathNode GetPathNodeInformation (Vector2Int _gridPos)
    {
        return gameGrid[_gridPos.x, _gridPos.y].gameObject.GetComponent<PathNode>();    //Grid Space ( X: 0 , Z: 0)
    }
    
    // Returns the length of the grid
    public int GetGridLength ()
    {
        return gridLength;
    }
    
    // Returns the width of the grid
    public int GetGridWidth ()
    {
        return gridWidth;
    }
    
    // Returns the nearest empty GridCell
    public List<GridCell> GetEmptyNeighbourCell (Vector2Int gridPosition)
    {
        List<GridCell> neighbourList = new List<GridCell>();

        if (gridPosition.x - 1 >= 0){
            // Left
            if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)).isOccupied){
                neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }
            // Left Down
            if (gridPosition.y - 1 >= 0) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
                }
            }
            // Left Up
            if (gridPosition.y + 1 < GetGridLength()) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x + 1 < GetGridWidth()){
            // Right
            if (gridPosition.y - 1 >= 0) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)).isOccupied){ 
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
                }
            }
            // Right Down
            if (gridPosition.y - 1 >= 0){ 
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)).isOccupied){ 
                neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
                }
            }
            // Right Up
                if (gridPosition.y + 1 < GetGridLength()) {
                    if (!GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x - 1 >= 0){
            // Down
            if (gridPosition.y - 1 >= 0){
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
                }
            }
            
            // Up
            if (gridPosition.y + 1 < GetGridLength()) {
                if (!GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetGridCellInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
                }
            }
        }
        return neighbourList;
    }


    // public List<GridCell> GetSurroundingCells (Vector2Int gridPosition, float rotation, Vector2Int size)
    // {
        
    // }

    public void PlaceBuildingOnGrid(Vector2Int position, BuildingType type, float rotation, GameObject building) {
        int width, height;
        switch (type) {
            case BuildingType.OneByOne:
                width = height = 1;
                break;
            case BuildingType.TwoByOne:
                width = 2;
                height = 1;
                break;
            case BuildingType.TwoByTwo:
                width = height = 2;
                break;
            case BuildingType.ThreeByOne:
                width = 3;
                height = 1;
                break;
            case BuildingType.ThreeByTwo:
                width = 3;
                height = 2;
                break;
            case BuildingType.ThreeByThree:
                width = height = 3;
                break;
            case BuildingType.FiveByFive:
                width = height = 5;
                break;
            default:
                Debug.LogError("Invalid building type: " + type);
                return;
        }

        // adjust position to be centered on the building's origin
        position -= new Vector2Int(width / 2, height / 2);

        // calculate rotated width and height based on rotation
        if (rotation == 90f || rotation == 270f) {
            int temp = width;
            width = height;
            height = temp;
        }

        // iterate over the grid cells within the building's rotated footprint
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // calculate the rotated position of the grid cell
                Vector2Int rotatedPos = position;
                switch ((int)rotation) {
                    case 90:
                        rotatedPos += new Vector2Int(width - y - 1, x);
                        break;
                    case 180:
                        rotatedPos += new Vector2Int(width - x - 1, height - y - 1);
                        break;
                    case 270:
                        rotatedPos += new Vector2Int(y, height - x - 1);
                        break;
                    default:
                        rotatedPos += new Vector2Int(x, y);
                        break;
                }

                // do something with the grid cell at the rotated position
                gameGrid[rotatedPos.x, rotatedPos.y].GetComponent<GridCell>().AddOccupyingObject(building);
            }
        }
    }


}
