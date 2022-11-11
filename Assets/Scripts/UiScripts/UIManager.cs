using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{   
    [SerializeField] internal PlayerDisplay playerDisplay;

    void Start ()
    {
        TurnManager.OnNewPlayerTurn += ChangeCurrentPlayer;
    }
    private void ChangeCurrentPlayer (Player _currentPlayer)
    {
        playerDisplay.ChangeDisplay(_currentPlayer.playerColorString + " " + _currentPlayer.playerName);
    }
}
