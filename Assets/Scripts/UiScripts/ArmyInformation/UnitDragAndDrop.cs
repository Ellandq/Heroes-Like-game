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

    public void OnDrag (PointerEventData eventData)
    {
        if (!connectedUnitButton.isSlotEmpty){
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

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
