using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    [Header ("Grid Information")]
    [SerializeField] private BattleGridCell[,] battleGrid;
    [SerializeField] private List<Vector2Int> cellsAvailableForSelection;
    private BattleGridCell selectedCell;

    [Header ("Grid Settings")]
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private GameObject gridCellPrefab;
    private const int GRID_SIZE_X = 12;
    private const int GRID_SIZE_Y = 10;
    private float gridOffset;

    private void Start (){
        selectedCell = null;
        cellsAvailableForSelection = new List<Vector2Int>();
        gridOffset = gridCellPrefab.transform.localScale.x;
        GenerateGrid();
    }

    private void Update (){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, gridLayer))
        {
            SelectCell(hit.collider.GetComponent<BattleGridCell>());
        }else{
            SelectCell(null);
        }
}

    public void GenerateGrid (){
        battleGrid = new BattleGridCell[GRID_SIZE_X, GRID_SIZE_Y];
        for (int x = 0; x < GRID_SIZE_X; x++){
            for (int y = 0; y < GRID_SIZE_Y; y++){
                GameObject gridCell = Instantiate(gridCellPrefab, GetGridCellPosition(x, y), Quaternion.identity, transform);
                gridCell.name = "GridCell ( X : " + x + ", Y : " + y + " )";
                battleGrid[x,y] = gridCell.GetComponent<BattleGridCell>();
                battleGrid[x,y].ChangeCellStatus(BattleGridCellStatus.unavailable);
                battleGrid[x,y].SetCellPosition(x, y);
            }
        }
    }

    public void SelectCell (BattleGridCell cell = null){
        if (cell == selectedCell) return;
        if (selectedCell != null){
            if (IsCellAvailableForSelection(selectedCell.GetPosition())){
                selectedCell.ChangeCellStatus(BattleGridCellStatus.available);
            }else{
                selectedCell.ChangeCellStatus(BattleGridCellStatus.unavailable);
            }
        }
        if (cell != null && cell.GetStatus() != BattleGridCellStatus.unavailable){
            cell.ChangeCellStatus(BattleGridCellStatus.selected);
            selectedCell = cell;
        }else{
            selectedCell = null;
        }
        
    }

    public BattleGridCell GetCell (Vector2Int position){
        return battleGrid[position.x, position.y];
    }

    private Vector3 GetGridCellPosition(int x, int y){
        return new Vector3(x * gridOffset, .5f, y * gridOffset);
    }

    private bool IsCellAvailableForSelection (Vector2Int position){
        foreach (Vector2Int pos in cellsAvailableForSelection){
            if (pos == position) { return true; }
        }
        return false;
    }
}

public enum BattleGridCellStatus{
    available, unavailable, selected
}
