using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private GameObject playerDisplay;
    private Text displayedPlayer;

    void Awake()
    {
        if (playerDisplay != null){
            displayedPlayer = playerDisplay.GetComponent<Text>();
        }
        GameManager.Instance.StartGame();
    }
    
    public void ChangeDisplay (string playerName)
    {
        if (playerDisplay != null){
            displayedPlayer.text = playerName;
        }
    }
}