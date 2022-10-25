using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    internal short objectsToCreate = 0;
    internal short objectsCreated = 0;

    [Header("Player basic information")]
    private City currentCity;
    private Army currentArmy;
    private Mine currentMine;
    public string playerName;
    public string playerColour;
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
    [SerializeField] int goldProduction;
    [SerializeField] int woodProduction;
    [SerializeField] int oreProduction;
    [SerializeField] int gemProduction;
    [SerializeField] int mercuryProduction;
    [SerializeField] int sulfurProduction;
    [SerializeField] int crystalProduction;

    void Start ()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        playerManager.OnNewDayPlayerUpdate.AddListener(DailyResourceGain);
        playerManager.OnNewDayPlayerUpdate.AddListener(PlayerDailyUpdate);
        string[] str = this.name.Split(' ');
        playerColour = str[0];
        playerName = str[1];
        foreach (string colour in playerManager.playablePlayerColours)
        {
            if (playerColour == colour)
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

    public void NewArmy(GameObject newArmy)
    {
        ownedArmies.Add(newArmy);
        currentArmy = ownedArmies[ownedArmies.Count - 1].GetComponent<Army>();
        currentArmy.AddOwningPlayer(this.gameObject);
        objectsToCreate++;
    }

    public void NewCity(GameObject newCity)
    {
        ownedCities.Add(newCity);
        currentCity = ownedCities[ownedCities.Count - 1].GetComponent<City>();
        currentCity.AddOwningPlayer(this.gameObject);
        UpdateResourceGain();
        objectsToCreate++;
    }

    public void NewMine(GameObject newMine)
    {
        ownedMines.Add(newMine);
        currentMine = ownedMines[ownedMines.Count - 1].GetComponent<Mine>();
        currentMine.AddOwningPlayer(this.gameObject);
        UpdateResourceGain();
        objectsToCreate++;
    }
    
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
        playerManager.UpdatePlayerUi(this);
    }

    private void PlayerDailyUpdate ()
    {
        
    }

    public void CheckPlayerStatus()
    {
        objectsCreated++;
        if (objectsCreated == objectsToCreate){
            playerManager.PlayerManagerReady();
        }
    }
}