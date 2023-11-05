using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dayDisplay;
    [SerializeField] TextMeshProUGUI weekDisplay;
    [SerializeField] TextMeshProUGUI monthDisplay;
    [SerializeField] TextMeshProUGUI turnDisplay;

    void Start ()
    {
        TurnManager.Instance.OnNewDay += UpdateTurnDisplay;
        turnDisplay.text = "1";
        dayDisplay.text = "1";
        weekDisplay.text = "1";
        monthDisplay.text = "1";
        GameManager.Instance.StartGame();
    }

    private void UpdateTurnDisplay ()
    {
        dayDisplay.text = Convert.ToString(TurnManager.Instance.GetDay());
        weekDisplay.text = Convert.ToString(TurnManager.Instance.GetWeek());
        monthDisplay.text = Convert.ToString(TurnManager.Instance.GetMonth());
        turnDisplay.text = Convert.ToString(TurnManager.Instance.GetTurn());
    }
}
