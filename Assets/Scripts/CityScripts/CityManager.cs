using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityManager : MonoBehaviour
{
    [SerializeField] private CityInteractibleObjects cityInteractibleObjects;
    public static CityManager Instance;
    Coroutine waitForArmyToBeCreated;

    [Header("Interactable objects")]
    public City currentCity;
    public UnitsInformation currentCityGarrison;
    public Player owningPlayer;
    public bool armyCreationStatus;

    [Header("City Information")]
    public List <Army> armiesNearCity;
    public List <GridCell> availableEnteranceCells;

    [Header("Events")]
    public UnityEvent onResourcesChanged;
    public UnityEvent<int> onNewBuildingCreated;

    [Header ("Object referances")]
    [SerializeField] public CityResourceDisplay cityResourceDisplay;

    private void Awake ()
    {
        Instance = this;
        armyCreationStatus = false;
        try{
            currentCity = SceneStateManager.displayedCity.GetComponent<City>();
            currentCityGarrison = SceneStateManager.displayedCity.GetComponent<UnitsInformation>();
            owningPlayer = currentCity.ownedByPlayer.GetComponent<Player>();
        }catch (NullReferenceException){
            Debug.Log("City object has not been selected");
        }
    }

    private void Start ()
    {
        onResourcesChanged?.Invoke();

        armiesNearCity = new List<Army>();
        availableEnteranceCells = new List<GridCell>();

        foreach (PathNode enteranceNode in currentCity.enteranceCells){
            if (enteranceNode.gridCell.isOccupied && enteranceNode.gridCell.isObjectInteractable){
                if (enteranceNode.gridCell.objectInThisGridSpace.tag == "Army"){
                    Debug.Log(enteranceNode.gridCell.objectInThisGridSpace);
                    if (enteranceNode.gridCell.objectInThisGridSpace.GetComponent<ObjectInteraction>().relatedArmy.canBeSelectedByCurrentPlayer){
                        armiesNearCity.Add(enteranceNode.gridCell.objectInThisGridSpace.GetComponent<ObjectInteraction>().relatedArmy);
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
