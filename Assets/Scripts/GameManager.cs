using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action <GameState> OnGameStateChanged;

    [Header ("Object References")]
    [SerializeField] public GameObject gameHandler;
    [SerializeField] private UnitSplitWindow unitSplitWindow;
    [SerializeField] private ResourceDisplay resourceDisplay;

    private Coroutine waitForSceneToDeload;
    private Coroutine waitForGameToBeReady;
    
    [Header ("Game State Information")]
    public GameState State;
    private bool mapLoaded;

    [Header ("Map information")]
    [SerializeField] private MapScriptableObject selectedMapInformation;
    [SerializeField] public MapWorldObjects selectedMapWorldObjects;
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
    private short gameComponentsReady = 0;

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
            Destroy(this);
        }
    }

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
        waitForGameToBeReady = StartCoroutine(WaitForGameToBeReady());
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
        
        }   
    }

    public void SaveGame (string _saveFileName)
    {
        
    }

    private void GameSetup()
    {
        GameGrid.Instance.CreateGrid(selectedMapInformation.mapSize);//Creates the game grid

        numberOfPlayers = selectedMapInformation.numberOfPlayers;
        playerTags = selectedMapInformation.players;
        numberOfHumanPlayers = selectedMapInformation.numberOfPossibleHumanPlayers;
        humanPlayerTags = selectedMapInformation.possibleHumanPlayers;

        UpdateGameState(GameState.LoadGame);
    }

    public void StartGame()
    {
        if (gameComponentsReady == 7 && !mapLoaded)
        {
            TurnManager.Instance.GetComponent<TurnManager>().StartGame();
            CameraManager.Instance.cameraMovement.CameraTeleportToWorldObject();
            CameraManager.Instance.EnableCamera();
            mapLoaded = true;
        }
        else gameComponentsReady++;
    }

    public void UpdateGameState (GameState newState)
    {
        State = newState;

        switch (newState){
            case GameState.LoadGame:
                PlayerManager.Instance.SetupPlayerManager();
                TurnManager.Instance.SetupTurnManager();
                WorldObjectManager.Instance.SetupWorldObjects();
            break;

            case GameState.PlayerTurn:
                UIManager.Instance.RefreshCurrentArmyDisplay();
            break;

            case GameState.FinishedTurn:

            break;

            case GameState.AiTurn:

            break;

            case GameState.CityEntered:
                if (CameraManager.Instance.cameraMovement.cameraFollowingObject){
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
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void DisableWorldObjects ()
    {
        gameHandler.SetActive(false);
    }

    public void EnableWorldObjects ()
    {
        gameHandler.SetActive(true);
    }

    public void EnterCity (GameObject cityToEnter, CityFraction _cityFraction)
    {
        UpdateGameState(GameState.CityEntered);
        SceneStateManager.EnterCity(cityToEnter, _cityFraction);
    }

    public void EnterCity (GameObject cityToEnter, CityFraction _cityFraction, Army _interactingArmy)
    {
        UpdateGameState(GameState.CityEntered);
        SceneStateManager.interactingArmy = _interactingArmy;
        SceneStateManager.EnterCity(cityToEnter, _cityFraction);
    }

    public void ExitCity ()
    {
        waitForSceneToDeload = StartCoroutine(WaitForSceneToDeload());
    }

    public bool IsCityOpened ()
    {
        if (SceneManager.sceneCount > 1) return true;
        else return false;
    }

    private IEnumerator WaitForSceneToDeload (){
        while (IsCityOpened()){
            yield return null;
        }
        UpdateGameState(GameState.CityLeft);
    }

    private IEnumerator WaitForGameToBeReady (){
        while (PlayerManager.Instance == null){
            yield return null;
        }
        GameSetup();
        waitForGameToBeReady = null;
    }
}

public enum GameState{
    LoadGame,
    SaveGame,
    PlayerTurn,
    FinishedTurn,
    AiTurn,
    CityEntered,
    CityLeft
}