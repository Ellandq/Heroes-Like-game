using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;
    private Coroutine coroutine;
    private float setUpProgress;
    public Pathfinding pathfinding;
    private int gridLength;
    private int gridWidth;
    public const float GRID_SPACE_SIZE = 5f;

    [SerializeField] private GameObject gridCellPrefab;
    public GridCell[,] gameGrid;
    public PathNode [,] pathNodeArray;

    void Start ()
    {
        Instance = this;
    }

    public void GameGridSetUp (Vector2Int gridSize){
        setUpProgress = 0f;
        coroutine = StartCoroutine(CreateGrid(gridSize));
    }
    
    // Gets the grid position from world position
    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / GRID_SPACE_SIZE);
        int z = Mathf.FloorToInt(worldPosition.z / GRID_SPACE_SIZE);

        x = Mathf.Clamp(x, 0, gridWidth);
        z = Mathf.Clamp(z, 0, gridLength);

        return new Vector2Int(x, z);
    }
    
    // Gets the world position of a grid position
    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * GRID_SPACE_SIZE;
        float z = gridPos.y * GRID_SPACE_SIZE;

        return new Vector3(x, 0, z);
    }
    
    // Returns a selected GridCell
    public GridCell GetCell (Vector2Int _gridPos)
    {
        return gameGrid[_gridPos.x, _gridPos.y].gameObject.GetComponent<GridCell>();    //Grid Space ( X: 0 , Z: 0)
    }
    
    // Returns a selected PathNode
    public PathNode GetNode (Vector2Int _gridPos)
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
            if (!GetCell(new Vector2Int(gridPosition.x - 1, gridPosition.y)).isOccupied){
                neighbourList.Add(GetCell(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }
            // Left Down
            if (gridPosition.y - 1 >= 0) {
                if (!GetCell(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)).isOccupied){
                    neighbourList.Add(GetCell(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
                }
            }
            // Left Up
            if (gridPosition.y + 1 < GetGridLength()) {
                if (!GetCell(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetCell(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x + 1 < GetGridWidth()){
            // Right
            if (gridPosition.y - 1 >= 0) {
                if (!GetCell(new Vector2Int(gridPosition.x + 1, gridPosition.y)).isOccupied){ 
                    neighbourList.Add(GetCell(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
                }
            }
            // Right Down
            if (gridPosition.y - 1 >= 0){ 
                if (!GetCell(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)).isOccupied){ 
                neighbourList.Add(GetCell(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
                }
            }
            // Right Up
                if (gridPosition.y + 1 < GetGridLength()) {
                    if (!GetCell(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetCell(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
                }
            }
        }
        if (gridPosition.x - 1 >= 0){
            // Down
            if (gridPosition.y - 1 >= 0){
                if (!GetCell(new Vector2Int(gridPosition.x, gridPosition.y - 1)).isOccupied){
                    neighbourList.Add(GetCell(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
                }
            }
            
            // Up
            if (gridPosition.y + 1 < GetGridLength()) {
                if (!GetCell(new Vector2Int(gridPosition.x, gridPosition.y + 1)).isOccupied){
                    neighbourList.Add(GetCell(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
                }
            }
        }
        return neighbourList;
    }

    public void PlaceBuildingOnGrid(Vector2Int position, BuildingType type, float rotation, GameObject building) {
        int width, height;
        Vector2Int startingOffset = new Vector2Int(0, 0);
        switch (type) {
            case BuildingType.OneByOne:
                width = height = 1;
                startingOffset.x = 0;
                startingOffset.y = 0;
                break;
            case BuildingType.TwoByOne:
                width = 2;
                height = 1;
                startingOffset.x = 0;
                startingOffset.y = 1;
                break;
            case BuildingType.TwoByTwo:
                width = height = 2;
                startingOffset.x = 1;
                startingOffset.y = 1;
                break;
            case BuildingType.ThreeByOne:
                width = 3;
                height = 1;
                startingOffset.x = -1;
                startingOffset.y = 0;
                break;
            case BuildingType.ThreeByTwo:
                width = 3;
                height = 2;
                startingOffset.x = -1;
                startingOffset.y = 1;
                break;
            case BuildingType.ThreeByThree:
                width = height = 3;
                startingOffset.x = -1;
                startingOffset.y = 1;
                break;
            case BuildingType.FiveByFive:
                width = height = 5;
                startingOffset.x = -2;
                startingOffset.y = 2;
                break;
            default:
                Debug.LogError("Invalid building type: " + type);
                return;
        }  
        
        if (type == BuildingType.TwoByTwo){
            Vector2Int originPos = Vector2Int.zero;
            switch ((int)rotation) {
                case 90:
                    originPos += new Vector2Int(position.x, position.y);
                    break;
                case 180:
                    originPos += new Vector2Int(position.x - 1, position.y);
                    break;
                case 270:
                    originPos += new Vector2Int(position.x - 1, position.y + 1);
                    break;
                default:
                    originPos += new Vector2Int(position.x, position.y + 1);
                    break;
            } 

            // iterate over the grid cells within the building's rotated footprint
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    gameGrid[originPos.x + x, originPos.y - y].AddOccupyingObject(building);
                }
            }
        }else{
            // calculate rotated width and height based on rotation
            if (rotation == 90f || rotation == 270f) {
                int temp = width;
                width = height;
                height = temp;

                temp = startingOffset.x;
                startingOffset.x = -startingOffset.y;
                startingOffset.y = -temp;
            } 

            // iterate over the grid cells within the building's rotated footprint
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    gameGrid[position.x + startingOffset.x + x, position.y + startingOffset.y - y].GetComponent<GridCell>().AddOccupyingObject(building);
                }
            }
        }       
    }

    private IEnumerator CreateGrid(Vector2Int gridSize)
    {
        gridLength = gridSize.x;
        gridWidth = gridSize.y;
        gameGrid = new GridCell[gridLength, gridWidth];
        pathNodeArray = new PathNode[gridLength, gridWidth];

        if (gridCellPrefab == null)
        {
            Debug.LogError("Error: Grid Cell Prefab on the Game grid is not assigned");
            yield break; // Exit the coroutine if gridCellPrefab is not assigned.
        }

        for (int z = 0; z < gridLength; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // Calculate progress as a percentage
                setUpProgress = (float)(x + z * gridWidth) / (gridLength * gridWidth);
                
                // Check elapsed time since the last frame
                if (Time.deltaTime < 1f / 144f)
                {
                    // Yield a frame if the frame time is less than 1/144 of a second
                    yield return null;
                }

                // Create a new GridSpace object for each cell
                GameObject obj = Instantiate(gridCellPrefab, new Vector3(x * GRID_SPACE_SIZE, 0, z * GRID_SPACE_SIZE), Quaternion.identity);
                obj.transform.parent = transform;
                obj.name = "Grid Space (X: " + x + ", Z: " + z + ")";
                gameGrid[x, z] = obj.GetComponent<GridCell>();
                gameGrid[x, z].SetPosition(x, z);
                pathNodeArray[x, z] = obj.GetComponent<PathNode>();
            }
        }

        pathfinding = new Pathfinding();

        setUpProgress = 1f;
    }

    public float GetSetUpProgress (){ return setUpProgress; }
}
