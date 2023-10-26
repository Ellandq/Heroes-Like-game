using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
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

    private MapWorldObjects mapWorldObjects;
    
    private void Start ()
    {
        Instance = this;
    }

    public void SetupWorldObjects ()
    {
        mapWorldObjects = GameManager.Instance.selectedMapWorldObjects;
        CreateWorldObjects();
    }

    private void CreateWorldObjects()
    {
        // Cities
        for (int i = 0; i < mapWorldObjects.citiesCount; i++){
            List<int>cityInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.cities.Count/mapWorldObjects.citiesCount; j++){
                cityInfoList.Add(mapWorldObjects.cities[j + (mapWorldObjects.cities.Count/mapWorldObjects.citiesCount * i)]);
            }
            InitialCitySetup(cityInfoList);
        }

        // Armies
        for (int i = 0; i < mapWorldObjects.armiesCount; i++){
            List<int>armyInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.armies.Count/mapWorldObjects.armiesCount; j++){
                armyInfoList.Add(mapWorldObjects.armies[j + (mapWorldObjects.armies.Count/mapWorldObjects.armiesCount * i)]);
            }
            InitialArmySetup(armyInfoList);
        }

        // Mines
        for (int i = 0; i < mapWorldObjects.minesCount; i++){
            List<int>mineInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.mines.Count/mapWorldObjects.minesCount; j++){
                mineInfoList.Add(mapWorldObjects.mines[j + (mapWorldObjects.mines.Count/mapWorldObjects.minesCount * i)]);
            }
            InitialMineSetup(mineInfoList);
        }

        // Resources
        for (int i = 0; i < mapWorldObjects.resourcesCount; i++){
            List<int>resourceInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.resources.Count/mapWorldObjects.resourcesCount; j++){
                resourceInfoList.Add(mapWorldObjects.resources[j + (mapWorldObjects.resources.Count/mapWorldObjects.resourcesCount * i)]);
            }
            InitialResourceSetup(resourceInfoList);
        }

        // Dwellings
        for (int i = 0; i < mapWorldObjects.dwellingsCount; i++){
            List<int>dwellingInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.dwellings.Count/mapWorldObjects.dwellingsCount; j++){
                dwellingInfoList.Add(mapWorldObjects.dwellings[j + (mapWorldObjects.dwellings.Count/mapWorldObjects.dwellingsCount * i)]);
            }
            InitialDwellingSetup(dwellingInfoList);
        }

        // Buildings
        for (int i = 0; i < mapWorldObjects.buildingsCount; i++){
            List<int>buildingInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.buildings.Count/mapWorldObjects.buildingsCount; j++){
                buildingInfoList.Add(mapWorldObjects.buildings[j + (mapWorldObjects.buildings.Count/mapWorldObjects.buildingsCount * i)]);
            }
            InitialBuildingSetup(buildingInfoList);
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
    private void InitialCitySetup (List<int> cityInfo)
    {
        // First part is the playerOnwership
        PlayerTag ownedByPlayer = (PlayerTag)cityInfo[0];
        
        // Second part is the grid position and orientation
        Vector2Int gridPosition = new Vector2Int(cityInfo[1], cityInfo[2]);
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float cityOrientation = Convert.ToSingle(cityInfo[3]);

        // Third part is the town fraction
        // CityFraction cityFraction = (CityFraction)cityInfo[4];
        CityFraction cityFraction = CityFraction.Coalition;
        
        // Fourth part, buildings
        int [] cityBuildingStatus = new int [30];
        for (int j = 0; j < 30; j++)
        {
            cityBuildingStatus[j] = cityInfo[j + 5];
        }
        // Fifth part is the city garrison
        int [] cityGarrison = new int [14];
        for (int j = 0; j < cityGarrison.Length; j++)
        {
            cityGarrison[j] = cityInfo[j + 35];
        }

        cities.Add(Instantiate(cityPrefab, objectPosition, Quaternion.identity));
        cities[numberOfCities].GetComponent<City>().CityInitialization(
            ownedByPlayer, cityFraction, gridPosition, cityOrientation, cityBuildingStatus, cityGarrison
        );
        cities[numberOfCities].transform.parent = transform;
        cities[numberOfCities].gameObject.name = "City " + (numberOfCities + 1) + " : " + cityFraction;
        
        // Adding the city to the approprieate player
        
        for (int i = 0; i < playerManager.players.Count; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewCity(cities[numberOfCities]);
                break;
            }
        }
        if (ownedByPlayer  == PlayerTag.None){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewCity(cities[numberOfCities]);
        }
        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfCities++;   
    }
    #endregion

    #region Building
    private void InitialBuildingSetup (List<int> buildingInfo)
    {

    }
    #endregion

    #region Mine
    private void InitialMineSetup (List<int> mineInfo)
    {
        // First part is the playerOnwership
        PlayerTag ownedByPlayer = (PlayerTag)mineInfo[0];
        
        // Second part is the grid position and orientation
        Vector2Int gridPosition = new Vector2Int(mineInfo[1], mineInfo[2]);
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float mineOrientation = Convert.ToSingle(mineInfo[3]);

        // Third part is the mine type
        ResourceType mineType = (ResourceType)mineInfo[4];

        // Fifth part is the mine garrison (usually empty)
        int [] mineGarrison = new int [14];
        for (int j = 0; j < mineGarrison.Length; j++)
        {
            mineGarrison[j] = mineInfo[j + 5];
        }

        mines.Add(Instantiate(minePrefab, objectPosition, Quaternion.identity));
        mines[numberOfMines].GetComponent<Mine>().MineInitialization(
            ownedByPlayer, mineType, gridPosition, mineOrientation, mineGarrison
        );
        mines[numberOfMines].transform.parent = transform;
        mines[numberOfMines].gameObject.name = mineType + " Mine : " + (numberOfCities + 1);

        // Adding the mine to the approprieate player
        
        for (int i = 0; i < playerManager.players.Count; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewMine(mines[numberOfMines]);
                break;
            }
        }
        if (ownedByPlayer  == PlayerTag.None){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewMine(mines[numberOfMines]);
        }

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfMines++;
    }
    #endregion

    #region Dwelling
    private void InitialDwellingSetup (List<int> dwellingInfo)
    {

    }
    #endregion

    #region Army
    private void InitialArmySetup (List<int> armyInfo)
    {
        // First part is the playerOnwership
        PlayerTag ownedByPlayer = (PlayerTag)armyInfo[0];
        
        // Second part is the grid position and orientation
        Vector2Int gridPosition = new Vector2Int(armyInfo[1], armyInfo[2]);
        
        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float armyOrientation = Convert.ToSingle(armyInfo[3]);

        // Third part are the army units
        int [] armyUnits = new int [14];
        for (int j = 0; j < armyUnits.Length; j++)
        {
            armyUnits[j] = armyInfo[j + 4]; // ?
        }

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        armies[numberOfArmies].GetComponent<Army>().ArmyInitialization(
            ownedByPlayer, gridPosition, armyOrientation, armyUnits
        );
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = ownedByPlayer + " Army " + (numberOfArmies + 1);

        // Adding the army to the approprieate player
        for (int i = 0; i < playerManager.players.Count; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewArmy(armies[numberOfArmies]);
                break;
            }
        }
        if (ownedByPlayer  == PlayerTag.None){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewArmy(armies[numberOfArmies]);
        }

        GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied = true;
        numberOfArmies++;   
    }

    public void CreateNewArmy (PlayerTag tag, Vector2Int _gridPosition, int[] _unitType, int[] _unitCount)
    {
        // First part is the playerOnwership
        PlayerTag ownedByPlayer = tag;
        
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
        
        for (int i = 0; i < playerManager.players.Count; i++)
        {
            if (playerManager.players[i].name == ownedByPlayer + " Player")
            {
                chosenPlayer = playerManager.players[i].GetComponent<Player>();
                chosenPlayer.NewArmy(armies[numberOfArmies]);
                break;
            }
        }
        if (ownedByPlayer  == PlayerTag.None){
            chosenPlayer = playerManager.neutralPlayer.GetComponent<Player>();
            chosenPlayer.NewArmy(armies[numberOfArmies]);
        }

        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(armies[numberOfArmies]);
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
    private void InitialResourceSetup (List<int> resourceInfo)
    {
        Vector2Int gridPosition = new Vector2Int(resourceInfo[0], resourceInfo[1]);

        if (GameGrid.Instance.GetGridCellInformation(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }
        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        ResourceType resourceType = (ResourceType)resourceInfo[2];
        int resourceCount = resourceInfo[3];

        resources.Add(Instantiate(resourcePrefab, objectPosition, Quaternion.identity));
        resources[numberOfResources].GetComponent<ResourcesObj>().ResourceInitialization(resourceType, resourceCount, gridPosition);
        resources[numberOfResources].transform.parent = transform;
        resources[numberOfResources].gameObject.name = resourceType + ": " + (numberOfResources + 1);

        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(resources[numberOfResources]);
        numberOfResources++;  
    }
    #endregion

    #region Artifacts
    private void InitialArtifactSetup (List<int> artifactInfo)
    {

    }
    #endregion
}

public enum CityFraction{
    Random, Bazaar, Coalition, DarkOnes, Hive, Magic, Temple
}

public enum ObjectType{
    City, Army, Mine, Dwelling, Building, Resource, None
}

public enum ResourceType{
    Gold, Wood, Ore, Gems, Mercury, Sulfur, Crystal
}

public enum CityBuildingState{
    NotBuilt = 0, Built = 1, Blocked = 2
}

public enum BuildingType{
    OneByOne,
    TwoByOne, OneByTwo, TwoByTwo,
    ThreeByOne, ThreeByTwo, ThreeByThree,
    FiveByFive
}