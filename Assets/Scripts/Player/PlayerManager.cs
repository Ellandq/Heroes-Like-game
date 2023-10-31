using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private string defaultPlayerName = "Player";

    [Header("Player information")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Player neutralPlayer;
    [SerializeField] private Player currentPlayer;
    private List<Player> players;

    [Header ("All Players Information")]
    [SerializeField] private List<PlayerTag> allPlayers;
    [SerializeField] private List<PlayerTag> humanPlayers;

    [Header("Player colors")]
    [SerializeField] private List<Color> playerColorList;
    [SerializeField] private Color blue;
    [SerializeField] private Color lightBlue;
    [SerializeField] private Color purple;
    [SerializeField] private Color red;
    [SerializeField] private Color orange;
    [SerializeField] private Color yellow;
    [SerializeField] private Color lightGreen;
    [SerializeField] private Color green;

    [Header("Events")]
    public UnityPlayerEvent OnNextPlayerTurn;
    public UnityEvent OnNewDayPlayerUpdate;

    public void Awake (){
        Instance = this;
    }

    public void SetupPlayerManager ()
    {
        allPlayers = GameManager.Instance.playerTags;
        humanPlayers = GameManager.Instance.humanPlayerTags;
        CreatePlayers(GameManager.Instance.numberOfPlayers);
        TurnManager.Instance.OnNewPlayerTurn += NewTurnUpdate;
        TurnManager.Instance.OnNewDay += NewDayUpdate;
    }

    // Creates a new player
    private void CreatePlayers (int howManyPlayers)
    {
        players = new List<Player>(howManyPlayers);

        if (playerPrefab == null)
        {
            Debug.LogError("Error: Player Prefab on the Player Manager is not assigned");
        }

        for (int i = 0; i < howManyPlayers; i++) 
        {   
            GameObject pl = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            players.Add(pl.GetComponent<Player>());
            players[i].SetUpPlayer(allPlayers[i]);
            pl.transform.parent = transform;
            pl.gameObject.name = Enum.GetName(typeof (PlayerTag), allPlayers[i]) + " " + defaultPlayerName;

        }
        GameManager.Instance.StartGame();
    }

    public void NewDayUpdate ()
    {
        OnNewDayPlayerUpdate?.Invoke();
    }

    private void NewTurnUpdate (PlayerTag tag){

    }

    // An update to run on each new turn
    public void NextPlayerTurn (Player _player)
    {
        OnNextPlayerTurn?.Invoke(_player);
    }

    // Updates the current player 
    private void UpdateCurrentPlayer (Player player)
    {
        currentPlayer = player;
    }

    public int GetPlayerCount (){ return players.Count; }

    // Sets the player color 
    public Color GetPlayerColour (PlayerTag tag)
    {
        return playerColorList[(int)tag - 1];
    }

    public Player GetPlayer (PlayerTag tag){
        foreach (Player pl in players){
            if (pl.GetPlayerTag() == tag) return pl;
        }
        return null;
    }

    public Player GetCurrentPlayer (){
        return currentPlayer;
    }

    public ResourceIncome GetStartingResources (){
        return new ResourceIncome(new int[7] { 10000, 10, 10, 5, 5, 5, 5 });
    }
}
public enum PlayerTag{
    None, Blue, LightBlue, Purple, Red, Orange, Yellow, LightGreen, Green
}

public enum PlayerState{
    TurnCompleted, TurnWaiting, TurnCurrent, PlayerDefeated, PlayerWon,
}

[System.Serializable]
public class UnityPlayerEvent : UnityEvent<PlayerTag> { }