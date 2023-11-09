using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OwnedObjectDisplay : MonoBehaviour
{
    [Header ("Buttons Information")]
    [SerializeField] private ObjectSelectionButtonHandler buttonHandler;
    [SerializeField] private List<RectTransform> objectSlotPositions;
    [SerializeField] private List<ObjectButton> objectButtons;

    [Header ("Position Information")]
    [SerializeField] private RectTransform rotationObject;
    [SerializeField] private bool isArmyDisplay;
    private Quaternion targetRotation;
    private Vector3 rotation;
    private float rotationSpeed = 5f;
    private int currentPosition;
    private bool manualMovementEnabled = false;
    private bool rotationInProgress = false;

    [Header ("Display status")]
    private RectTransform rect;
    private Vector2 targetPosition;
    private Vector2 pinnedPosition;
    private Vector2 hiddenPosition;
    private bool isPinned = true;
    private Coroutine movementChecker;

    private void Start (){
        rect = GetComponent<RectTransform>();
        pinnedPosition = rect.anchoredPosition;
        hiddenPosition = isArmyDisplay ? pinnedPosition - new Vector2(82f, 0f) : pinnedPosition + new Vector2(82f, 0f);
        targetPosition = pinnedPosition;
    } 

    private void Update (){
        if (!manualMovementEnabled){
            Quaternion currentRotation = rotationObject.localRotation;
            Quaternion desiredRotation = Quaternion.Euler(rotation);

            if (!rotationInProgress) {
                targetRotation = currentRotation;
            }

            float distance = Quaternion.Angle(currentRotation, desiredRotation);

            if (distance > 0.2f)
            {
                // Set rotationInProgress to true when the rotation starts
                rotationInProgress = true;

                targetRotation = Quaternion.Lerp(targetRotation, desiredRotation, Time.deltaTime * rotationSpeed);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < objectSlotPositions.Count; i++){
                    objectSlotPositions[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }
            else if (distance > 0f)
            {
                // Set rotationInProgress to true when the rotation starts
                rotationInProgress = true;

                float maxAngle = Mathf.Min(Time.deltaTime * rotationSpeed, distance);
                targetRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, maxAngle);
                rotationObject.localRotation = targetRotation;

                for (int i = 0; i < objectSlotPositions.Count; i++){
                    objectSlotPositions[i].localRotation = Quaternion.Inverse(targetRotation);
                }
            }
            else {
                // Set rotationInProgress to false when the rotation is completed
                rotationInProgress = false;
            }
        }

        Vector2 currentPos = rect.anchoredPosition;
        if (currentPos != targetPosition) {
            float lerpSpeed = 7.5f;
            currentPos = Vector2.Lerp(currentPos, targetPosition, lerpSpeed * Time.deltaTime);
            rect.anchoredPosition = currentPos;
        }

    }

    public void ManualRotation (float delta){
        if (!manualMovementEnabled) manualMovementEnabled = true;
    }

    public void StopManualMovement (){

    }

    // Updates the rotation to the new position
    public void RotateDisplay (int movementValue){
        UpdateNewSlot(movementValue);
        rotation.z += 30f * movementValue;
        currentPosition += movementValue;
        buttonHandler.UpdateButtonStatus();
    }

    // Resets the position to the starting value
    private void ResetDisplayPosition ()
    {
        rotation.z = 0f;
        currentPosition = 0;
        transform.localEulerAngles = rotation;

        for (int i = 0; i < objectSlotPositions.Count; i++){
            objectSlotPositions[i].localRotation = Quaternion.identity;
        }
    }

    // Updates the army display
    public void UpdateDisplay (bool resetPosition = false)
    {
        Player player = PlayerManager.Instance.GetCurrentPlayer();
        int i = 0;
        ResetDisplay();
        if (resetPosition) ResetDisplayPosition();

        if (isArmyDisplay){
            foreach (Army army in player.GetOwnedArmies()){
                objectButtons[i].UpdateConnectedObject(army);
                i++;
            }
        }else{
            foreach (City city in player.GetOwnedCities()){
                objectButtons[i].UpdateConnectedObject(city);
                i++;
            }
        }
        buttonHandler.UpdateButtonStatus();
    }

    // Resets the army display
    private void ResetDisplay ()
    {
        for (int i = 0; i < 12; i++){
            objectButtons[i].ResetButton();
        }
    }

    private void UpdateNewSlot(int amount)
    {
        int startingSlot;
        int endingSlot;
        int currentArmyIndex;

        List<IUnitHandler> objects;

        if (isArmyDisplay) {
            objects = new List<IUnitHandler>(PlayerManager.Instance.GetCurrentPlayer().GetOwnedArmies());
        } else {
            objects = new List<IUnitHandler>(PlayerManager.Instance.GetCurrentPlayer().GetOwnedCities());
        }

        if (amount > 0)
        {
            startingSlot = currentPosition + 4;
            endingSlot = Mathf.Min(startingSlot + amount - 1, objects.Count - 1);
            currentArmyIndex = startingSlot;

            for (int i = startingSlot; i <= endingSlot; i++)
            {
                int slotIndex = i % 12;
                objectButtons[slotIndex].UpdateConnectedObject(objects[currentArmyIndex]);
                currentArmyIndex++;
                if (currentArmyIndex >= objects.Count){
                    currentArmyIndex = 0;
                }
            }

            for (int i = endingSlot + 1; i < startingSlot + 4; i++)
            {
                int slotIndex = i % 12;
                objectButtons[slotIndex].ResetButton();
            }
        }
        else
        {
            startingSlot = currentPosition - 1;
            endingSlot = Mathf.Max(startingSlot + amount + 1, -1);
            currentArmyIndex = startingSlot;

            for (int i = startingSlot; i >= endingSlot; i--)
            {
                int slotIndex = i % 12;
                objectButtons[slotIndex].UpdateConnectedObject(objects[currentArmyIndex]);
                currentArmyIndex--;
                if (currentArmyIndex < 0){
                    currentArmyIndex = objects.Count - 1;
                }
            }

            for (int i = endingSlot - 1; i > startingSlot - 4; i--)
            {
                int slotIndex = ((i % 12) + 12) % 12;
                objectButtons[slotIndex].ResetButton();
            }
        }
    }

    private void ChangedHiddenState(bool status){
        buttonHandler.ChangedHiddenStatus(status);
    }

    public int GetCurrentPosition () { return currentPosition; }

    private float GetMouseDistance (){ return Vector2.Distance(CameraManager.Instance.GetUiElementPosition(transform.position), InputManager.Instance.mouseInput.GetMousePosition2D()); }

    public bool IsArmyDisplay () { return isArmyDisplay; }

    public void ChangePinStatus (){
        isPinned = !isPinned;

        if (isPinned && movementChecker != null){
            StopCoroutine(movementChecker);
        }else {
            movementChecker = StartCoroutine(MovementChecker());
        }
    }

    private IEnumerator MovementChecker ()
    {
        bool status = GetMouseDistance() > 450f;
        targetPosition = status ? hiddenPosition : pinnedPosition;

        while (true)
        {
            bool currentStatus = GetMouseDistance() > 450f;

            if (currentStatus != status)
            {
                targetPosition = currentStatus ? hiddenPosition : pinnedPosition;
                status = currentStatus;
                
                ChangedHiddenState(status);
            }

            yield return null;
        }
    }
}
