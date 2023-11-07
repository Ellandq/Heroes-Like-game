using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action <GameState> OnGameStateChanged;

    [Header ("Object References")]
    [SerializeField] public GameObject worldObjects;
    
    [Header ("Game State Information")]
    public GameState state;
    private bool mapLoaded;

    [Header ("Map information")]
    [SerializeField] private MapScriptableObject selectedMapInformation;
    [SerializeField] private MapWorldObjects selectedMapWorldObjects;
    [SerializeField] private MapWorldObjects tmpMapWorldObjects;
    [SerializeField] private SaveFileScriptableObject saveFileScriptableObject;

    [Header ("PLayer Information")]
    [SerializeField] public short numberOfPlayers;
    [SerializeField] public List<PlayerTag> playerTags;
    [SerializeField] public short numberOfHumanPlayers;
    [SerializeField] public List<PlayerTag> humanPlayerTags;

    [Header ("Save information")]
    [SerializeField] private string saveFileName, saveFilePath;
    private string mapFilePath, mapName;

    [Header ("Coroutines")]
    private Coroutine waitForSceneToDeload;
    private Coroutine gameSetup;

    private void Awake()
    {
        
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
            if (SceneManager.GetActiveScene().name == "Overworld"){
                mapLoaded = false;
                NewGame();
            }
        }else{
            mapLoaded = false;
            NewGame();
            Destroy(gameObject);
        }
    }

    #region Load-And-Save

    private void NewGame ()
    {
        if (SceneStateManager.selectedMapName != null){
            mapName = SceneStateManager.selectedMapName;
        }
        else{
            mapName = SceneStateManager.defaultMap;
        }

        mapFilePath = Application.dataPath + "/Maps/" + mapName;
        
        if (Directory.Exists(Application.dataPath + "/Maps")){
            BinaryFormatter bf = new BinaryFormatter();
            if (Directory.Exists(mapFilePath)){
                FileStream file = File.Open(mapFilePath + "/MapInformation.txt", FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), selectedMapInformation);
                file.Close();

                file = File.Open(mapFilePath + "/MapWorldObjects.txt", FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), selectedMapWorldObjects);
                file.Close();
            }else{
                Debug.LogError("Selected map does not exist");
            }
        }else{
            Debug.LogError("The Map folder does not exist");
        }
        gameSetup = StartCoroutine(WaitForManagers());
    }

    public void LoadGame (string _saveFileName)
    {
        string _mapName = selectedMapWorldObjects.mapName;
        

        if (Directory.Exists(Application.dataPath + "/SaveFiles")){
            BinaryFormatter bf = new BinaryFormatter();
            saveFileName = _saveFileName;
            saveFilePath = Application.dataPath + "/SaveFiles/" + saveFileName;

            //  Load the mapWorldObjects information from save file
            FileStream file = File.Open(saveFilePath + "/MapWorldObjects.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), tmpMapWorldObjects);
            file.Close();

            if (tmpMapWorldObjects.mapName == _mapName){
                selectedMapWorldObjects = tmpMapWorldObjects;
            }else{
                // Load the map information from the "Maps" folder
                file = File.Open(mapFilePath + "/MapInformation.txt", FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), selectedMapInformation);
                file.Close();
                
            }
        }else{
            Debug.LogError("Map couldn't be loaded. ");
        }  
    }

    public void SaveGame (string _saveFileName)
    {
        
    }

    #endregion

    #region Game State

    public void StartGame()
    {
        gameSetup = null;
        PlayerManager.Instance.StartGame();
        CameraManager.Instance.cameraMovement.CameraTeleportToWorldObject();
        CameraManager.Instance.EnableCamera();
        mapLoaded = true;
        UpdateGameState(GetNewGameState());
    }

    public void UpdateGameState (GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.LoadGame:
                gameSetup = StartCoroutine(LoadWorldAsync());
                break;

            case GameState.PlayerTurn:
                UIManager.Instance.RefreshCurrentArmyDisplay();
                break;

            case GameState.FinishedTurn:
                // Handle FinishedTurn
                // Add your logic here
                break;

            case GameState.AiTurn:
                // Handle AiTurn
                // Add your logic here
                break;

            case GameState.CityEntered:
                if (CameraManager.Instance.cameraMovement.cameraFollowingObject)
                {
                    CameraManager.Instance.cameraMovement.CameraTeleportToWorldObject();
                }
                break;

            case GameState.CityLeft:
                EnableWorldObjects();
                waitForSceneToDeload = null;
                SceneStateManager.interactingArmy = null;
                UIManager.Instance.UpdateResourceDisplay();
                UpdateGameState(GameState.PlayerTurn);
                break;

            case GameState.SaveGame:
                // Handle SaveGame
                // Add your logic here
                break;

            case GameState.BattleStarted:
                if (CameraManager.Instance.cameraMovement.cameraFollowingObject)
                {
                    CameraManager.Instance.cameraMovement.CameraTeleportToWorldObject();
                }
                break;

            case GameState.BattleFinished:
                // Handle BattleFinished
                // Add your logic here
                break;

            default:
                // Handle the default case (if needed)
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    #endregion

    #region Scene Management

    public void EnableWorldObjects (bool status = true)
    {
        worldObjects.SetActive(status);
    }

    public void EnterCity (City cityToEnter, Army interactingArmy = null)
    {
        UpdateGameState(GameState.CityEntered);
        SceneStateManager.EnterCity(cityToEnter, interactingArmy);
    }

    public void ExitCity ()
    {
        waitForSceneToDeload = StartCoroutine(WaitForSceneToDeload());
    }

    public void StartBattle (Army attackingArmy, Army defendingArmy){
        UpdateGameState(GameState.BattleStarted);
        // TODO
    }

    public void EndBattle (){
        // TODO
    }

    public bool IsSceneOpened ()
    {
        if (SceneManager.sceneCount > 1) return true;
        else return false;
    }

    #endregion

    #region Getters

    public bool AreWorldObjectsActive () { return worldObjects.activeSelf; }

    private GameState GetNewGameState (){
        switch (state)
        {
            case GameState.LoadGame:
                return GameState.PlayerTurn;
            case GameState.SaveGame:
                return GameState.PlayerTurn;
            case GameState.PlayerTurn:
                // Handle PlayerTurn
                break;
            case GameState.FinishedTurn:
                // Handle FinishedTurn
                break;
            case GameState.AiTurn:
                // Handle AiTurn
                break;
            case GameState.CityEntered:
                // Handle CityEntered
                break;
            case GameState.CityLeft:
                return GameState.PlayerTurn;
            case GameState.BattleStarted:
                return GameState.BattleFinished;
            case GameState.BattleFinished:
                return GameState.PlayerTurn;
            default:
                // Handle the default case (if needed)
                break;
        }
        return state;
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitForSceneToDeload (){
        while (IsSceneOpened()){
            yield return null;
        }
        UpdateGameState(GetNewGameState());
    }

    private IEnumerator WaitForManagers (){
        while (PlayerManager.Instance == null && TurnManager.Instance == null 
        && WorldObjectManager.Instance == null && UIManager.Instance == null){
            yield return null;
        }
        UpdateGameState(GameState.LoadGame);
    }

    private IEnumerator LoadWorldAsync()
    {
        float status = 0f;
        numberOfPlayers = selectedMapInformation.numberOfPlayers;
        playerTags = selectedMapInformation.players;
        numberOfHumanPlayers = selectedMapInformation.numberOfPossibleHumanPlayers;
        humanPlayerTags = selectedMapInformation.possibleHumanPlayers;

        // Step 1: Game Grid Setup
        GameGrid.Instance.GameGridSetUp(selectedMapInformation.mapSize);

        while (GameGrid.Instance.GetSetUpProgress() < 1f)
        {
            Debug.Log("Step 1 progress: " + (GameGrid.Instance.GetSetUpProgress() * 100f) + "%");
            yield return null;
        }
        status += 0.25f; // Adjust progress

        // Step 2: Player Manager Setup
        PlayerManager.Instance.SetupPlayerManager();
        Debug.Log("Step 2 progress: 25%");
        status += 0.15f; // Adjust progress

        // Step 3: Turn Manager Setup
        TurnManager.Instance.SetupTurnManager();
        Debug.Log("Step 3 progress: 40%");
        status += 0.1f; // Adjust progress

        UIManager.Instance.SetupUIManager();
        Debug.Log("Step 4 progress: 50%");
        status += 0.1f; // Adjust progress

        // Step 4: World Object Manager Setup
        WorldObjectManager.Instance.SetupWorldObjects(selectedMapWorldObjects);

        while (WorldObjectManager.Instance.GetSetUpProgress() < 1f)
        {
            Debug.Log("Step 5 progress: " + (WorldObjectManager.Instance.GetSetUpProgress() * 100f) + "%");
            yield return null;
        }
        status += 0.35f; // Adjust progress

        yield return null;

        Debug.Log("Step 6 progress: 95%");
        ObjectSelector.Instance.SetupObjectSelector();
        
        status += .05f;

        // Use 'status' to track overall progress and report it as needed
        Debug.Log("Overall progress: " + (status * 100f) + "%");

        StartGame();
    }

    #endregion
}

public enum GameState{
    // Load and save
    LoadGame, SaveGame,
    // Turn
    PlayerTurn, FinishedTurn, AiTurn,
    // City
    CityEntered, CityLeft,
    // Battle
    BattleStarted, BattleFinished
}