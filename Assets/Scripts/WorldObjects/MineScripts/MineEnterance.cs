using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEnterance : ObjectEnterance
{
    [SerializeField] private Mine mine;



    private void Start (){
        SetEnteranceCells();
        GameGrid.Instance.GetCell(GameGrid.Instance.GetGridPosFromWorld(transform.position)).AddOccupyingObject(gameObject);

    }
    
    public void SetEnteranceCells()
    {
        List <PathNode> enteranceList = new List<PathNode>();
        Vector2Int gridPosition = GameGrid.Instance.GetGridPosFromWorld(transform.TransformPoint(Vector3.zero));
        float mineRotation = mine.GetRotation();   

        if (mineRotation == 0){
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (mineRotation == 90){
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (mineRotation == 180){
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (mineRotation == 270){
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetNode(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else {
            Debug.Log("Enterance list of object: " + this.transform.parent.gameObject.name + " is empty.");
            return;
        }
    }
}
