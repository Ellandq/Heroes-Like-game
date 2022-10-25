using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPathFindingMovementHandler : MonoBehaviour
{
    Coroutine moveToNextNode;
    [SerializeField] UnityEvent onMoveFinish;
    [SerializeField] VisablePath visablePath;
    [SerializeField] GameGrid gameGrid;
    [SerializeField] Army thisArmy;
    [SerializeField] GameObject objectToInteractWith;
    private List<Vector3> pathVectorList;
    private Vector3 previousSelectedPosition;
    private int currentPathIndex;
    private bool isReadyToMove;
    public bool isMoving;
    public const float movementSpeed = 0.5f;

    void Start ()
    {
        gameGrid = FindObjectOfType<GameGrid>();
        visablePath = FindObjectOfType<VisablePath>();
        isMoving = false;
        thisArmy = this.gameObject.GetComponentInParent<Army>();
        TurnManager.OnNewPlayerTurn += StopMoving;
    }

    public void HandleMovement(Vector3 _targetPosition) {
        
        if (pathVectorList != null) {
            
            if (Vector3.Distance(_targetPosition, pathVectorList[pathVectorList.Count - 1]) < 5f | Vector3.Distance(previousSelectedPosition, _targetPosition) < 5f)
            {
                isMoving = true;
                MovementStatus();
            }else{
                isReadyToMove = true;
                visablePath.DestroyVisablePath();
                previousSelectedPosition = _targetPosition;
                SetTargetPosition(_targetPosition);
                if (pathVectorList != null){
                    visablePath.CreateVisablePath(pathVectorList, CalculatePathCost(), thisArmy);
                }
            }
            
        } else{
            isReadyToMove = true;
            previousSelectedPosition = _targetPosition;
            SetTargetPosition(_targetPosition);
            if (pathVectorList != null){
                visablePath.CreateVisablePath(pathVectorList, CalculatePathCost(), thisArmy);
            } 
        }
    }

    private void MovementStatus ()
    {
        if (currentPathIndex >= pathVectorList.Count) {
            if (objectToInteractWith != null) objectToInteractWith.GetComponent<ObjectInteraction>().ObjectInteractionEvent(this.gameObject);
            StopMoving();
            isReadyToMove = false;
            objectToInteractWith = null;
            return;
        }
        if (moveToNextNode == null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            moveToNextNode = StartCoroutine(MoveToNextNode(targetPosition));
        }
    }

    public void StopMoving() {
        pathVectorList = null;
        objectToInteractWith = null;
        isMoving = false;
        visablePath.DestroyVisablePath();
    }

    public void StopMoving(Player player) {
        pathVectorList = null;
        isMoving = false;
        visablePath.DestroyVisablePath();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = gameGrid.pathfinding.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1){
            pathVectorList.RemoveAt(0);
        }

        if (gameGrid.GetGridCellInformation(gameGrid.GetGridPosFromWorld(targetPosition)).isObjectInteractable){
            objectToInteractWith = gameGrid.GetGridCellInformation(gameGrid.GetGridPosFromWorld(targetPosition)).objectInThisGridSpace;
        }
    }

    public void PrepareNextMove ()
    {
        currentPathIndex++;
        moveToNextNode = null;
        if (isMoving)
        {
            MovementStatus(); 
        }
    }

    private IEnumerator MoveToNextNode (Vector3 _targetPosition)
    {
        Vector3 _startingPosition = transform.position;
        while (transform.position != _targetPosition)
        {
            yield return null;
            _targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed);
        }
        onMoveFinish?.Invoke();
    }

    private int CalculatePathCost()
    {
        int pathCost = 0;

        if (Vector3.Distance(pathVectorList[0], transform.position) > 5f){
            pathCost += 14;
        }else{
            pathCost += 10;
        }
        for(int i = 1; i < pathVectorList.Count; i++){
            if (Vector3.Distance(pathVectorList[i - 1], pathVectorList[i]) > 5f) pathCost += 14;
            else pathCost += 10;
        }
        return pathCost;
    }
}
