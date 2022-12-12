using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityUnitDragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private CityUnitButton connectedUnitButton;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject selectedObjectUnit;

    private Vector3 startingPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake ()
    {
        canvasGroup = selectedObjectUnit.GetComponent<CanvasGroup>();
        rectTransform = selectedObjectUnit.GetComponent<RectTransform>();
    }

    // checks if the selected slot is empty and begins dragging if it isn't
    public void OnBeginDrag (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            selectedObjectUnit.GetComponent<Image>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
            selectedObjectUnit.SetActive(true);
            rectTransform.position = transform.GetChild(0).GetComponent<RectTransform>().position;
            startingPosition = rectTransform.position;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Updates the dragged object position
    public void OnDrag (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    // When the 
    public void OnEndDrag (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            selectedObjectUnit.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = startingPosition;
        }
    }

    public void OnDrop (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            selectedObjectUnit.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = startingPosition;
        }
    }
}
