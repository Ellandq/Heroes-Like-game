using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityEnterance : MonoBehaviour
{
    [SerializeField] City city;
    GameGrid gameGrid;
    private List <PathNode> enteranceList;
    private Vector2Int gridPosition;
    private float cityRotation;

    void Start ()
    {
        gameGrid = FindObjectOfType<GameGrid>();
        gridPosition = gameGrid.GetGridPosFromWorld(transform.TransformPoint(Vector3.zero));
        cityRotation = city.GetCityRotation();
        enteranceList = new List<PathNode>();
        GetEnteranceCells();
    }

    private void GetEnteranceCells()
    {
        if (cityRotation == 0){
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (cityRotation == 90){
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (cityRotation == 180){
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (cityRotation == 270){
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(gameGrid.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else {
            Debug.Log("Enterance list of object: " + this.transform.parent.gameObject.name + " is empty.");
            return;
        }

        this.gameObject.GetComponent<ObjectEnterance>().enteranceNodes = enteranceList;
        city.GetEnteranceInformation(enteranceList);
    }
}
