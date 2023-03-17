using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDisplay : MonoBehaviour
{
    [SerializeField] private ArmySelectionMovementButtons movementButtons;
    [SerializeField] public Player currentPlayer;

    [Header ("Army Buttons Information")]
    [SerializeField] private List <RectTransform> armySlotsPosition;
    [SerializeField] private List <ArmyButton> armySlots;

    [Header ("Army Display Positional Information")]
    [SerializeField] private RectTransform rotationObject;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public int currentPosition;
    private Quaternion targetRotation;
    [SerializeField] private bool manualMovementEnabled;

    private void Start ()
    {
        UIManager.Instance.UIManagerReady();
    }

    // Updates the army display
    internal void UpdateArmyDisplay (Player player)
    {
        currentPlayer = player;
        ResetArmyDisplay();
        ResetDisplayPosition();
        for (int i = 0; i < currentPlayer.ownedArmies.Count && i < 4; i++){
            armySlots[i].UpdateConnectedArmy(currentPlayer.ownedArmies[i]);
        }
        movementButtons.UpdateButtonStatus();
    }

    internal void UpdateArmyDisplay ()
    {
        ResetArmyDisplay();
        for (int i = 0; i < currentPlayer.ownedArmies.Count && i < 4; i++){
            armySlots[i].UpdateConnectedArmy(currentPlayer.ownedArmies[i]);
        }
        movementButtons.UpdateButtonStatus();
    }

    // Rotates the object if the 
    private void Update ()
    {
        if (!manualMovementEnabled){
            Quaternion currentRotation = rotationObject.localRotation;
            Quaternion desiredRotation = Quaternion.Euler(rotation);

            float distance = Quaternion.Angle(currentRotation, desiredRotation);
            
            if (distance > 0.2f)
            {
                targetRotation = Quaternion.Lerp(targetRotation, desiredRotation, Time.deltaTime * rotationSpeed);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < armySlotsPosition.Count; i++){
                    armySlotsPosition[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }
            else if (distance > 0f)
            {
                float maxAngle = Mathf.Min(Time.deltaTime * rotationSpeed, distance);
                targetRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, maxAngle);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < armySlotsPosition.Count; i++){
                    armySlotsPosition[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }  
        }
    }

    // Resets the army display
    private void ResetArmyDisplay ()
    {
        for (int i = 0; i < 12; i++){
            armySlots[i].ResetArmyButton();
        }
    }

    // Updates the slots that will be visable when the object rotation updates
    private void UpdateNewSlot (int amount){
        int startingSlot;
        int endingSlot;
        int currentArmyIndex;

        // If movement is forward
        if (amount > 0){
            startingSlot = (currentPosition + 4);
            endingSlot = startingSlot + amount - 1;
            currentArmyIndex = startingSlot;

            // Ensure the slot numbers stay withing the range 0-11
            if (startingSlot > 11)startingSlot %= 12;
            if (endingSlot > 11) endingSlot %= 12;
            
            // Iterate over slots that will be visable after the rotation
            for (int i = startingSlot; (i >= startingSlot || i <= endingSlot) && !(i > endingSlot && i < startingSlot) && currentArmyIndex < currentPlayer.ownedArmies.Count; i++){
                // Update the army displayed in this slot
                armySlots[i].UpdateConnectedArmy(currentPlayer.ownedArmies[currentArmyIndex]);

                if (i == 11){
                    Debug.Log("Reset button: 0");
                    armySlots[0].ResetArmyButton();
                }else{
                    Debug.Log("Reset button: " + (i + 1));
                    armySlots[i + 1].ResetArmyButton();
                }

                // If we've reached the end of the array, start over from the beginning
                if (i == 11){
                    if (endingSlot == i) break;
                    i = 0;
                }
                currentArmyIndex++;
            }
        }else{ // If movement is backwards
            startingSlot = (currentPosition - 1);
            endingSlot = startingSlot + amount + 1;
            currentArmyIndex = startingSlot;

            // Ensure the slot numbers stay withing the range 0-11
            if (startingSlot > 11)startingSlot %= 12;
            if (endingSlot > 11) endingSlot %= 12;
            
            for (int i = startingSlot; (i <= startingSlot || i >= endingSlot) && !(i < endingSlot && i > startingSlot) && currentArmyIndex >= 0; i--){
                // Update the army displayed in this slot
                armySlots[i].UpdateConnectedArmy(currentPlayer.ownedArmies[currentArmyIndex]);

                if (i == 0){
                    Debug.Log("Reset button: 11");
                    armySlots[11].ResetArmyButton();
                }else{
                    Debug.Log("Reset button: " + (i - 1));
                    armySlots[i - 1].ResetArmyButton();
                }

                // If we've reached the beggining of the array, start over from the end
                if (i == 0){
                    if (endingSlot == i) break;
                    i = 11;
                }
                currentArmyIndex--;
            }
        }
    }

    // Updates the rotation to the new position
    public void RotateDisplay (int movementValue){
        UpdateNewSlot(movementValue);
        rotation.z += (30f * movementValue);
        currentPosition += movementValue;
        movementButtons.UpdateButtonStatus();
    }

    // Resets the position to the starting value
    private void ResetDisplayPosition ()
    {
        rotation.z = 0f;
        currentPosition = 0;
        transform.localEulerAngles = rotation;
    }
}
