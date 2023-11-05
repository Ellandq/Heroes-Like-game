using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmySelectionMovementButtons : MonoBehaviour
{
    [SerializeField] private OwnedArmiesDisplay armyDisplay;

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
        int pos = armyDisplay.GetCurrentPosition();
        int maxPosition = PlayerManager.Instance.GetCurrentPlayer().GetOwnedArmies().Count - 3;
        if (maxPosition < 0) maxPosition = 0;
        
        if (pos == minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = false;
        }else if ((pos - 2) <= minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = true;
        }else if ((pos - 2) > 0){
            fastBackwardsButton.interactable = true;
            backwardsButton.interactable = true;
        }

        if (pos == maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = false;
        }else if ((pos + 2) >= maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = true;
        }else if ((pos) < (maxPosition - 2)){
            fastForwardsButton.interactable = true;
            forwardsButton.interactable = true;
        }
    }
}
