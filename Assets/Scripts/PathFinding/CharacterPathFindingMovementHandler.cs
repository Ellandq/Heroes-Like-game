using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPathFindingMovementHandler : MonoBehaviour
{
    [Header("Coroutines")]
    private Coroutine moveToNextNode;

    [Header("Events")]
    [SerializeField] private UnityEvent onMoveFinish;

    [Header("Object References")]
    [SerializeField] private Army thisArmy;

    [Header("Current Path Information")]
    private ObjectEnterance entrance;
    private WorldObject objectToInteractWith;
    private List<Vector3> pathVectorList;
    private List<int> pathCost;

    [Header("Movement Information")]
    private Vector3 previousSelectedPosition;
    private int currentPathIndex;
    private bool isMoving { get; set; }
    private const float movementSpeed = 30f;

    private void Start()
    {
        isMoving = false;
        thisArmy = gameObject.GetComponentInParent<Army>();
        TurnManager.Instance.OnNewTurn += StopMoving;
    }

    public void HandleMovement(Vector3 targetPosition, ObjectEnterance entrance = null)
    {
        this.entrance = entrance;

        if (pathVectorList != null && (Vector3.Distance(targetPosition, pathVectorList[pathVectorList.Count - 1]) < 5f || Vector3.Distance(previousSelectedPosition, targetPosition) < 5f))
        {
            isMoving = true;
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(thisArmy);
            MovementStatus();
        }
        else
        {
            objectToInteractWith = null;
            VisablePath.Instance.DestroyVisiblePath();
            previousSelectedPosition = targetPosition;
            SetTargetPosition(targetPosition);
            if (pathVectorList != null)
            {
                pathCost = new List<int>();
                VisablePath.Instance.CreateVisiblePath(pathVectorList, CalculatePathCost(), thisArmy);
            }
            else
            {
                objectToInteractWith = null;
            }
        }
    }

    private void MovementStatus()
    {
        if (currentPathIndex >= pathVectorList.Count)
        {
            if (objectToInteractWith != null) objectToInteractWith.Interact(thisArmy);
            StopMoving();
            objectToInteractWith = null;
            return;
        }

        if (moveToNextNode == null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];

            if (thisArmy.CanArmyMoveToPathNode(pathCost[currentPathIndex]))
            {
                thisArmy.RemoveMovementPoints(pathCost[currentPathIndex]);
                moveToNextNode = StartCoroutine(MoveToNextNode(targetPosition));
            }
            else
            {
                isMoving = false;
            }
        }
    }

    public void StopMoving()
    {
        pathVectorList = null;
        objectToInteractWith = null;
        isMoving = false;
        VisablePath.Instance.DestroyVisiblePath();
    }

    public void StopMoving(Player player)
    {
        pathVectorList = null;
        isMoving = false;
        VisablePath.Instance.DestroyVisiblePath();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        Vector3 currentPos = GameGrid.Instance.GetWorldPosFromGridPos(thisArmy.GetGridPosition());
        currentPathIndex = 0;

        if (entrance == null)
        {
            pathVectorList = GameGrid.Instance.pathfinding.FindPath(currentPos, targetPosition);
        }
        else
        {
            pathVectorList = GameGrid.Instance.pathfinding.FindPath(currentPos, targetPosition, entrance);
        }

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }

        GridCell cell = GameGrid.Instance.GetCell(GameGrid.Instance.GetGridPosFromWorld(targetPosition));

        if (cell.isObjectInteractable)
        {
            objectToInteractWith = cell.objectInThisGridSpace.GetComponent<WorldObject>();
        }
    }

    public void PrepareNextMove()
    {
        currentPathIndex++;
        moveToNextNode = null;

        if (isMoving)
        {
            MovementStatus();
        }
    }

    private IEnumerator MoveToNextNode(Vector3 targetPosition)
    {
        thisArmy.SetGridPosition(GameGrid.Instance.GetGridPosFromWorld(targetPosition));

        while (transform.position != targetPosition)
        {
            yield return null;
            targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }

        onMoveFinish?.Invoke();
    }

    private List<int> CalculatePathCost()
    {
        if (Vector3.Distance(pathVectorList[0], transform.position) > 6f)
        {
            pathCost.Add(140);
        }
        else
        {
            pathCost.Add(100);
        }

        for (int i = 1; i < pathVectorList.Count; i++)
        {
            if (Vector3.Distance(pathVectorList[i - 1], pathVectorList[i]) > 6f) pathCost.Add(140);
            else pathCost.Add(100);
        }

        return pathCost;
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }
}
