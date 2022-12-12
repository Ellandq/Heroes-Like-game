using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Coroutine waitForObjectToBeDestroyed;
    public UnityEvent onArmyAdded;
    public UnityEvent onCityAdded;
    internal short objectsToCreate = 0;
    internal short objectsCreated = 0;
    private short daysToLoose = 4;
    private GameObject lastObjectSelectedByPlayer;
    private bool playerLost;

    private GameObject objectToDestroy;

    [Header("Player basic information")]
    public PlayerTag thisPlayerTag;
    public Color playerColor;
    public bool isPlayerAi = true;
    public bool turnDone = false;

    private City currentCity;
    private Army currentArmy;
    private Mine currentMine;

    [Header("Player structures and armies: ")]
    public List<GameObject> ownedArmies;
    public List<GameObject> ownedCities;
    public List<GameObject> ownedMines;

    [Header("Player resources")]
    public int gold;
    public int wood;
    public int ore;
    public int gems;
    public int mercury;
    public int sulfur;
    public int crystals;

    [Header("Player daily production")]
    [SerializeField] private int goldProduction;
    [SerializeField] private int woodProduction;
    [SerializeField] private int oreProduction;
    [SerializeField] private int gemProduction;
    [SerializeField] private int mercuryProduction;
    [SerializeField] private int sulfurProduction;
    [SerializeField] private int crystalProduction;

    private void Start ()
    {
        playerLost = false;
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(DailyResourceGain);
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(PlayerDailyUpdate);
        
        gold = 10000;
        wood = 20;
        ore = 20;
        gems = 5;
        mercury = 5;
        sulfur = 5;
        crystals = 5;

        PlayerManager.Instance.PlayerManagerReady();
    }

    // Adds a new army for the player
    public void NewArmy(GameObject newArmy)
    {
        ownedArmies.Add(newArmy);
        currentArmy = ownedArmies[ownedArmies.Count - 1].GetComponent<Army>();
        currentArmy.AddOwningPlayer(this.gameObject);
        objectsToCreate++;
    }

    // Removes a given army from the player
    public void RemoveArmy (GameObject armyToRemove)
    {  
        if (GameManager.Instance.gameHandler.activeSelf){
            
            for (int i = 0; i < ownedArmies.Count; i++){
                if (ownedArmies[i].name == armyToRemove.name){
                    ownedArmies.RemoveAt(i);
                    Destroy(armyToRemove);
                }
            }
            if (waitForObjectToBeDestroyed == null)
            {
                waitForObjectToBeDestroyed = StartCoroutine(WaitForObjectToBeDestroyed(armyToRemove));
            }
        }else{
            objectToDestroy = armyToRemove;
        }
    }

    // Adds a new City for the player
    public void NewCity(GameObject newCity)
    {
        ownedCities.Add(newCity);
        currentCity = ownedCities[ownedCities.Count - 1].GetComponent<City>();
        currentCity.AddOwningPlayer(this.gameObject);
        UpdateResourceGain();
        objectsToCreate++;
    }

    // Adds a new mine for the player
    public void NewMine(GameObject newMine)
    {
        ownedMines.Add(newMine);
        currentMine = ownedMines[ownedMines.Count - 1].GetComponent<Mine>();
        currentMine.AddOwningPlayer(this.gameObject);
        UpdateResourceGain();
        objectsToCreate++;
    }
    
    // Resets and checks the player resource gain
    public void UpdateResourceGain()
    {
        goldProduction = 0;
        woodProduction = 0;
        oreProduction = 0;
        gemProduction = 0;
        mercuryProduction = 0;
        sulfurProduction = 0;
        crystalProduction = 0;

        for (int i = 0; i < ownedCities.Count; i++)
        {
            goldProduction += ownedCities[i].GetComponent<City>().cityGoldProduction;
        }

        for (int i = 0; i < ownedMines.Count; i++)
        {
            switch (ownedMines[i].GetComponent<Mine>().mineType)
            {
                case ResourceType.Gold:
                    goldProduction += 1000;
                break;

                case ResourceType.Wood:
                    woodProduction += 2;
                break;
                
                case ResourceType.Ore:
                    oreProduction += 2;
                break;
                
                case ResourceType.Gems:
                    gemProduction += 1;
                break;
                
                case ResourceType.Mercury:
                    mercuryProduction += 1;
                break;
                
                case ResourceType.Sulfur:
                    sulfurProduction += 1;
                break;
                
                case ResourceType.Crystal:
                    crystalProduction += 1;
                break;
                
            }
        }  
    }

    // Gives an approprieate amount of resources to the player on a new day
    private void DailyResourceGain()
    {
        UpdateResourceGain();
        gold += goldProduction;
        wood += woodProduction;
        ore += oreProduction;
        gems += gemProduction;
        mercury += mercuryProduction;
        sulfur += sulfurProduction;
        crystals += crystalProduction;
    }

    // Adds a given resource to the player
    public void AddResources (ResourceType _resourceType, int _resourceCount)
    {
        switch (_resourceType)
            {
            case ResourceType.Gold:
                gold += _resourceCount;
            break;

            case ResourceType.Wood:
                wood += _resourceCount;
            break;
            
            case ResourceType.Ore:
                ore += _resourceCount;
            break;
            
            case ResourceType.Gems:
                gems += _resourceCount;
            break;
            
            case ResourceType.Mercury:
                mercury += _resourceCount;
            break;
            
            case ResourceType.Sulfur:
                sulfur += _resourceCount;
            break;
            
            case ResourceType.Crystal:
                crystals += _resourceCount;
            break;
           
        }
        PlayerManager.Instance.UpdatePlayerUI(this);
    }

    // Removes a set amount of resources
    public void RemoveResources (int[] resourceList)
    {
        gold -= resourceList[0];
        wood -= resourceList[1];
        ore -= resourceList[2];
        gems -= resourceList[3];
        mercury -= resourceList[4];
        sulfur -= resourceList[5];
        crystals -= resourceList[6];
    }
    
    // A daily player update
    private void PlayerDailyUpdate ()
    {
        CheckPlayerLooseCondition();
    }

    // A player update on a new turn
    public void NewTurnUpdate ()
    {
        if (lastObjectSelectedByPlayer != null){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(lastObjectSelectedByPlayer);
            ObjectSelector.Instance.AddSelectedObject(lastObjectSelectedByPlayer);
        }else{
            AddFirstObjectToFollow();
        }
    }

    // Checks what object is currently selected
    public void GetSelectedObject ()
    {
        lastObjectSelectedByPlayer =  ObjectSelector.Instance.lastObjectSelected;
    }

    // Checks if any loose conditions are met
    private void CheckPlayerLooseCondition ()
    {
        if (this.gameObject.name != "Neutral Player" && !playerLost){
            if (ownedCities.Count == 0){
                daysToLoose--;
                if (ownedArmies.Count == 0){
                    Debug.Log(this.gameObject.name + " has lost. ");
                    int objectCount = ownedArmies.Count - 1;
                    for (int i = objectCount; i >= 0; i--){
                        WorldObjectManager.Instance.RemoveArmy(ownedArmies[i]);
                    }
                    objectCount = ownedCities.Count - 1;
                    for (int i = objectCount; i >= 0; i--){
                        ownedCities[i].GetComponent<City>().RemoveOwningPlayer();
                    }
                    objectCount = ownedMines.Count - 1;
                    for (int i = objectCount; i >= 0; i--){
                        ownedMines[i].GetComponent<Mine>().RemoveOwningPlayer();
                    }
                    playerLost = true;
                }else{
                    if (daysToLoose == 0){
                        Debug.Log(this.gameObject.name + " has lost. ");
                        int objectCount = ownedArmies.Count - 1;
                        for (int i = objectCount; i >= 0; i--){
                            WorldObjectManager.Instance.RemoveArmy(ownedArmies[i]);
                        }
                        objectCount = ownedCities.Count - 1;
                        for (int i = objectCount; i >= 0; i--){
                            ownedCities[i].GetComponent<City>().RemoveOwningPlayer();
                        }
                        objectCount = ownedMines.Count - 1;
                        for (int i = objectCount; i >= 0; i--){
                            ownedMines[i].GetComponent<Mine>().RemoveOwningPlayer();
                        }
                        playerLost = true;
                    }
                }
                if (!playerLost){
                    Debug.Log(this.gameObject.name + " has " + daysToLoose + " turns to regain a city.");
                }
            }
        }
    }

    // Ads an object to follow on the start of the turn
    private void AddFirstObjectToFollow ()
    {
        if (ownedArmies.Count > 0){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(ownedArmies[0].transform.GetChild(0).gameObject);
            ObjectSelector.Instance.AddSelectedObject(ownedArmies[0].transform.GetChild(0).gameObject);
        }else if (ownedCities.Count > 0){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(ownedCities[0]);
            ObjectSelector.Instance.AddSelectedObject(ownedCities[0]);
        }
    }

    // What to do when the player GameObject is destroyed
    private void OnDestroy()
    {
        PlayerManager.Instance.OnNewDayPlayerUpdate.RemoveListener(DailyResourceGain);
        PlayerManager.Instance.OnNewDayPlayerUpdate.RemoveListener(PlayerDailyUpdate);
    }

    // Enumerator to wait for the selected object to be destroyed so correct updates can be run
    private IEnumerator WaitForObjectToBeDestroyed (GameObject objectToDestroy){
        while (objectToDestroy != null){
            yield return null;
        }
        ObjectSelector.Instance.RemoveSelectedObject();
        if (PlayerManager.Instance.currentPlayer == this) TownAndArmySelection.Instance.UpdatePlayerDisplay(this); 

        waitForObjectToBeDestroyed = null;
    }

    private void OnEnable()
    {
        if (objectToDestroy != null){
            RemoveArmy(objectToDestroy);
            objectToDestroy = null;
        }
    }
}