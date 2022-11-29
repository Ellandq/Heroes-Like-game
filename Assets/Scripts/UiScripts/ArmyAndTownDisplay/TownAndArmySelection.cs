using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownAndArmySelection : MonoBehaviour
{
    public static TownAndArmySelection Instance;
    [SerializeField] TownDisplay townDisplay;
    [SerializeField] ArmyDisplay armyDisplay;
    [SerializeField] Player currentPlayer;

    private void Start ()
    {
        Instance = this;
        TurnManager.OnNewPlayerTurn += UpdatePlayerDisplay;
        GameManager.Instance.StartGame();
    }

    // Updates the Player display
    public void UpdatePlayerDisplay (Player _player)
    {
        if (currentPlayer != null){
            currentPlayer.onArmyAdded.RemoveAllListeners();
            currentPlayer.onCityAdded.RemoveAllListeners();
        }
        currentPlayer = _player;
        currentPlayer.onArmyAdded.AddListener(UpdateCurrentArmyDisplay);
        currentPlayer.onCityAdded.AddListener(UpdateCurrentCityDisplay);
        townDisplay.UpdateTownDisplay(currentPlayer);
        armyDisplay.UpdateArmyDisplay(currentPlayer);
    }

    // Updates the army display
    public void UpdateCurrentArmyDisplay ()
    {
        armyDisplay.UpdateArmyDisplay(currentPlayer);
    }

    // Updates the city display
    public void UpdateCurrentCityDisplay ()
    {
        townDisplay.UpdateTownDisplay(currentPlayer);
    }

    private void OnEnable ()
    {
        if (currentPlayer != null)  UpdateCurrentArmyDisplay();
    }
}
