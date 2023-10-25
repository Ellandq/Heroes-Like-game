using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    [SerializeField] private GameObject gridCellPrefab;

    [SerializeField] private BattleGridCell[,] battleGrid;
    private BattleGridCell selectedCell;

    private const int GRID_SIZE_X = 12;
    private const int GRID_SIZE_Y = 10;

    private float gridOffset;

    private void Start (){
        gridOffset = gridCellPrefab.transform.localScale.x;
        GenerateGrid();
    }

    public void GenerateGrid (){
        battleGrid = new BattleGridCell[GRID_SIZE_X, GRID_SIZE_Y];
        for (int x = 0; x < GRID_SIZE_X; x++){
            for (int y = 0; y < GRID_SIZE_Y; y++){
                GameObject gridCell = Instantiate(gridCellPrefab, GetGridCellPosition(x, y), Quaternion.identity, transform);
                gridCell.name = "GridCell ( X : " + x + ", Y : " + y + " )";
                battleGrid[x,y] = gridCell.GetComponent<BattleGridCell>();
                battleGrid[x,y].ChangeCellStatus(BattleGridCellStatus.available);
                battleGrid[x,y].SetCellPosition(x, y);
            }
        }
    }

    private Vector3 GetGridCellPosition(int x, int y){
        return new Vector3(x * gridOffset, .5f, y * gridOffset);
    }
}

public enum BattleGridCellStatus{
    available, unavailable, selected
}
