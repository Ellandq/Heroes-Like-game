using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownSelectionMovementButtons : MonoBehaviour
{
    [SerializeField] private TownDisplay townDisplay;

    [Header ("Button References")]
    [SerializeField] private Button fastBackwardsButton;
    [SerializeField] private Button backwardsButton;
    [SerializeField] private Button forwardsButton;
    [SerializeField] private Button fastForwardsButton;
    
    public void MoveDisplay (int moveAmount)
    {
        townDisplay.RotateDisplay(moveAmount);
    }

    public void UpdateButtonStatus()
    {
        int minPosition = 0;
        int maxPosition = townDisplay.currentPlayer.ownedArmies.Count - 2;
        if (maxPosition < 0) maxPosition = 0;

        if (townDisplay.currentPosition == minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = false;
        }else if ((townDisplay.currentPosition - 2) <= minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = true;
        }else if ((townDisplay.currentPosition - 2) > 0){
            fastBackwardsButton.interactable = true;
            backwardsButton.interactable = true;
        }

        if (townDisplay.currentPosition == maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = false;
        }else if ((townDisplay.currentPosition - 2) <= maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = true;
        }else if ((townDisplay.currentPosition) < (maxPosition - 2)){
            fastForwardsButton.interactable = true;
            forwardsButton.interactable = true;
        }
    }
}
