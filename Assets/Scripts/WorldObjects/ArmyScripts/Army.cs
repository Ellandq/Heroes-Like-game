using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEditor.SceneManagement;

public class Army : WorldObject, IObjectInteraction
{
    [Header ("Events")]
    public UnityEvent onMovementPointsChanged;

    [Header("Army information")]
    [SerializeField] private ArmyHighlight armyHighlight;
    private ArmyInformation unitsInformation;
    private int maxMovementPoints;
    private int movementPoints;
    private bool canBeSelectedByCurrentPlayer;

    [Header ("Misc.")]
    [SerializeField] private GameObject flag;

    #region Initialization

    public void Initialize(Vector2Int gridPosition, float rotation, PlayerTag ownedByPlayer, short [] armyUnits)
    { 
        Initialize(gridPosition, rotation, ObjectType.Army, ownedByPlayer);
        unitsInformation = new ArmyInformation(armyUnits);
    }
    
    public override void FinalizeObject ()
    {
        maxMovementPoints = Convert.ToInt32(unitsInformation.GetMovementPoints());
        movementPoints = maxMovementPoints;

        PlayerManager.Instance.OnNextPlayerTurn.AddListener(UpdateArmySelectionAvailability);
        TurnManager.Instance.OnNewDay.AddListener(RestoreMovementPoints);

        if (PlayerManager.Instance.GetCurrentPlayer() == PlayerManager.Instance.GetPlayer(GetPlayerTag()) && GameManager.Instance.state == GameState.PlayerTurn){
            UIManager.Instance.UpdateCurrentArmyDisplay();
        }
    }

    #endregion

    #region Movement-and-Selection

    private void UpdateArmySelectionAvailability(PlayerTag tag)
    {
        if (tag == GetPlayerTag()){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    // Updates this army grid position
    public void UpdateArmyGridPosition (){
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
        gridPosition = GameGrid.Instance.GetGridPosFromWorld(this.gameObject.transform.position);
        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(this.gameObject);
    }

    // Check this army movement points based on its units
    private void UpdateMaxMovementPoints(){
        maxMovementPoints = Convert.ToInt32(unitsInformation.GetMovementPoints());
    }

    // Restore this army movement points based on its units
    private void RestoreMovementPoints (){
        unitsInformation.RestoreMovementPoints();
        maxMovementPoints = Convert.ToInt32(unitsInformation.GetMovementPoints());
        movementPoints = maxMovementPoints;
        onMovementPointsChanged?.Invoke();
    }

    // Add a selected numbber of movement points
    public void AddMovementPoints(int pointsToAdd){
        movementPoints += pointsToAdd;
        if (movementPoints > maxMovementPoints) movementPoints = maxMovementPoints;
        onMovementPointsChanged?.Invoke();
    }

    // Remove a selected number of movement points
    public void RemoveMovementPoints(short pathCost){
        unitsInformation.RemoveMovementPoints(pathCost);
        movementPoints -= pathCost;
        onMovementPointsChanged?.Invoke();
    }

    // Check if army can move to on a selected path
    public bool CanArmyMoveToPathNode (int _nextPathCost){
        if (movementPoints >= _nextPathCost) return true;
        else return false;
    }

    #endregion

    #region Interactions

    // Army interaction with another army
    public void Interact<T>(T other)
    {
        Army interactingArmy = other as Army;
        if (interactingArmy.GetPlayerTag() == GetPlayerTag()){
            UIManager.Instance.UpdateArmyInterface(interactingArmy, this);
        }else{
            
        }
    }

    // Interaction with this army
    public void Interact () { UIManager.Instance.UpdateArmyInterface(this.gameObject); }

    #endregion

    #region Setters

    public override void ChangeOwningPlayer(PlayerTag playerTag)
    {
        if (playerTag != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColour(playerTag);
        }else{
            flag.SetActive(false);
        }
        base.ChangeOwningPlayer(playerTag);
    }

    #endregion

    #region Getters

    public bool IsArmyEmpty () { return unitsInformation.IsArmyEmpty(); }

    public bool IsSelectableByCurrentPlayer () { return canBeSelectedByCurrentPlayer;}

    public int GetMovementPoints () { return movementPoints; }

    public int GetMaxMovementPoints () { return maxMovementPoints; }

    public override PlayerTag GetPlayerTag () { return base.GetPlayerTag(); }

    public UnitsInformation GetUnitsInformation () { return unitsInformation; }
    
    #endregion

    #region Misc

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

    #endregion
}