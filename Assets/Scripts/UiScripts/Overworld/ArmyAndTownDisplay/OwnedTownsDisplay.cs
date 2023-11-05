using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedTownsDisplay : MonoBehaviour
{
    [SerializeField] private TownSelectionMovementButtons movementButtons;

    [Header ("Town Buttons Information")]
    [SerializeField] private List <RectTransform> citySlotsPosition;
    [SerializeField] private List <TownButton> citySlots;

    [Header ("Town Display Positional Information")]
    [SerializeField] private RectTransform rotationObject;
    private Quaternion targetRotation;
    private Vector3 rotation;
    private float rotationSpeed;
    private int currentPosition;
    private bool manualMovementEnabled;

    // Add a flag variable to track whether the rotation is in progress
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
        rotation.z += 30f * movementValue;
        currentPosition += movementValue;
    }

    // Resets the position to the starting value
    private void ResetDisplayPosition (){
        rotation.z = 0f;
        currentPosition = 0;
        transform.localEulerAngles = rotation;
    }

    // Updates the town display
    public void UpdateCityDisplay (bool resetPosition = true)
    {
        Player player = PlayerManager.Instance.GetCurrentPlayer();
        ResetTownDisplay();
        if (resetPosition) ResetDisplayPosition();
        int i = 0;
        foreach (City city in player.GetOwnedCities()){
            citySlots[i].UpdateConnectedCity(city);
            i++;
        }
        movementButtons.UpdateButtonStatus();
    }

    // Resets the town display
    private void ResetTownDisplay (){
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

        List<City> cities = PlayerManager.Instance.GetCurrentPlayer().GetOwnedCities();

        // If the rotation is forward
        if (amount > 0)
        {
            // Calculate starting and ending slots, as well as the current City index
            startingSlot = currentPosition + 4;
            endingSlot = Mathf.Min(startingSlot + amount - 1, cities.Count - 1);
            currentCityIndex = startingSlot;

            // Iterate over the visible slots and update them with the corresponding City
            for (int i = startingSlot; i <= endingSlot; i++)
            {
                // Calculate the actual slot index (wraps around to the beginning of the array if necessary)
                int slotIndex = i % 12;

                // Update the City displayed in this slot
                citySlots[slotIndex].UpdateConnectedCity(cities[currentCityIndex]);

                // Move to the next City in the list
                currentCityIndex++;

                // If we've reached the end of the list, start over from the beginning
                if (currentCityIndex >= cities.Count)
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
                citySlots[slotIndex].UpdateConnectedCity(cities[currentCityIndex]);

                // Move to the next City in the list
                currentCityIndex--;

                // If we've reached the beginning of the list, start over from the end
                if (currentCityIndex < 0)
                {
                    currentCityIndex = cities.Count - 1;
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

    public int GetCurrentPosition () { return currentPosition; }
}
