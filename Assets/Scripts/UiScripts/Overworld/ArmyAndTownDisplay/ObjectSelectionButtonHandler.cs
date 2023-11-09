using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelectionButtonHandler : MonoBehaviour
{
    [SerializeField] private OwnedObjectDisplay objectDisplay;

    [Header ("Button References")]
    [SerializeField] private Button fastBackwardsButton;
    [SerializeField] private Button backwardsButton;
    [SerializeField] private Button forwardsButton;
    [SerializeField] private Button fastForwardsButton;

    [Header ("Position Information")]
    private RectTransform rect;
    private Vector2 targetPosition;
    private Vector2 pinnedPosition;
    private Vector2 hiddenPosition;
    private bool isPinned = true;
    private Coroutine displayMovement;

    private void Start (){
        rect = GetComponent<RectTransform>();
        pinnedPosition = rect.anchoredPosition;
        hiddenPosition = pinnedPosition;
        if (objectDisplay.IsArmyDisplay()){
            hiddenPosition += new Vector2(-125f, -25);
        }else{
            hiddenPosition += new Vector2(125f, -25);
        }
    }
    
    public void MoveDisplay (int moveAmount){
        objectDisplay.RotateDisplay(moveAmount);
    }

    public void UpdateButtonStatus()
    {
        int minPosition = 0;
        int pos = objectDisplay.GetCurrentPosition();
        int maxPosition = PlayerManager.Instance.GetCurrentPlayer().GetOwnedArmies().Count - 3;
        if (maxPosition < 0) maxPosition = 0;
        
        if (pos == minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = false;
        }else if ((pos - 2) <= minPosition){
            fastBackwardsButton.interactable = false;
            backwardsButton.interactable = true;
        }else if ((pos - 2) > 0){
            fastBackwardsButton.interactable = true;
            backwardsButton.interactable = true;
        }

        if (pos == maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = false;
        }else if ((pos + 2) >= maxPosition){
            fastForwardsButton.interactable = false;
            forwardsButton.interactable = true;
        }else if (pos < (maxPosition - 2)){
            fastForwardsButton.interactable = true;
            forwardsButton.interactable = true;
        }
    }

    public void ChangedHiddenStatus (bool status){
        if (displayMovement != null) displayMovement = null;
        if (status){
            targetPosition = hiddenPosition;
        }else{
            targetPosition = pinnedPosition;
        }
        displayMovement = StartCoroutine(MoveButtons());
    }

    private IEnumerator MoveButtons (){
        Vector2 currentPos = rect.anchoredPosition;
        while (Vector2.Distance(targetPosition, currentPos) > 2f){
            float lerpSpeed = 7.5f;
            currentPos = Vector2.Lerp(currentPos, targetPosition, lerpSpeed * Time.deltaTime);
            rect.anchoredPosition = currentPos;
            yield return null;
        }
        rect.anchoredPosition = targetPosition;
    }
}
