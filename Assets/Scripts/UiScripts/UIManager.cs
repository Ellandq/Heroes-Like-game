using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance;
    private int uiComponentsReady;

    [Header ("Current Player Information")]
    [SerializeField] private Player currentPlayer;

    [Header ("Resources UI")]
    [SerializeField] private ResourceDisplay resourceDisplay;

    [Header ("Current Army Information UI")]
    [SerializeField] private ArmyInformation armyInformation;

    [Header ("Army Interface UI")]
    [SerializeField] private ArmyInterfaceArmyInformation armyInterface;

    [Header ("Army and Town Display UI")]
    [SerializeField] private TownDisplay townDisplay;
    [SerializeField] private ArmyDisplay armyDisplay;

    public void Awake ()
    {
        Instance = this;
        TurnManager.OnNewPlayerTurn += ActivePlayerChangeUpdate;
        GameManager.Instance.StartGame();
        uiComponentsReady = 0;
    }

    public void UIManagerReady ()
    {
        if (uiComponentsReady == 3)
        {
            GameManager.Instance.StartGame();
        }
        else uiComponentsReady++;
    }

    // Function to be called every time a new player is selected
    private void ActivePlayerChangeUpdate (Player currentPlayer){
        UpdatePlayerDisplay(currentPlayer);
        resourceDisplay.UpdateDisplay(currentPlayer);
    }

    #region TownAndArmyDisplay

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
        townDisplay.UpdateCityDisplay(currentPlayer);
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
        townDisplay.UpdateCityDisplay(currentPlayer);
    }

    #endregion

    #region CurrentArmyDisplay

    public void RefreshCurrentArmyDisplay ()
    {
        armyInformation.RefreshElement();
    }

    #endregion

    #region ResourcesDisplay

    public void UpdateResourceDisplay ()
    {
        resourceDisplay.UpdateDisplay();
    }

    #endregion
}
