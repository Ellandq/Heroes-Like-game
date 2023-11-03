using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private Coroutine coroutine;
    private float setUpProgress;

    private string defaultPlayerName = "Player";

    [Header("Player information")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Player neutralPlayer;
    private PlayerTag currentPlayer;

    [Header ("All Players Information")]
    private Dictionary<PlayerTag, Player> players;
    private List<PlayerTag> allPlayers;
    private List<PlayerTag> humanPlayers;

    [Header("Player colors")]
    [SerializeField] private List<Color> playerColorList;

    public void Awake (){
        Instance = this;
    }

    public void SetupPlayerManager ()
    {
        allPlayers = GameManager.Instance.playerTags;
        humanPlayers = GameManager.Instance.humanPlayerTags;
        setUpProgress = 0f;
        coroutine = StartCoroutine(CreatePlayers(GameManager.Instance.numberOfPlayers));
    }

    public void StartGame (){
        foreach (var pl in players){
            pl.Value.TurnUpdate();
        }
    }

    public void NewDayUpdate (){
        foreach (PlayerTag tag in allPlayers){
            players[tag].DailyUpdate();
        }
    }

    public void NewTurnUpdate (){
        currentPlayer = GetNextPlayer();
        foreach (var pl in players){
            pl.Value.TurnUpdate();
        }
    }

    public void RemovePlayer (PlayerTag tag) { players.Remove(tag); }

    public short GetPlayerCount (){ return Convert.ToInt16(players.Count); }

    // Sets the player color 
    public Color GetPlayerColour (PlayerTag tag)
    {
        return playerColorList[(int)tag];
    }

    public Player GetPlayer (PlayerTag tag){
        return players[tag];
    }

    public Player GetNeutralPlayer () { return neutralPlayer; }

    public PlayerTag GetCurrentPlayer (){
        return currentPlayer;
    }

    public PlayerTag GetNextPlayer (){
        return allPlayers[(allPlayers.FindIndex(a => a == currentPlayer) + 1) % allPlayers.Count];
    }

    public ResourceIncome GetStartingResources (){
        return new ResourceIncome(new int[7] { 10000, 10, 10, 5, 5, 5, 5 });
    }

    public bool IsPlayerAi (PlayerTag player){
        foreach (PlayerTag tag in humanPlayers){
            if (tag == player) return false;
        }
        return true;
    }

    public float GetSetUpProgress (){ return setUpProgress; }

    private IEnumerator CreatePlayers(int howManyPlayers)
    {
        players = new Dictionary<PlayerTag, Player>(howManyPlayers);

        if (playerPrefab == null)
        {
            Debug.LogError("Error: Player Prefab on the Player Manager is not assigned");
            yield break; // Exit the coroutine if playerPrefab is not assigned.
        }

        for (int i = 0; i < howManyPlayers; i++)
        {
            GameObject pl = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            players[allPlayers[i]] = pl.GetComponent<Player>();
            players[allPlayers[i]].SetUpPlayer(allPlayers[i], IsPlayerAi(allPlayers[i]));
            pl.transform.parent = transform;
            pl.gameObject.name = Enum.GetName(typeof(PlayerTag), allPlayers[i]) + " " + defaultPlayerName;

            setUpProgress = (float)(i + 1) / howManyPlayers;

            // Check elapsed time since the last frame
            if (Time.deltaTime < 1f / 144f)
            {
                // Yield a frame if the frame time is less than 1/144 of a second
                yield return null;
            }
        }

        currentPlayer = allPlayers[0];
    }
}
public enum PlayerTag{
    None, Blue, LightBlue, Purple, Red, Orange, Yellow, LightGreen, Green
}

public enum PlayerState{
    TurnCompleted, TurnWaiting, TurnCurrent, PlayerDefeated, PlayerWon,
}