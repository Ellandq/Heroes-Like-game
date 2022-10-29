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
    [SerializeField] GameGrid gameGrid;
    [SerializeField] GameObject uiManager;
    [SerializeField] GameObject turnManager;
    [SerializeField] GameObject gameHandler;

    public GameState State;

    string mapFilePath, mapName;
    string [] mapInformationSplit;
    public short numberOfPlayers;
    public short numberOfHumanPlayers;
    public string [] allPlayerColours;
    public string [] playerColours;
    private short gameComponentsReady = 0;

    public static Action <GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
        mapName = "TestMap";
        mapFilePath = Application.dataPath + "/Maps/" + mapName + "/MapInfo.txt";
        GameSetup();
        uiManager.SetActive(true);
    }

    void Start ()
    {
        Mathf.Clamp(numberOfPlayers, 1, 6);
        
    }

    private void GameSetup()
    {
        mapInformationSplit = File.ReadAllLines(mapFilePath);
        
        gameGrid.CreateGrid(Convert.ToInt32(mapInformationSplit[0]), Convert.ToInt32(mapInformationSplit[1]));//Creates the game grid
        numberOfPlayers = Convert.ToInt16(mapInformationSplit[2]); //Sets up the number of players

        Array.Resize(ref allPlayerColours, numberOfPlayers);
        for (int i = 0; i < allPlayerColours.Length; i++)
        {
            allPlayerColours[i] = mapInformationSplit[i+3]; // Sets up player colours
        }
        numberOfHumanPlayers = Convert.ToInt16(mapInformationSplit[3 + numberOfPlayers]);

        Array.Resize(ref playerColours, numberOfHumanPlayers);
        for (int i = 0; i < playerColours.Length; i++)
        {
            playerColours[i] = mapInformationSplit[i + numberOfPlayers + 4];
        }  
    }

    public void StartGame()
    {
        if (gameComponentsReady == 2)
        {
            turnManager.GetComponent<TurnManager>().StartGame();
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

            break;

            case GameState.CityLeft:

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
}

public enum GameState{
    PlayerTurn,
    FinishedTurn,
    AiTurn,
    CityEntered,
    CityLeft
}