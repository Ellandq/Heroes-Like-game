using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject uiManager;
    [SerializeField] public GameObject gameHandler;
    [SerializeField] private UnitSplitWindow unitSplitWindow;
    [SerializeField] private ResourceDisplay resourceDisplay;

    private Coroutine waitForSceneToDeload;
    private Coroutine waitForGameToBeReady;

    public GameState State;

    private MapScriptableObject selectedMapInformation;
    string mapFilePath, mapName;
    public short numberOfPlayers;
    public short numberOfHumanPlayers;
    public string [] allPlayerColours;
    public string [] playerColours;
    private short gameComponentsReady = 0;

    public static Action <GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        if (SceneStateManager.selectedMapName != null)  mapName = SceneStateManager.selectedMapName;
        else mapName = SceneStateManager.defaultMap;
        mapFilePath = "Maps/" + mapName + "/MapInformationObject";
        selectedMapInformation = Resources.Load<MapScriptableObject>(mapFilePath);
        waitForGameToBeReady = StartCoroutine(WaitForGameToBeReady());
    }

    private void Start ()
    {
        Mathf.Clamp(numberOfPlayers, 1, 6);
    }

    private void GameSetup()
    {
        GameGrid.Instance.CreateGrid(selectedMapInformation.mapSize);//Creates the game grid
        numberOfPlayers = selectedMapInformation.numberOfPlayers; //Sets up the number of players

        Array.Resize(ref allPlayerColours, numberOfPlayers);
        for (int i = 0; i < allPlayerColours.Length; i++)
        {
            allPlayerColours[i] = selectedMapInformation.players[i]; // Sets up player colours
        }
        numberOfHumanPlayers = selectedMapInformation.numberOfHumanPlayers;

        Array.Resize(ref playerColours, numberOfHumanPlayers);
        for (int i = 0; i < playerColours.Length; i++)
        {
            playerColours[i] = selectedMapInformation.humanPlayers[i];
        } 
        uiManager.SetActive(true);
        PlayerManager.Instance.SetupPlayerManager();
        TurnManager.Instance.SetupTurnManager();
        WorldObjectManager.Instance.SetupWorldObjects();
    }

    public void StartGame()
    {
        if (gameComponentsReady == 9)
        {
            TurnManager.Instance.GetComponent<TurnManager>().StartGame();
            CameraManager.Instance.cameraMovement.CameraTeleportToWorldObject();
        }
        else gameComponentsReady++;
    }

    public void UpdateGameState (GameState newState)
    {
        State = newState;

        switch (newState){
            case GameState.PlayerTurn:

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
                GameManager.Instance.EnableWorldObjects();
                waitForSceneToDeload = null;
                SceneStateManager.interactingArmy = null;
                ArmyInformation.Instance.RefreshElement();
                unitSplitWindow.SetInstanceStatus();
                resourceDisplay.UpdateDisplay();
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

    public void EnterCity (GameObject cityToEnter, string _cityFraction)
    {
        UpdateGameState(GameState.CityEntered);
        SceneStateManager.EnterCity(cityToEnter, _cityFraction);
    }

    public void EnterCity (GameObject cityToEnter, string _cityFraction, Army _interactingArmy)
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

    public IEnumerator WaitForSceneToDeload (){
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
    PlayerTurn,
    FinishedTurn,
    AiTurn,
    CityEntered,
    CityLeft
}