using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmySelectionMovementButtons : MonoBehaviour
{
    [SerializeField] private ArmyDisplay armyDisplay;

    [Header ("Button References")]
    [SerializeField] private Button fastBackwardsButton;
    [SerializeField] private Button backwardsButton;
    [SerializeField] private Button forwardsButton;
    [SerializeField] private Button fastForwardsButton;
    
    public void MoveDisplay (int moveAmount)
    {
        armyDisplay.RotateDisplay(moveAmount);
    }

    public void UpdateButtonStatus()
    {
        int minPosition = 0;
        int maxPosition = armyDisplay.currentPlayer.ownedArmies.Count - 3;
        if (maxPosition < 0) maxPosition = 0;

        if (armyDisplay.currentPosition == minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = false;
        }else if ((armyDisplay.currentPosition - 2) <= minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = true;
        }else if ((armyDisplay.currentPosition - 2) > 0){
            fastBackwardsButton.interactable = true;
            backwardsButton.interactable = true;
        }

        if (armyDisplay.currentPosition == maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = false;
        }else if ((armyDisplay.currentPosition + 2) >= maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = true;
        }else if ((armyDisplay.currentPosition) < (maxPosition - 2)){
            fastForwardsButton.interactable = true;
            forwardsButton.interactable = true;
        }
    }
}
