using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmyInterfaceUnitDragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private ArmyInterfaceUnitButton connectedUnitButton; // The ArmyInterfaceUnitButton object connected to this object
    [SerializeField] private Canvas canvas; // The canvas object for the UI
    [SerializeField] private GameObject selectedObjectUnit; // The game object to display when this object is being dragged

    private Vector3 startingPosition; // The starting position of the object being dragged
    private RectTransform rectTransform; // The RectTransform component of the selectedObjectUnit game object
    private CanvasGroup canvasGroup; // The CanvasGroup component of the selectedObjectUnit game object

    private void Awake ()
    {
        // Get the CanvasGroup and RectTransform components of the selectedObjectUnit game object
        canvasGroup = selectedObjectUnit.GetComponent<CanvasGroup>();
        rectTransform = selectedObjectUnit.GetComponent<RectTransform>();
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        // Only allow dragging if the connectedUnitButton is not empty
        if (!connectedUnitButton.isSlotEmpty){
            // Set the sprite of the selectedObjectUnit to the sprite of this object
            selectedObjectUnit.GetComponent<Image>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
            // Make the selectedObjectUnit game object visible
            selectedObjectUnit.SetActive(true);
            // Set the position of the selectedObjectUnit game object to the position of this object
            rectTransform.position = transform.GetChild(0).GetComponent<RectTransform>().position;
            // Set the starting position of the object being dragged to the current position of the selectedObjectUnit game object
            startingPosition = rectTransform.position;
            // Disable raycasting for the canvasGroup to allow the object to be dragged over other UI elements
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag (PointerEventData eventData)
    {
        // Only allow dragging if the connectedUnitButton is not empty
        if (!connectedUnitButton.isSlotEmpty){
            // Update the position of the selectedObjectUnit game object to follow the mouse cursor
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        // Only allow dragging if the connectedUnitButton is not empty
        if (!connectedUnitButton.isSlotEmpty){
            // Hide the selectedObjectUnit game object
            selectedObjectUnit.SetActive(false);
            // Re-enable raycasting for the canvasGroup
            canvasGroup.blocksRaycasts = true;
            // Set the position of the selectedObjectUnit game object to the starting position
            rectTransform.anchoredPosition = startingPosition;
        }
    }

    public void OnDrop (PointerEventData eventData)
    {
        // Only allow dropping if the connectedUnitButton is not empty
        if (!connectedUnitButton.isSlotEmpty){
            // Hide the selectedObjectUnit game object
            selectedObjectUnit.SetActive(false);
            // Re-enable raycasting for the canvasGroup
            canvasGroup.blocksRaycasts = true;
            // Set the position of the selectedObjectUnit game object to the starting position
            rectTransform.anchoredPosition = startingPosition;
        }
    }
}
