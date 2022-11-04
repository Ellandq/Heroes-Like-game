using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownAndArmySelection : MonoBehaviour
{
    [SerializeField] TownDisplay townDisplay;
    [SerializeField] ArmyDisplay armyDisplay;
    [SerializeField] Player currentPlayer;


    private void Start ()
    {
        TurnManager.OnNewPlayerTurn += UpdatePlayerDisplay;
    }

    public void UpdatePlayerDisplay (Player _player)
    {
        if (currentPlayer != null){
            currentPlayer.onArmyAdded.RemoveAllListeners();
            currentPlayer.onCityAdded.RemoveAllListeners();
        }
        townDisplay.UpdateTownDisplay(_player);
        armyDisplay.UpdateArmyDisplay(_player);
        currentPlayer = _player;
        currentPlayer.onArmyAdded.AddListener(UpdateCurrentArmyDisplay);
        currentPlayer.onCityAdded.AddListener(UpdateCurrentCityDisplay);
    }

    public void UpdateCurrentArmyDisplay ()
    {
        armyDisplay.UpdateArmyDisplay(currentPlayer);
    }

    public void UpdateCurrentCityDisplay ()
    {
        townDisplay.UpdateTownDisplay(currentPlayer);
    }
}
