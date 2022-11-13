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

    [Header("Player basic information")]
    private City currentCity;
    private Army currentArmy;
    private Mine currentMine;
    public string playerName;
    public string playerColorString;
    public Color playerColor;
    public bool isPlayerAi = true;
    public bool turnDone = false;

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
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(DailyResourceGain);
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(PlayerDailyUpdate);
        string[] str = this.name.Split(' ');
        playerColorString = str[0];
        playerName = str[1];
        foreach (string colour in PlayerManager.Instance.playablePlayerColours)
        {
            if (playerColorString == colour)
            {
                isPlayerAi = false;
            }
        }
        gold = 10000;
        wood = 20;
        ore = 20;
        gems = 5;
        mercury = 5;
        sulfur = 5;
        crystals = 5;
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
                case "Gold":
                    goldProduction += 1000;
                break;

                case "Wood":
                    woodProduction += 2;
                break;
                
                case "Ore":
                    oreProduction += 2;
                break;
                
                case "Gem":
                    gemProduction += 1;
                break;
                
                case "Mercury":
                    mercuryProduction += 1;
                break;
                
                case "Sulfur":
                    sulfurProduction += 1;
                break;
                
                case "Crystal":
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
    public void AddResources (string _resourceType, int _resourceCount)
    {
        switch (_resourceType)
            {
            case "Gold":
                gold += _resourceCount;
            break;

            case "Wood":
                wood += _resourceCount;
            break;
            
            case "Ore":
                ore += _resourceCount;
            break;
            
            case "Gem":
                gems += _resourceCount;
            break;
            
            case "Mercury":
                mercury += _resourceCount;
            break;
            
            case "Sulfur":
                sulfur += _resourceCount;
            break;
            
            case "Crystal":
                crystals += _resourceCount;
            break;
           
        }
        PlayerManager.Instance.UpdatePlayerUI(this);
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
        if (this.gameObject.name != "Neutral Player"){
            if (ownedCities.Count == 0){
                daysToLoose--;
                if (ownedArmies.Count == 0){
                    Debug.Log(this.gameObject.name + " has lost. ");
                    Destroy(this.gameObject);
                }else{
                    if (daysToLoose == 0){
                        Debug.Log(this.gameObject.name + " has lost. ");
                        Destroy(this.gameObject);
                    }
                }
                Debug.Log(this.gameObject.name + " has " + daysToLoose + " turns to regain a city.");
            }
        }
    }

    // Checks if the player is ready for the game to start
    public void CheckPlayerStatus()
    {
        objectsCreated++;
        if (objectsCreated == objectsToCreate){
            PlayerManager.Instance.PlayerManagerReady();
        }
    }

    // Ads an object to follow on the start of the turn
    private void AddFirstObjectToFollow ()
    {
        if (ownedArmies.Count > 0){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(ownedArmies[0].transform.GetChild(0).gameObject);
            ObjectSelector.Instance.AddSelectedObject(ownedArmies[0].transform.GetChild(0).gameObject);
        }else if (ownedCities.Count > 0){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(ownedCities[0].transform.GetChild(0).gameObject);
            ObjectSelector.Instance.AddSelectedObject(ownedCities[0].transform.GetChild(0).gameObject);
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
}