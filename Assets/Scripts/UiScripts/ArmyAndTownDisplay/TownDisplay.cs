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

    private void Start ()
    {
        UIManager.Instance.UIManagerReady();
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

    // Rotates the object if the 
    private void Update ()
    {
        if (!manualMovementEnabled){
            Quaternion currentRotation = rotationObject.localRotation;
            Quaternion desiredRotation = Quaternion.Euler(rotation);

            float distance = Quaternion.Angle(currentRotation, desiredRotation);

            if (distance > 0.1f)
            {
                targetRotation = Quaternion.Lerp(targetRotation, desiredRotation, Time.deltaTime * rotationSpeed);
                rotationObject.localRotation = targetRotation;
            }
            else if (distance > 0f)
            {
                float maxAngle = Mathf.Min(Time.deltaTime * rotationSpeed, distance);
                targetRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, maxAngle);
                rotationObject.localRotation = targetRotation;
            }  
        }
    }

    // Resets the town display
    private void ResetTownDisplay ()
    {
        for (int i = 0; i < 12; i++){
            citySlots[i].ResetCityButton();
        }
    }

    // Updates the slots that will be visable when the object rotation updates
    private void UpdateNewSlot (int amount){
        int startingSlot;
        int endingSlot;
        int currentArmy;
        if (amount > 0){
            startingSlot = (currentPosition + 4);
            endingSlot = startingSlot + amount - 1;
            currentArmy = startingSlot;

            if (startingSlot > 11)startingSlot %= 12;
            if (endingSlot > 11) endingSlot %= 12;
            
            for (int i = startingSlot; (i >= startingSlot || i <= endingSlot) && !(i > endingSlot && i < startingSlot); i++){
                citySlots[i].UpdateConnectedCity(currentPlayer.ownedCities[currentArmy]);
                if (i == 11){
                    if (endingSlot == i) break;
                    i = 0;
                }
                currentArmy++;
            }
        }else{
            startingSlot = (currentPosition - 1);
            endingSlot = startingSlot + amount + 1;
            currentArmy = startingSlot;

            if (startingSlot > 11)startingSlot %= 12;
            if (endingSlot > 11) endingSlot %= 12;
            
            for (int i = startingSlot; (i <= startingSlot || i >= endingSlot) && !(i < endingSlot && i > startingSlot); i--){
                citySlots[i].UpdateConnectedCity(currentPlayer.ownedCities[currentArmy]);
                if (i == 0){
                    if (endingSlot == i) break;
                    i = 11;
                }
                currentArmy--;
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
}
