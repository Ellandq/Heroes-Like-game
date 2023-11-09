using UnityEngine;
using UnityEngine.UI;

public class ObjectSelectionManualMovementHandler : MonoBehaviour
{
    [SerializeField] private Vector2 originPos;

    private Vector2 dragStartPos;
    private bool isDragging = false;

    // [SerializeField] private IObjectDisplay display;

    private void Start()
    {
        originPos = CameraManager.Instance.GetUiElementPosition(transform.position);
    
        // Ensure the reference to OwnedArmiesDisplay is set in the Inspector
        // if (display == null)
        // {
        //     Debug.LogError("OwnedArmiesDisplay reference is not set.");
        // }
    }

    private void ResetUIPosition(){
        // display.StopManualMovement();
    }


    private void Update (){
        float distance = Vector2.Distance(originPos, InputManager.Instance.mouseInput.GetMousePosition2D());

    }
}
