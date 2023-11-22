using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("Object Referances")]
    [SerializeField] private PlayerTurn playerTurn;
    [SerializeField] private AiTurn aiTurn;

    [Header("Turn Information")]
    [SerializeField] private short dayCounter;
    [SerializeField] private short weekCounter;
    [SerializeField] private short monthCounter;

    [Header("Game Information")]
    [SerializeField] short currentPlayerTurn;
    [SerializeField] short playerCount;

    [Header ("Events")]
    public Action OnNewDay;
    public Action OnNewTurn;

    private void Awake()
    {
        Instance = this;
        dayCounter = 1;
        weekCounter = 1;
        monthCounter = 1;
        currentPlayerTurn = 0;
    }

    public void SetupTurnManager (){
        playerCount = PlayerManager.Instance.GetPlayerCount();
    }

    // Starts a new turn
    public void NextTurn ()
    {
        TurnCounter();
        if (currentPlayerTurn < playerCount - 1)
        {
            currentPlayerTurn++;
        }else{
            OnNewDay?.Invoke();
            currentPlayerTurn = 0;
        } 
        OnNewTurn?.Invoke();
    }

    // Updates the turn counter
    private void TurnCounter()
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
    }

    public short GetDay (){ return dayCounter; }

    public short GetWeek (){ return weekCounter; }

    public short GetMonth (){ return monthCounter; }

    public short GetTurn () { return currentPlayerTurn; }  
}   

