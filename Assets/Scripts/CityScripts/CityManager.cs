using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityManager : MonoBehaviour
{
    public static CityManager Instance;

    [Header("Interactable objects")]
    public City currentCity;
    public bool armyCreationStatus;

    [Header("City Information")]
    public List <Army> armiesNearCity;
    public List <GridCell> availableEnteranceCells;

    [Header("Events")]
    public UnityEvent onResourcesChanged;
    public UnityEvent<int> onNewBuildingCreated;

    [Header ("Object referances")]
    [SerializeField] public CityResourceDisplay cityResourceDisplay;
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
        
        if (SceneStateManager.interactingArmy != null){
            CityArmyInterface.Instance.GetArmyUnits(SceneStateManager.interactingArmy);
        }else{
            if (armiesNearCity.Count > 0){
                CityArmyInterface.Instance.GetArmyUnits(armiesNearCity[0]);
            }else{
                CityArmyInterface.Instance.GetArmyUnits();
            }
        }
        
        DwellingUI.Instance.UpdateDwellingDisplay();
    }

    public void ExitCityScene ()
    {
        CityArmyInterface.Instance.ResetElement();
        if (!armyCreationStatus){
            waitForArmyToBeCreated = StartCoroutine(WaitForArmyToBeCreated());
        }else{
            SceneStateManager.ExitCity();
        }
        
    }

    private IEnumerator WaitForArmyToBeCreated (){
        while (!armyCreationStatus){
            yield return null;
        }
        SceneStateManager.ExitCity();
        waitForArmyToBeCreated = null;
    }
}
