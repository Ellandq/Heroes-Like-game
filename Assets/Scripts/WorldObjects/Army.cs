using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Army : MonoBehaviour
{
    public UnityEvent onMovementPointsChanged;
    [SerializeField] TurnManager turnManager;
    [SerializeField] GameGrid gameGrid;
    [SerializeField] GameObject flag;

    [Header("Army information")]
    [SerializeField] public GameObject ownedByPlayer;
    public Vector2Int gridPosition;
    private Vector3 position;
    private Vector3 rotation;
    public int maxMovementPoints;
    public int movementPoints;
    public bool canBeSelectedByCurrentPlayer;

    [Header("Unit slots references")]
    [SerializeField] public List <GameObject> unitSlots;

    void Start ()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        gameGrid = GameObject.Find("GameGrid").GetComponent<GameGrid>();
        maxMovementPoints = 2200;
        movementPoints = maxMovementPoints;
        PlayerManager.Instance.OnNextPlayerTurn.AddListener(UpdateArmySelectionAvailability);
        turnManager.OnNewDay.AddListener(RestoreMovementPoints);
        ownedByPlayer.GetComponent<Player>().CheckPlayerStatus();
        GetComponentInChildren<ObjectInteraction>().ChangeObjectName(this.gameObject.name);
         
        if (PlayerManager.Instance.currentPlayer == ownedByPlayer.GetComponent<Player>()){
            TownAndArmySelection.Instance.UpdateCurrentArmyDisplay(); 
        }
    }

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
    }

    public void ArmyInitialization (string _ownedByPLayer, Vector2Int _gridPosition, float _armyOrientation, int [] _armyUnits)
    {
        gridPosition = _gridPosition;
        rotation.y = _armyOrientation;
        transform.localEulerAngles = rotation;

        unitSlots[0].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[0], _armyUnits[1]);
        unitSlots[1].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[2], _armyUnits[3]);
        unitSlots[2].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[4], _armyUnits[5]);
        unitSlots[3].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[6], _armyUnits[7]);
        unitSlots[4].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[8], _armyUnits[9]);
        unitSlots[5].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[10], _armyUnits[11]);
        unitSlots[6].GetComponent<UnitSlot>().SetSlotStatus(_armyUnits[12], _armyUnits[13]);
    }

    private void UpdateArmySelectionAvailability(Player _player)
    {
        if (_player.gameObject.name  == ownedByPlayer.name){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    public void UpdateArmyGridPosition ()
    {
        gridPosition = gameGrid.GetGridPosFromWorld(this.gameObject.transform.position);
    }

    private void CheckMovementPoints()
    {

    }

    private void RestoreMovementPoints ()
    {
        movementPoints = maxMovementPoints;
        onMovementPointsChanged?.Invoke();
    }

    public void AddMovementPoints(int pointsToAdd)
    {
        movementPoints += pointsToAdd;
        if (movementPoints > maxMovementPoints) movementPoints = maxMovementPoints;
        onMovementPointsChanged?.Invoke();
    }

    public void RemoveMovementPoints(int _pathCost)
    {
        movementPoints -= _pathCost;
        onMovementPointsChanged?.Invoke();
    }

    public bool CanArmyMoveToPathNode (int _nextPathCost)
    {
        if (movementPoints >= _nextPathCost) return true;
        else return false;
    }

    public void ArmyInteraction (GameObject interactingArmy)
    {
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            ArmyInterfaceArmyInformation.Instance.GetArmyUnits(interactingArmy, this.gameObject);
        }else{
            Debug.Log("Do battle with: " + interactingArmy.name);
        }
    }

    public void ArmyInteraction ()
    {
        ArmyInterfaceArmyInformation.Instance.GetArmyUnits(this.gameObject);
    }

    public bool IsArmyEmpty ()
    {
        foreach (GameObject unit in unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    private void OnDestroy ()
    {
        PlayerManager.Instance.OnNextPlayerTurn.RemoveListener(UpdateArmySelectionAvailability);
        turnManager.OnNewDay.RemoveListener(RestoreMovementPoints);
        onMovementPointsChanged.RemoveAllListeners();
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
        
        try{
            if (CameraManager.Instance.cameraMovement.cameraFollowingObject) CameraManager.Instance.cameraMovement.CameraRemoveObjectToFollow();
        }catch (NullReferenceException){}
        
    }
}