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
    private Coroutine coroutine;
    private float status;

    [Header("Object prefabs")]
    [SerializeField] private GameObject cityPrefab;
    [SerializeField] private GameObject armyPrefab;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private GameObject resourcePrefab;

    [Header("Cities information")]
    private List<GameObject> cities;
    private int numberOfCities = 0;

    [Header("Armies information")]
    private List<GameObject> armies;
    private int numberOfArmies = 0;

    [Header("Mines information")]
    private List<GameObject> mines;
    private int numberOfMines = 0;

    [Header("Resources information")]
    private List<GameObject> resources;
    private int numberOfResources = 0;

    private MapWorldObjects mapWorldObjects;
    
    private void Start (){
        Instance = this;
    }

    public void SetupWorldObjects (MapWorldObjects worldObjects)
    {
        mapWorldObjects = worldObjects;
        coroutine = StartCoroutine(CreateWorldObjects());
    }

    private IEnumerator CreateWorldObjects()
    {
        status = 0f;

        // Step 1: Cities
        for (int i = 0; i < mapWorldObjects.citiesCount; i++)
        {
            float startTime = Time.time;

            List<int> cityInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.cities.Count / mapWorldObjects.citiesCount; j++)
            {
                cityInfoList.Add(mapWorldObjects.cities[j + (mapWorldObjects.cities.Count / mapWorldObjects.citiesCount * i)]);
            }

            // Call the non-coroutine function and check time
            InitialCitySetup(cityInfoList);

            // Check time since the last frame and yield if necessary
            while (Time.time - startTime < 1f / 144f)
            {
                yield return null;
            }

            status += 0.15f; // Adjust progress
        }

        // Step 2: Armies
        for (int i = 0; i < mapWorldObjects.armiesCount; i++)
        {
            float startTime = Time.time;

            List<int> armyInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.armies.Count / mapWorldObjects.armiesCount; j++)
            {
                armyInfoList.Add(mapWorldObjects.armies[j + (mapWorldObjects.armies.Count / mapWorldObjects.armiesCount * i)]);
            }

            // Call the non-coroutine function and check time
            InitialArmySetup(armyInfoList);

            // Check time since the last frame and yield if necessary
            while (Time.time - startTime < 1f / 144f)
            {
                yield return null;
            }

            status += 0.15f; // Adjust progress
        }

        // Step 3: Mines
        for (int i = 0; i < mapWorldObjects.minesCount; i++)
        {
            float startTime = Time.time;

            List<int> mineInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.mines.Count / mapWorldObjects.minesCount; j++)
            {
                mineInfoList.Add(mapWorldObjects.mines[j + (mapWorldObjects.mines.Count / mapWorldObjects.minesCount * i)]);
            }

            // Call the non-coroutine function and check time
            InitialMineSetup(mineInfoList);

            // Check time since the last frame and yield if necessary
            while (Time.time - startTime < 1f / 144f)
            {
                yield return null;
            }

            status += 0.15f; // Adjust progress
        }

        // Step 4: Resources
        for (int i = 0; i < mapWorldObjects.resourcesCount; i++)
        {
            float startTime = Time.time;

            List<int> resourceInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.resources.Count / mapWorldObjects.resourcesCount; j++)
            {
                resourceInfoList.Add(mapWorldObjects.resources[j + (mapWorldObjects.resources.Count / mapWorldObjects.resourcesCount * i)]);
            }

            // Call the non-coroutine function and check time
            InitialResourceSetup(resourceInfoList);

            // Check time since the last frame and yield if necessary
            while (Time.time - startTime < 1f / 144f)
            {
                yield return null;
            }

            status += 0.15f; // Adjust progress
        }

        // Step 5: Dwellings
        for (int i = 0; i < mapWorldObjects.dwellingsCount; i++)
        {
            float startTime = Time.time;

            List<int> dwellingInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.dwellings.Count / mapWorldObjects.dwellingsCount; j++)
            {
                dwellingInfoList.Add(mapWorldObjects.dwellings[j + (mapWorldObjects.dwellings.Count / mapWorldObjects.dwellingsCount * i)]);
            }

            // Call the non-coroutine function and check time
            InitialDwellingSetup(dwellingInfoList);

            // Check time since the last frame and yield if necessary
            while (Time.time - startTime < 1f / 144f)
            {
                yield return null;
            }

            status += 0.15f; // Adjust progress
        }

        // Step 6: Buildings
        for (int i = 0; i < mapWorldObjects.buildingsCount; i++)
        {
            float startTime = Time.time;

            List<int> buildingInfoList = new List<int>();
            for (int j = 0; j < mapWorldObjects.buildings.Count / mapWorldObjects.buildingsCount; j++)
            {
                buildingInfoList.Add(mapWorldObjects.buildings[j + (mapWorldObjects.buildings.Count / mapWorldObjects.buildingsCount * i)]);
            }

            // Call the non-coroutine function and check time
            InitialBuildingSetup(buildingInfoList);

            // Check time since the last frame and yield if necessary
            while (Time.time - startTime < 1f / 144f)
            {
                yield return null;
            }

            status += 0.15f; // Adjust progress
        }

        // Use 'status' to track overall progress and report it as needed
        Debug.Log("Overall progress: " + (status * 100f) + "%");
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
        
        if (GameGrid.Instance.GetCell(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float cityOrientation = Convert.ToSingle(cityInfo[3]);

        // Third part is the town fraction
        // CityFraction cityFraction = (CityFraction)cityInfo[4];
        CityFraction cityFraction = CityFraction.Coalition;
        
        // Fourth part, buildings
        byte [] cityBuildingStatus = new byte [30];
        for (int j = 0; j < 30; j++)
        {
            cityBuildingStatus[j] = (byte)cityInfo[j + 5];
        }
        // Fifth part is the city garrison
        short [] cityGarrison = new short [14];
        for (int j = 0; j < cityGarrison.Length; j++)
        {
            cityGarrison[j] = (short)cityInfo[j + 35];
        }

        cities.Add(Instantiate(cityPrefab, objectPosition, Quaternion.identity));
        cities[numberOfCities].GetComponent<City>().Initialize(
            gridPosition, cityOrientation, ownedByPlayer, cityFraction,  cityBuildingStatus, cityGarrison
        );
        cities[numberOfCities].transform.parent = transform;
        cities[numberOfCities].gameObject.name = "City " + (numberOfCities + 1) + " : " + cityFraction;
        
        GameGrid.Instance.GetCell(gridPosition).isOccupied = true;
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
        
        if (GameGrid.Instance.GetCell(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float mineOrientation = Convert.ToSingle(mineInfo[3]);

        // Third part is the mine type
        List<ResourceType> mineType = new List<ResourceType>(){(ResourceType)mineInfo[4]};

        // Fifth part is the mine garrison (usually empty)
        short [] mineGarrison = new short [14];
        for (int j = 0; j < mineGarrison.Length; j++)
        {
            mineGarrison[j] = (short)mineInfo[j + 5];
        }

        mines.Add(Instantiate(minePrefab, objectPosition, Quaternion.identity));
        mines[numberOfMines].GetComponent<Mine>().Initialize(
            gridPosition, mineOrientation, ownedByPlayer, mineType, mineGarrison
        );
        mines[numberOfMines].transform.parent = transform;
        mines[numberOfMines].gameObject.name = mineType + " Mine : " + (numberOfCities + 1);

        GameGrid.Instance.GetCell(gridPosition).isOccupied = true;
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
        
        if (GameGrid.Instance.GetCell(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float armyOrientation = Convert.ToSingle(armyInfo[3]);

        // Third part are the army units
        short [] armyUnits = new short [14];
        for (int j = 0; j < armyUnits.Length; j++)
        {
            armyUnits[j] = (short)armyInfo[j + 4];
        }

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        armies[numberOfArmies].GetComponent<Army>().Initialize(
            gridPosition, armyOrientation, ownedByPlayer, armyUnits
        );
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = ownedByPlayer + " Army " + (numberOfArmies + 1);

        GameGrid.Instance.GetCell(gridPosition).isOccupied = true;
        numberOfArmies++;   
    }

    public void CreateNewArmy (PlayerTag tag, Vector2Int _gridPosition, int[] _unitType, int[] _unitCount)
    {
        // First part is the playerOnwership
        PlayerTag ownedByPlayer = tag;
        
        // Second part is the grid position and orientation
        Vector2Int gridPosition = _gridPosition;

        if (GameGrid.Instance.GetCell(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }

        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        float armyOrientation = 0f;

        // Third part are the army units
        short [] armyUnits = new short [14];
        for (int j = 0; j < 7; j++)
        {
            armyUnits[2 * j] = (short)_unitType[j];
            armyUnits[2 * j + 1] = (short)_unitCount[j];
        }

        armies.Add(Instantiate(armyPrefab, objectPosition, Quaternion.identity));
        armies[numberOfArmies].GetComponent<Army>().Initialize(
            gridPosition, armyOrientation, ownedByPlayer, armyUnits
        );
        armies[numberOfArmies].transform.parent = transform;
        armies[numberOfArmies].gameObject.name = ownedByPlayer + " Army " + (numberOfArmies + 1);

        GameGrid.Instance.GetCell(gridPosition).AddOccupyingObject(armies[numberOfArmies]);
        numberOfArmies++;  
    }

    public void RemoveArmy (GameObject selectedArmy)
    {
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

        if (GameGrid.Instance.GetCell(gridPosition).isOccupied){
            Debug.Log("Objects overlapping");
            return;
        }
        Vector3 objectPosition = GameGrid.Instance.GetWorldPosFromGridPos(gridPosition);
        ResourceType resourceType = (ResourceType)resourceInfo[2];
        int resourceCount = resourceInfo[3];

        resources.Add(Instantiate(resourcePrefab, objectPosition, Quaternion.identity));
        resources[numberOfResources].GetComponent<ResourcesObject>().Initialize(resourceType, resourceCount, gridPosition);
        resources[numberOfResources].transform.parent = transform;
        resources[numberOfResources].gameObject.name = resourceType + ": " + (numberOfResources + 1);

        GameGrid.Instance.GetCell(gridPosition).AddOccupyingObject(resources[numberOfResources]);
        numberOfResources++;  
    }
    #endregion

    #region Artifacts
    private void InitialArtifactSetup (List<int> artifactInfo)
    {

    }
    #endregion

    public float GetSetUpProgress () { return status; }
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