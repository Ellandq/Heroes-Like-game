using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Army : WorldObject, IObjectInteraction
{
    private bool armyReady = false;
    public UnityEvent onMovementPointsChanged;
    [SerializeField] GameObject flag;

    [Header("Army information")]
    // [SerializeField] public GameObject ownedByPlayer;
    public int maxMovementPoints;
    public int movementPoints;
    public bool canBeSelectedByCurrentPlayer;

    [Header("Unit slots references")]
    public UnitsInformation unitsInformation;

    // Initialize this army
    public void Initialize(Vector2Int gridPosition, float rotation, PlayerTag ownedByPlayer, int [] _armyUnits)
    { 
        base.Initialize(gridPosition, rotation, ObjectType.Army, ownedByPlayer);
        unitsInformation.SetUnitStatus(_armyUnits);
    }
    
    // Finalizes the army object
    private void FinalizeArmy ()
    {
        // Sets the movement points 
        maxMovementPoints = Convert.ToInt16(unitsInformation.CheckMovementPoints());
        movementPoints = maxMovementPoints;

        PlayerManager.Instance.OnNextPlayerTurn.AddListener(UpdateArmySelectionAvailability);
        TurnManager.Instance.OnNewDay.AddListener(RestoreMovementPoints);
        armyReady = true;

        // if (PlayerManager.Instance.currentPlayer == ownedByPlayer.GetComponent<Player>() && GameManager.Instance.state == GameState.PlayerTurn){
        //     UIManager.Instance.UpdateCurrentArmyDisplay();
        // }
    }

    // Add an owning player
    public override void ChangeOwningPlayer(PlayerTag playerTag)
    {
        if (playerTag != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColour(playerTag);
        }
        ownedByPlayer = playerTag;
    }

    // Remove an owning player
    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
    }

    // Updates if the army is owned by the current player
    private void UpdateArmySelectionAvailability(Player _player)
    {
        if (_player.gameObject.name  == ownedByPlayer.name){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    // Updates this army grid position
    public void UpdateArmyGridPosition ()
    {
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
        gridPosition = GameGrid.Instance.GetGridPosFromWorld(this.gameObject.transform.position);
        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(this.gameObject);
    }

    // Check this army movement points based on its units
    private void CheckMovementPoints()
    {
        maxMovementPoints = Convert.ToInt16(unitsInformation.CheckMovementPoints());
    }

    // Restore this army movement points based on its units
    private void RestoreMovementPoints ()
    {
        unitsInformation.RestoreMovementPoints();
        maxMovementPoints = Convert.ToInt16(unitsInformation.CheckMovementPoints());
        movementPoints = maxMovementPoints;
        onMovementPointsChanged?.Invoke();
    }

    // Add a selected numbber of movement points
    public void AddMovementPoints(int pointsToAdd)
    {
        movementPoints += pointsToAdd;
        if (movementPoints > maxMovementPoints) movementPoints = maxMovementPoints;
        onMovementPointsChanged?.Invoke();
    }

    // Remove a selected number of movement points
    public void RemoveMovementPoints(int _pathCost)
    {
        unitsInformation.RemoveMovementPoints(_pathCost);
        movementPoints -= _pathCost;
        onMovementPointsChanged?.Invoke();
    }

    // Check if army can move to on a selected path
    public bool CanArmyMoveToPathNode (int _nextPathCost)
    {
        if (movementPoints >= _nextPathCost) return true;
        else return false;
    }

    // Army interaction with another army
    public void ArmyInteraction (GameObject interactingArmy)
    {
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            UIManager.Instance.UpdateArmyInterface(interactingArmy, this.gameObject);
        }else{
            Debug.Log("Do battle with: " + interactingArmy.name);
        }
    }

    // Interaction with this army
    public void ArmyInteraction ()
    {
        UIManager.Instance.UpdateArmyInterface(this.gameObject);
    }

    // Check if army is empty
    public bool IsArmyEmpty ()
    {
        return unitsInformation.IsArmyEmpty();
    }

    private void OnDestroy ()
    {
        PlayerManager.Instance.OnNextPlayerTurn.RemoveListener(UpdateArmySelectionAvailability);
        TurnManager.Instance.OnNewDay.RemoveListener(RestoreMovementPoints);
        onMovementPointsChanged.RemoveAllListeners();
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();

        if (ObjectSelector.Instance.lastObjectSelected != null){
            if (ObjectSelector.Instance.lastObjectSelected == transform.GetChild(0)){
                ObjectSelector.Instance.RemoveSelectedObject();
            }
        }
        
        try{
            if (CameraManager.Instance.cameraMovement.cameraFollowingObject) CameraManager.Instance.cameraMovement.CameraRemoveObjectToFollow();
        }catch (NullReferenceException){}
        
    }
}