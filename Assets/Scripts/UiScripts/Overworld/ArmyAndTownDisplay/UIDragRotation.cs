using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragRotation : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 dragStartPos;
    private bool isDragging = false;

    // Reference to the OwnedArmiesDisplay script
    [SerializeField]
    private IObjectDisplay display;

    private void Awake()
    {
        // Ensure the reference to OwnedArmiesDisplay is set in the Inspector
        if (display == null)
        {
            Debug.LogError("OwnedArmiesDisplay reference is not set.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Store the initial mouse position when the user presses the UI element
        dragStartPos = eventData.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Calculate the vertical drag difference
            float yDelta = eventData.position.y - dragStartPos.y;

            // Send the Y position difference to the OwnedArmiesDisplay script for manual rotation
            display.ManualRotation(yDelta);

            // Update the drag start position for continuous drag tracking
            dragStartPos = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        ResetUIPosition();
    }

    private void ResetUIPosition(){
        display.StopManualMovement();
    }
}
