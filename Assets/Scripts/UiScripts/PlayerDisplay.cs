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
        if (playerDisplay != null){
            displayedPlayer = playerDisplay.GetComponent<Text>();
        }
    }
    
    public void ChangeDisplay (string playerName)
    {
        if (playerDisplay != null){
            displayedPlayer.text = playerName;
        }
    }
}