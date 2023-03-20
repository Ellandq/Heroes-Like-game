using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPathFindingMovementHandler : MonoBehaviour
{
    private Coroutine moveToNextNode;
    [SerializeField] UnityEvent onMoveFinish;
    [SerializeField] private VisablePath visablePath;
    [SerializeField] private Army thisArmy;
    [SerializeField] private GameObject objectToInteractWith;
    private List<Vector3> pathVectorList;
    private List <int> pathCost;
    private Vector3 previousSelectedPosition;
    private int currentPathIndex;
    private bool isReadyToMove;
    public bool isMoving;
    public const float movementSpeed = 30f;

    void Start ()
    {
        visablePath = FindObjectOfType<VisablePath>();
        isMoving = false;
        thisArmy = this.gameObject.GetComponentInParent<Army>();
        TurnManager.OnNewPlayerTurn += StopMoving;
    }

    // Sets the target position and if the given position is close to the already set position starts moving
    public void HandleMovement(Vector3 _targetPosition) {
        
        if (pathVectorList != null) {
            
            if (Vector3.Distance(_targetPosition, pathVectorList[pathVectorList.Count - 1]) < 5f | Vector3.Distance(previousSelectedPosition, _targetPosition) < 5f) // Checks if the position is close to the previous one
            {
                isMoving = true;
                CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(this.gameObject);
                MovementStatus();
            }else{  // Sets a new target position
                objectToInteractWith = null;
                isReadyToMove = true;
                visablePath.DestroyVisablePath();
                previousSelectedPosition = _targetPosition;
                SetTargetPosition(_targetPosition);
                if (pathVectorList != null){
                    pathCost = new List <int>();
                    visablePath.CreateVisablePath(pathVectorList, CalculatePathCost(), thisArmy);
                }else{
                    objectToInteractWith = null;
                }
            }  
        } else{     // Sets a target position
            isReadyToMove = true;
            previousSelectedPosition = _targetPosition;
            SetTargetPosition(_targetPosition);
            if (pathVectorList != null){
                pathCost = new List <int>();
                visablePath.CreateVisablePath(pathVectorList, CalculatePathCost(), thisArmy);
            }else{
                objectToInteractWith = null;
            }
            
        }
    }

    // Checks the movement status
    private void MovementStatus ()
    {
        // Checks if the movement is done and stops moving if it is
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
            if (thisArmy.CanArmyMoveToPathNode(pathCost[currentPathIndex]))
            {
                thisArmy.RemoveMovementPoints(pathCost[currentPathIndex]);
                moveToNextNode = StartCoroutine(MoveToNextNode(targetPosition));
            }else{
                isMoving = false;
            }
        }
    }

    // Stops moving and resets the element
    public void StopMoving() {
        pathVectorList = null;
        objectToInteractWith = null;
        isMoving = false;
        visablePath.DestroyVisablePath();
    }

    // Stops moving on new turn 
    public void StopMoving(Player player) {
        pathVectorList = null;
        isMoving = false;
        visablePath.DestroyVisablePath();
    }

    // Returns the current world position of selected army
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // Sets the object position to a given position
    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = GameGrid.Instance.pathfinding.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1){
            pathVectorList.RemoveAt(0);
        }

        if (GameGrid.Instance.GetGridCellInformation(GameGrid.Instance.GetGridPosFromWorld(targetPosition)).isObjectInteractable){
            objectToInteractWith = GameGrid.Instance.GetGridCellInformation(GameGrid.Instance.GetGridPosFromWorld(targetPosition)).objectInThisGridSpace;
        }
    }

    // Prepares to move to the next PathNode
    public void PrepareNextMove ()
    {
        currentPathIndex++;
        moveToNextNode = null;
        if (isMoving)
        {
            MovementStatus(); 
        }
    }

    // Enumerator to slowly move from one PathNode to another
    private IEnumerator MoveToNextNode (Vector3 _targetPosition)
    {
        Vector3 _startingPosition = transform.position;
        while (transform.position != _targetPosition)
        {
            yield return null;
            _targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed * Time.deltaTime);
        }
        onMoveFinish?.Invoke();
    }
    
    // Calculates the path cost of the current path and returns it as a intiger list
    private List <int> CalculatePathCost()
    {      
        if (Vector3.Distance(pathVectorList[0], transform.position) > 6f){
            pathCost.Add(140);
        }else{
            pathCost.Add(100);
        }
        for(int i = 1; i < pathVectorList.Count; i++){
            if (Vector3.Distance(pathVectorList[i - 1], pathVectorList[i]) > 6f) pathCost.Add(140);
            else pathCost.Add(100);
        }
        return pathCost;
    }
}
