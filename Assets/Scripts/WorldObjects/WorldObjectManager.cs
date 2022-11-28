using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;
    [SerializeField] PlayerManager playerManager;
    Player chosenPlayer;

    [Header("Object prefabs")]
    [SerializeField] private GameObject cityPrefab;
    [SerializeField] private GameObject armyPrefab;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private GameObject resourcePrefab;

    [Header("Cities information")]
    public List<GameObject> cities;
    public int numberOfCities = 0;

    [Header("Armies information")]
    public List<GameObject> armies;
    public int numberOfArmies = 0;

    [Header("Mines information")]
    public List<GameObject> mines;
    public int numberOfMines = 0;

    [Header("Resources information")]
    public List<GameObject> resources;
    public int numberOfResources = 0;


    private string [] worldObjectArray;
    private string currentReadLine;
    private string [] splitLine;
    private string myFilePath;
    private string mapName;
    
    void Start ()
    {
        Instance = this;
    }

    public void SetupWorldObjects ()
    {
        if (SceneStateManager.selectedMapName != null)  mapName = SceneStateManager.selectedMapName;
        else mapName = SceneStateManager.defaultMap;
        myFilePath = "Maps/" + mapName + "/WorldObjects";
        ReadfromFile();
    }

    private void ReadfromFile ()
    {
        worldObjectArray = Resources.Load<TextAsset>(myFilePath).text.Split('\n');
        for (int i = 0; i < worldObjectArray.Length; i++)
        {
            currentReadLine = worldObjectArray[i];
            splitLine = currentReadLine.Split(' ');
            if (splitLine[0] == "Enviroment")
            {
                InitialEnviromentSetup(splitLine);
            }
            else if (splitLine[0] == "City")
            {
                InitialCitySetup(splitLine);
            }
            else if (splitLine[0] == "NeutralBuilding")
            {
                InitialNeutralBuildingSetup(splitLine);
            }
            else if (splitLine[0] == "Mine")
            {
                InitialMineSetup(splitLine);
            }
            else if (splitLine[0] == "Dwelling")
            {
                InitialDwellingSetup(splitLine);
            }
            else if (splitLine[0] == "Army")
            {
                InitialArmySetup(splitLine);
            }
            else if (splitLine[0] == "Resource")
            {
                InitialResourceSetup(splitLine);
            }
            else if (splitLine[0] == "Artifact")
            {
                InitialArtifactSetup(splitLine);
            }
            else
            {
                Debug.LogError("World object is not defined");
            }
        }
        GameManager.Instance.StartGame();
    }
    #region Enviroment
    private void InitialEnviromentSetup (string [] _splitLine)
    {
        //for now it's gonna be empty
    }
    #endregion

    #region City
    private void InitialCitySetup (string [] _splitLine)
    {
        // First part is the playerOnwership
        string ownedByPlayer = _splitLine[1];
        
        // Second part is the grid position and orientation
        int gridPosX = Convert.ToInt32(_splitLine[2]);
        int gridPosY = Convert.ToInt32(_splitLine[3]);
        Vector2Int gridPosition = new Vector2Int(gridPosX, gridPosY);
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float cityOrientation = float.Parse(_splitLine[4]);

        // Third part is the town fraction
        string cityFraction = _splitLine[5];

        // Fourth part, buildings
        int [] cityBuildingStatus = new int [30];
        for (int j = 0; j < 30; j++)
        {
            cityBuildingStatus[j] = Convert.ToInt32(_splitLine[j + 6]);
        }
        // Fifth part is the city garrison
        int [] cityGarrison = new int [14];
        for (int j = 0; j < cityGarrison.Length; j++)
        {
            cityGarrison[j] = Convert.ToInt32(_splitLine[j + 36]);
        }

        cities.Add(Instantiate(cityPrefab, objectPosition, Quaternion.identity));
        cities[numberOfCities].GetComponent<City>().CityInitialization(
            ownedByPlayer, cityFraction, gridPosition, cityOrientation, cityBuildingStatus, cityGarrison
        );
        cities[numberOfCities].transform.parent = transform;
        cities[numberOfCities].gameObject.name = "City " + (numberOfCities + 1) + " : " + cityFraction;

        // Adding the city to the approprieate player
        
        for (int i = 0; i < playerManager.players.Length; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewCity(cities[numberOfCities]);
                break;
            }
        }
        if (ownedByPlayer  == "Neutral"){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewCity(cities[numberOfCities]);
        }
        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfCities++;   
    }
    #endregion

    #region Building
    private void InitialNeutralBuildingSetup (string [] _splitLine)
    {

    }
    #endregion

    #region Mine
    private void InitialMineSetup (string [] _splitLine)
    {
        // First part is the playerOnwership
        string ownedByPlayer = _splitLine[1];
        
        // Second part is the grid position and orientation
        int gridPosX = Convert.ToInt32(_splitLine[2]);
        int gridPosY = Convert.ToInt32(_splitLine[3]);
        Vector2Int gridPosition = new Vector2Int(gridPosX, gridPosY);
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float mineOrientation = float.Parse(_splitLine[4]);

        // Third part is the mine type
        string mineType = _splitLine[5];

        // Fifth part is the mine garrison (usually empty)
        int [] mineGarrison = new int [14];
        for (int j = 0; j < mineGarrison.Length; j++)
        {
            mineGarrison[j] = Convert.ToInt32(_splitLine[j + 6]);
        }

        mines.Add(Instantiate(minePrefab, objectPosition, Quaternion.identity));
        mines[numberOfMines].GetComponent<Mine>().MineInitialization(
            ownedByPlayer, mineType, gridPosition, mineOrientation, mineGarrison
        );
        mines[numberOfMines].transform.parent = transform;
        mines[numberOfMines].gameObject.name = mineType + " Mine : " + (numberOfCities + 1);

        // Adding the mine to the approprieate player
        
        for (int i = 0; i < playerManager.players.Length; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewMine(mines[numberOfMines]);
                break;
            }
        }
        if (ownedByPlayer  == "Neutral"){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewMine(mines[numberOfMines]);
        }

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfMines++;
    }
    #endregion

    #region Dwelling
    private void InitialDwellingSetup (string [] _splitLine)
    {

    }
    #endregion

    #region Army
    private void InitialArmySetup (string [] _splitLine)
    {
        // First part is the playerOnwership
        string ownedByPlayer = _splitLine[1];
        
        // Second part is the grid position and orientation
        int gridPosX = Convert.ToInt32(_splitLine[2]);
        int gridPosY = Convert.ToInt32(_splitLine[3]);
        Vector2Int gridPosition = new Vector2Int(gridPosX, gridPosY);

        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float armyOrientation = float.Parse(_splitLine[4]);

        // Third part are the army units
        int [] armyUnits = new int [14];
        for (int j = 0; j < armyUnits.Length; j++)
        {
            armyUnits[j] = Convert.ToInt32(_splitLine[j + 3]);
        }

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        armies[numberOfArmies].GetComponent<Army>().ArmyInitialization(
            ownedByPlayer, gridPosition, armyOrientation, armyUnits
        );
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = ownedByPlayer + " Army " + (numberOfArmies + 1);

        // Adding the army to the approprieate player
        for (int i = 0; i < playerManager.players.Length; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewArmy(armies[numberOfArmies]);
                break;
            }
        }
        if (ownedByPlayer  == "Neutral"){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewArmy(armies[numberOfArmies]);
        }

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfArmies++;   
    }

    public void CreateNewArmy (string _playerColor, Vector2Int _gridPosition, int[] _unitType, int[] _unitCount)
    {
        // First part is the playerOnwership
        string ownedByPlayer = _playerColor;
        
        // Second part is the grid position and orientation
        Vector2Int gridPosition = _gridPosition;

        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float armyOrientation = 0f;

        // Third part are the army units
        int [] armyUnits = new int [14];
        for (int j = 0; j < 7; j++)
        {
            armyUnits[2 * j] = _unitType[j];
            armyUnits[2 * j + 1] = _unitCount[j];
        }

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        armies[numberOfArmies].GetComponent<Army>().ArmyInitialization(
            ownedByPlayer, gridPosition, armyOrientation, armyUnits
        );
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = ownedByPlayer + " Army " + (numberOfArmies + 1);

        // Adding the army to the approprieate player
        
        for (int i = 0; i < playerManager.players.Length; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewArmy(armies[numberOfArmies]);
                break;
            }
        }
        if (ownedByPlayer  == "Neutral"){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewArmy(armies[numberOfArmies]);
        }

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfArmies++;  
    }

    public void RemoveArmy (GameObject selectedArmy)
    {
        chosenPlayer = selectedArmy.GetComponent<Army>().ownedByPlayer.GetComponent<Player>();
        chosenPlayer.RemoveArmy(selectedArmy);
        for (int i = 0; i < armies.Count; i++){
            if (armies[i].name == selectedArmy.name){
                armies.RemoveAt(i);
            }
        }
        numberOfArmies--;  
    }
    #endregion

    #region Resources
    private void InitialResourceSetup (string [] _splitLine)
    {
        int gridPosX = Convert.ToInt32(_splitLine[1]);
        int gridPosY = Convert.ToInt32(_splitLine[2]);
        Vector2Int gridPosition = new Vector2Int(gridPosX, gridPosY);

        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }
        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        string resourceType = _splitLine[3];
        int resourceCount = Convert.ToInt32(_splitLine[4]);

        resources.Add(Instantiate(resourcePrefab, objectPosition, Quaternion.identity));
        resources[numberOfResources].GetComponent<ResourcesObj>().ResourceInitialization(resourceType, resourceCount, gridPosition);
        resources[numberOfResources].transform.parent = transform;
        resources[numberOfResources].gameObject.name = resourceType + ": " + (numberOfResources + 1);

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfResources++;  
    }
    #endregion

    #region Artifacts
    private void InitialArtifactSetup (string [] _splitLine)
    {

    }
    #endregion

    
}
