using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("Object Referances")]
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerTurn playerTurn;
    [SerializeField] AiTurn aiTurn;
    [SerializeField] GameObject uiManager;
    Player player;

    [Header("Turn Information")]
    [SerializeField] internal short dayCounter;
    [SerializeField] internal short weekCounter;
    [SerializeField] internal short monthCounter;

    [Header("Game Information")]
    [SerializeField] short currentPlayerTurn;
    [SerializeField] short playerCount;

    public static event Action<Player> OnNewPlayerTurn;
    public UnityEvent OnNewDay;
    public UnityEvent OnTurnManagerReady;

    void Start()
    {
        Instance = this;
        dayCounter = 1;
        weekCounter = 1;
        monthCounter = 1;

        currentPlayerTurn = 0;
        playerCount = Convert.ToInt16(playerManager.players.Length);
        player = playerManager.players[currentPlayerTurn].GetComponent<Player>();
        OnTurnManagerReady.Invoke();
    }

    public void StartGame ()
    {
        OnNewPlayerTurn?.Invoke(player);
        player.NewTurnUpdate();  
    }

    public void nextTurn ()
    {
        player.GetSelectedObject();
        if (currentPlayerTurn < playerCount - 1)
        {
            currentPlayerTurn++;
        }else{
            OnNewDay.Invoke();
            currentPlayerTurn = 0;
        } 
        player = playerManager.players[currentPlayerTurn].GetComponent<Player>();
        OnNewPlayerTurn.Invoke(player);
        player.NewTurnUpdate();  
    }

    public void TurnCounter()
    {
        if (dayCounter != 7){
            dayCounter++;
        }else{
            dayCounter = 1;
            if (weekCounter != 4){
                weekCounter++;
            }else {
                monthCounter++;
                weekCounter = 1;
            }
        }
        currentPlayerTurn++;
    }
}   

