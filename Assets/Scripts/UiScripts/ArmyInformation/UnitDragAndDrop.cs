using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private UnitButton connectedUnitButton;
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

    // When object is being dragged sets the selection object image to the same as this one
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

    // When the object is being dragged sets the selection object position to the mouse corrected position
    public void OnDrag (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    // When the objects is no longer dragged disables the selection object
    public void OnEndDrag (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            selectedObjectUnit.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = startingPosition;
        }
    }

    // When the objects is dropped disables the selection object
    public void OnDrop (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            selectedObjectUnit.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = startingPosition;
        }
    }
}
