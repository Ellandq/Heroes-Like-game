using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{   
    [SerializeField] internal PlayerDisplay playerDisplay;

    public void Awake ()
    {
        TurnManager.OnNewPlayerTurn += ChangeCurrentPlayer;
        GameManager.Instance.StartGame();
    }

    private void ChangeCurrentPlayer (Player _currentPlayer)
    {
        playerDisplay.ChangeDisplay(Enum.GetName(typeof (PlayerTag), _currentPlayer.thisPlayerTag) + " Player");
    }
}
