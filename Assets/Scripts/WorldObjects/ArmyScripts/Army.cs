using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEditor.SceneManagement;

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

    #region Initialization

    public void Initialize(Vector2Int gridPosition, float rotation, PlayerTag ownedByPlayer, int [] _armyUnits)
    { 
        base.Initialize(gridPosition, rotation, ObjectType.Army, ownedByPlayer);
        unitsInformation.SetUnitStatus(_armyUnits);
    }
    
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

    #endregion

    public override void ChangeOwningPlayer(PlayerTag playerTag)
    {
        if (playerTag != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColour(playerTag);
        }else{
            flag.SetActive(false);
        }
        ownedByPlayer = playerTag;
    }

    // Updates if the army is owned by the current player
    private void UpdateArmySelectionAvailability(Player player)
    {
        if (player.GetPlayerTag() == ownedByPlayer){
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
    private void UpdateMaxMovementPoints()
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
    public void Interact<T>(T other)
    {
        Army interactingArmy = other as Army;
        if (interactingArmy.GetPlayerTag() == GetPlayerTag()){
            UIManager.Instance.UpdateArmyInterface(interactingArmy, this.gameObject);
        }else{
            // TODO
        }
    }

    // Interaction with this army
    public void Interact () { UIManager.Instance.UpdateArmyInterface(this.gameObject); }

    protected override void OnDestroy ()
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

    #region Getters

    public bool IsArmyEmpty ()
    {
        return unitsInformation.IsArmyEmpty();
    }

    public override PlayerTag GetPlayerTag () { return base.GetPlayerTag(); }
    #endregion
}