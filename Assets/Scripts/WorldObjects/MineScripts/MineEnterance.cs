using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEnterance : MonoBehaviour, IEnteranceInteraction
{
    [SerializeField] private Mine mine;
    private List <PathNode> enteranceList;


    private void Start (){
        enteranceList = new List<PathNode>();
        SetEnteranceCells();
    }
    
    public void SetEnteranceCells()
    {
        Vector2Int gridPosition = GameGrid.Instance.GetGridPosFromWorld(transform.TransformPoint(Vector3.zero));
        if (mine.GetRotation() == 0){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (mine.GetRotation() == 90){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (mine.GetRotation() == 180){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (mine.GetRotation() == 270){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else {
            Debug.Log("Enterance list of object: " + this.transform.parent.gameObject.name + " is empty.");
            return;
        }
    }

    public List<PathNode> GetEnteranceList () { return enteranceList; }

    public T GetConnectedObject<T>(){
        return (T)(object)mine;
    }
}
