using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownAndArmySelection : MonoBehaviour
{
    [SerializeField] TownDisplay townDisplay;
    [SerializeField] ArmyDisplay armyDisplay;

    private void Start ()
    {
        TurnManager.OnNewPlayerTurn += UpdatePlayerDisplay;
    }

    public void UpdatePlayerDisplay (Player player)
    {
        townDisplay.UpdateTownDisplay(player);
        armyDisplay.UpdateArmyDisplay(player);
    }
}
