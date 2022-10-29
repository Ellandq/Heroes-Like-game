using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    private string defaultPlayerName = "Player";
    private string [] playerColours;
    public string [] playablePlayerColours;
    private short playersReady = 0;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] public GameObject neutralPlayer;
    public GameObject[] players;
    [SerializeField] Player currentPlayer;

    public UnityEvent OnPlayerManagerReady;
    public UnityPlayerEvent OnNextPlayerTurn;
    public UnityEvent OnNewDayPlayerUpdate;
    public static event Action OnCurrentPlayerResourcesGained;

    public void Awake ()
    {
        playerColours = GameManager.Instance.allPlayerColours;
        playablePlayerColours = GameManager.Instance.playerColours;
        CreatePlayers(GameManager.Instance.numberOfPlayers);
        TurnManager.OnNewPlayerTurn += NextPlayerTurn;
        TurnManager.OnNewPlayerTurn += UpdateCurrentPlayer;
        
    }

    private void CreatePlayers (int howManyPlayers)
    {
        players = new GameObject[howManyPlayers];

        if (playerPrefab == null)
        {
            Debug.LogError("Error: Player Prefab on the Player Manager is not assigned");
        }

        for (int i = 0; i < howManyPlayers; i++)
        {
            players[i] = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            players[i].GetComponent<Player>();
            players[i].transform.parent = transform;
            players[i].gameObject.name = playerColours[i] + " " + defaultPlayerName;
        }
    }

    public void NewDayUpdate ()
    {
        OnNewDayPlayerUpdate?.Invoke();
    }

    public void NextPlayerTurn (Player _player)
    {
        OnNextPlayerTurn?.Invoke(_player);
    }

    public void PlayerManagerReady ()
    {
        playersReady++;
        if (playersReady == players.Length){
            OnPlayerManagerReady.Invoke();
        }
    }

    private void UpdateCurrentPlayer (Player _player)
    {
        currentPlayer = _player;
    }

    public void UpdatePlayerUi (Player _player)
    {
        if (_player == currentPlayer) OnCurrentPlayerResourcesGained?.Invoke();
    }
}

[System.Serializable]
public class UnityPlayerEvent : UnityEvent<Player> { }