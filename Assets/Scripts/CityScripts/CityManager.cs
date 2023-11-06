using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;

    [Header("Interactable objects")]
    private City currentCity;
    public bool armyCreationStatus;

    [Header("City Information")]
    public List <Army> armiesNearCity;
    public List <GridCell> availableEnteranceCells;

    [Header("Events")]
    public UnityEvent onResourcesChanged;
    public UnityEvent<BuildingID> onNewBuildingCreated;

    [Header ("Object referances")]
    [SerializeField] private CityResourceDisplay cityResourceDisplay;
    [SerializeField] private CityInteractibleObjects cityInteractibleObjects;
    [SerializeField] private CityArmyInterface cityArmyInterface;

    [Header ("Events")]
    Coroutine waitForArmyToBeCreated;

    private void Awake ()
    {
        Instance = this;
        armyCreationStatus = false;
        try{
            currentCity = SceneStateManager.displayedCity.GetComponent<City>();
        }catch (NullReferenceException){
            Debug.Log("City object has not been selected");
        }
    }

    private void Start ()
    {
        onResourcesChanged?.Invoke();

        armiesNearCity = new List<Army>();
        availableEnteranceCells = new List<GridCell>();

        foreach (PathNode enteranceNode in currentCity.GetEnteranceList()){
            if (enteranceNode.gridCell.isOccupied && enteranceNode.gridCell.isObjectInteractable){
                WorldObject obj = enteranceNode.gridCell.objectInThisGridSpace.GetComponent<WorldObject>();
                if (obj.GetObjectType() == ObjectType.Army){
                    Debug.Log("Object found near enterance: " + obj.gameObject.name);
                    if ((obj as Army).IsSelectableByCurrentPlayer()){
                        armiesNearCity.Add(obj as Army);
                    }
                }
            }else{
                availableEnteranceCells.Add(enteranceNode.gridCell);
            }
        }
        
        cityArmyInterface.CityInterfaceSetup();
        
        DwellingUI.Instance.UpdateDwellingDisplay();
    }

    public void UpdateResourceDisplay(){
        cityResourceDisplay.UpdateDisplay();
    }

    public void RefreshArmyInterface (){
        cityArmyInterface.RefreshElement();
    }

    public void ExitCityScene ()
    {
        cityArmyInterface.ResetElement();
        if (!armyCreationStatus){
            waitForArmyToBeCreated = StartCoroutine(WaitForArmyToBeCreated());
        }else{
            SceneStateManager.ExitCity();
        }
        
    }

    public City GetCity () { return currentCity; }

    private IEnumerator WaitForArmyToBeCreated (){
        while (!armyCreationStatus){
            yield return null;
        }
        SceneStateManager.ExitCity();
        waitForArmyToBeCreated = null;
    }

    public List<Army> GetArmyList () { return armiesNearCity; } 

    public List<GridCell> GetAvailableEnteranceCells () { return availableEnteranceCells; }
}
