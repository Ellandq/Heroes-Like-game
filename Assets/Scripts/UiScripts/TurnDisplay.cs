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
    private int turnCount;

    void Start ()
    {
        TurnManager.Instance.OnNewDay.AddListener(UpdateTurnDisplay);
        turnCount = 1;
        turnDisplay.text = "1";
        dayDisplay.text = "1";
        weekDisplay.text = "1";
        monthDisplay.text = "1";
        GameManager.Instance.StartGame();
    }

    private void UpdateTurnDisplay ()
    {
        dayDisplay.text = Convert.ToString(TurnManager.Instance.dayCounter);
        weekDisplay.text = Convert.ToString(TurnManager.Instance.weekCounter);
        monthDisplay.text = Convert.ToString(TurnManager.Instance.monthCounter);
        turnCount++;
        turnDisplay.text = Convert.ToString(turnCount);
    }
}
