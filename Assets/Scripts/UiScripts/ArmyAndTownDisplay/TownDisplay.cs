using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownDisplay : MonoBehaviour
{
    [SerializeField] private TownSelectionMovementButtons movementButtons;
    [SerializeField] public Player currentPlayer;

    [Header ("Town Buttons Information")]
    [SerializeField] private List <RectTransform> citySlotsPosition;
    [SerializeField] private List <TownButton> citySlots;

    [Header ("Town Display Positional Information")]
    [SerializeField] private RectTransform rotationObject;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public int currentPosition;
    private Quaternion targetRotation;
    [SerializeField] private bool manualMovementEnabled;

    // Add a flag variable to track whether the rotation is in progress
    private bool rotationInProgress = false;

    private void Start ()
    {
        UIManager.Instance.UIManagerReady();
    }

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

                for (int i = 0; i < citySlotsPosition.Count; i++){
                    citySlotsPosition[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }
            else if (distance > 0f)
            {
                // Set rotationInProgress to true when the rotation starts
                rotationInProgress = true;

                float maxAngle = Mathf.Min(Time.deltaTime * rotationSpeed, distance);
                targetRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, maxAngle);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < citySlotsPosition.Count; i++){
                    citySlotsPosition[i].localRotation = Quaternion.Inverse(targetRotation);
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
        rotation.z += (30f * movementValue);
        currentPosition += movementValue;
    }

    // Resets the position to the starting value
    private void ResetDisplayPosition ()
    {
        rotation.z = 0f;
        currentPosition = 0;
        transform.localEulerAngles = rotation;
    }

    // Resets the town display
    private void ResetTownDisplay ()
    {
        for (int i = 0; i < 12; i++){
            citySlots[i].ResetCityButton();
        }
    }

    private void UpdateNewSlot(int amount)
    {
        // Declare variables to keep track of starting and ending slots, as well as the current City index
        int startingSlot;
        int endingSlot;
        int currentCityIndex;

        // If the rotation is forward
        if (amount > 0)
        {
            // Calculate starting and ending slots, as well as the current City index
            startingSlot = currentPosition + 4;
            endingSlot = Mathf.Min(startingSlot + amount - 1, currentPlayer.ownedArmies.Count - 1);
            currentCityIndex = startingSlot;

            // Iterate over the visible slots and update them with the corresponding City
            for (int i = startingSlot; i <= endingSlot; i++)
            {
                // Calculate the actual slot index (wraps around to the beginning of the array if necessary)
                int slotIndex = i % 12;

                // Update the City displayed in this slot
                citySlots[slotIndex].UpdateConnectedCity(currentPlayer.ownedArmies[currentCityIndex]);

                // Move to the next City in the list
                currentCityIndex++;

                // If we've reached the end of the list, start over from the beginning
                if (currentCityIndex >= currentPlayer.ownedArmies.Count)
                {
                    currentCityIndex = 0;
                }
            }

            // Reset the City display for remaining slots
            for (int i = endingSlot + 1; i < startingSlot + 4; i++)
            {
                // Calculate the actual slot index (wraps around to the beginning of the array if necessary)
                int slotIndex = i % 12;

                // Reset the City button in this slot
                citySlots[slotIndex].ResetCityButton();
            }
        }
        else // If the rotation is backward
        {
            // Calculate starting and ending slots, as well as the current City index
            startingSlot = currentPosition - 1;
            endingSlot = Mathf.Max(startingSlot + amount + 1, -1);
            currentCityIndex = startingSlot;

            // Iterate over the visible slots and update them with the corresponding City
            for (int i = startingSlot; i >= endingSlot; i--)
            {
                // Calculate the actual slot index (wraps around to the end of the array if necessary)
                int slotIndex = i % 12;

                // Update the City displayed in this slot
                citySlots[slotIndex].UpdateConnectedCity(currentPlayer.ownedArmies[currentCityIndex]);

                // Move to the next City in the list
                currentCityIndex--;

                // If we've reached the beginning of the list, start over from the end
                if (currentCityIndex < 0)
                {
                    currentCityIndex = currentPlayer.ownedArmies.Count - 1;
                }
            }

            // Reset the City display for remaining slots
            for (int i = endingSlot - 1; i > startingSlot - 4; i--)
            {
                // Calculate the actual slot index (wraps around to the end of the array if necessary)
                int slotIndex = ((i % 12) + 12) % 12;

                // Reset the City button in this slot
                citySlots[slotIndex].ResetCityButton();
            }
        }
    }

    // Updates the town display
    internal void UpdateCityDisplay (Player player)
    {
        currentPlayer = player;
        ResetTownDisplay();
        ResetDisplayPosition();
        for (int i = 0; i < currentPlayer.ownedCities.Count && i < 4; i++){
            citySlots[i].UpdateConnectedCity(currentPlayer.ownedCities[i]);
        }
        movementButtons.UpdateButtonStatus();
    }

    internal void UpdateTownDisplay ()
    {
        ResetTownDisplay();
        for (int i = 0; i < currentPlayer.ownedCities.Count && i < 4; i++){
            citySlots[i].UpdateConnectedCity(currentPlayer.ownedCities[i]);
        }
        movementButtons.UpdateButtonStatus();
    }
}
