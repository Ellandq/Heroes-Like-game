using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Vector2Int currentGridPosition;
    [SerializeField] private LayerMask whatIsAGridLayer;
    [SerializeField] internal bool isCurrentGridOccupied;
    private MouseInput mouseInput;

    // Start is called before the first frame update
    void Start()
    {
        mouseInput = inputManager.GetComponent<MouseInput>();
    }

    // Update is called once per frame
    void Update()
    {
        GridCell cellMouseIsOver = IsMouseOverAGridSpace();
        if (cellMouseIsOver != null)
        {
            currentGridPosition = GameGrid.Instance.GetGridPosFromWorld(mouseInput.MouseWorldPosition(whatIsAGridLayer));
            isCurrentGridOccupied = cellMouseIsOver.isOccupied;
        }
    }

    // Returns the grid cell if mouse is over a grid cell and returns null if it is not
    private GridCell IsMouseOverAGridSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, 100f, whatIsAGridLayer))
        {
            return hitinfo.transform.GetComponent<GridCell>();
        }
        else
        {
            return null;
        }
    }
}
