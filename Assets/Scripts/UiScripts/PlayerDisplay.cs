using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject playerDisplay;
    private Text displayedPlayer;

    void Start()
    {
        displayedPlayer = playerDisplay.GetComponent<Text>();
    }
    
    public void ChangeDisplay (string playerName)
    {
        displayedPlayer.text = playerName;
    }
}