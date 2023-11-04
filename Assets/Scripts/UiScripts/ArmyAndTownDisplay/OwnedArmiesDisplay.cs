using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedArmiesDisplay : MonoBehaviour
{
    [SerializeField] private ArmySelectionMovementButtons movementButtons;

    [Header ("Army Buttons Information")]
    [SerializeField] private List <RectTransform> armySlotsPosition;
    [SerializeField] private List <ArmyButton> armySlots;

    [Header ("Army Display Positional Information")]
    [SerializeField] private RectTransform rotationObject;
    private Quaternion targetRotation;
    private Vector3 rotation;
    private float rotationSpeed;
    private int currentPosition;
    private bool manualMovementEnabled;
    private bool rotationInProgress = false;

    private void Update ()
    {
        if (!manualMovementEnabled){
            Quaternion currentRotation = rotationObject.localRotation;
            Quaternion desiredRotation = Quaternion.Euler(rotation);

            if (!rotationInProgress) {
                // Only update targetRotation if the rotation is not in progress
                targetRotation = currentRotation;
            }

            float distance = Quaternion.Angle(currentRotation, desiredRotation);

            if (distance > 0.2f)
            {
                // Set rotationInProgress to true when the rotation starts
                rotationInProgress = true;

                targetRotation = Quaternion.Lerp(targetRotation, desiredRotation, Time.deltaTime * rotationSpeed);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < armySlotsPosition.Count; i++){
                    armySlotsPosition[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }
            else if (distance > 0f)
            {
                // Set rotationInProgress to true when the rotation starts
                rotationInProgress = true;

                float maxAngle = Mathf.Min(Time.deltaTime * rotationSpeed, distance);
                targetRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, maxAngle);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < armySlotsPosition.Count; i++){
                    armySlotsPosition[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }
            else {
                // Set rotationInProgress to false when the rotation is completed
                rotationInProgress = false;
            }
        }
    }

    // Updates the rotation to the new position
    public void RotateDisplay (int movementValue){
        UpdateNewSlot(movementValue);
        rotation.z += 30f * movementValue;
        currentPosition += movementValue;
        movementButtons.UpdateButtonStatus();
    }

    // Resets the position to the starting value
    private void ResetDisplayPosition ()
    {
        rotation.z = 0f;
        currentPosition = 0;
        transform.localEulerAngles = rotation;

        for (int i = 0; i < armySlotsPosition.Count; i++){
            armySlotsPosition[i].localRotation = Quaternion.identity;
        }
    }

    // Updates the army display
    public void UpdateArmyDisplay (bool resetPosition = true)
    {
        Player player = PlayerManager.Instance.GetCurrentPlayer();
        ResetArmyDisplay();
        if (resetPosition) ResetDisplayPosition();
        int i = 0;
        foreach (Army army in player.GetOwnedArmies()){
            armySlots[i].UpdateConnectedArmy(army);
            i++;
        }
        movementButtons.UpdateButtonStatus();
    }

    // Resets the army display
    private void ResetArmyDisplay ()
    {
        for (int i = 0; i < 12; i++){
            armySlots[i].ResetArmyButton();
        }
    }

    private void UpdateNewSlot(int amount)
    {
        // Declare variables to keep track of starting and ending slots, as well as the current army index
        int startingSlot;
        int endingSlot;
        int currentArmyIndex;

        List<Army> armies = PlayerManager.Instance.GetCurrentPlayer().GetOwnedArmies();

        // If the rotation is forward
        if (amount > 0)
        {
            // Calculate starting and ending slots, as well as the current army index
            startingSlot = currentPosition + 4;
            endingSlot = Mathf.Min(startingSlot + amount - 1, armies.Count - 1);
            currentArmyIndex = startingSlot;

            // Iterate over the visible slots and update them with the corresponding army
            for (int i = startingSlot; i <= endingSlot; i++)
            {
                // Calculate the actual slot index (wraps around to the beginning of the array if necessary)
                int slotIndex = i % 12;

                // Update the army displayed in this slot
                armySlots[slotIndex].UpdateConnectedArmy(armies[currentArmyIndex]);

                // Move to the next army in the list
                currentArmyIndex++;

                // If we've reached the end of the list, start over from the beginning
                if (currentArmyIndex >= armies.Count)
                {
                    currentArmyIndex = 0;
                }
            }

            // Reset the army display for remaining slots
            for (int i = endingSlot + 1; i < startingSlot + 4; i++)
            {
                // Calculate the actual slot index (wraps around to the beginning of the array if necessary)
                int slotIndex = i % 12;

                // Reset the army button in this slot
                armySlots[slotIndex].ResetArmyButton();
            }
        }
        else // If the rotation is backward
        {
            // Calculate starting and ending slots, as well as the current army index
            startingSlot = currentPosition - 1;
            endingSlot = Mathf.Max(startingSlot + amount + 1, -1);
            currentArmyIndex = startingSlot;

            // Iterate over the visible slots and update them with the corresponding army
            for (int i = startingSlot; i >= endingSlot; i--)
            {
                // Calculate the actual slot index (wraps around to the end of the array if necessary)
                int slotIndex = i % 12;

                // Update the army displayed in this slot
                armySlots[slotIndex].UpdateConnectedArmy(armies[currentArmyIndex]);

                // Move to the next army in the list
                currentArmyIndex--;

                // If we've reached the beginning of the list, start over from the end
                if (currentArmyIndex < 0)
                {
                    currentArmyIndex = armies.Count - 1;
                }
            }

            // Reset the army display for remaining slots
            for (int i = endingSlot - 1; i > startingSlot - 4; i--)
            {
                // Calculate the actual slot index (wraps around to the end of the array if necessary)
                int slotIndex = ((i % 12) + 12) % 12;

                // Reset the army button in this slot
                armySlots[slotIndex].ResetArmyButton();
            }
        }
    }

    public int GetCurrentPosition () { return currentPosition; }
}
