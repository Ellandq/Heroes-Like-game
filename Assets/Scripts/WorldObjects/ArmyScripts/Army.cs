using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEditor.SceneManagement;

public class Army : WorldObject, IUnitHandler
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
    [SerializeField] private CharacterPathFindingMovementHandler movementHandle;

    #region Initialization

    public void Initialize(Vector2Int gridPosition, float rotation, PlayerTag ownedByPlayer, short [] armyUnits)
    { 
        Initialize(gridPosition, rotation, ObjectType.Army, ownedByPlayer);
        unitsInformation = new ArmyInformation(armyUnits);
        ChangeOwningPlayer(ownedByPlayer);

        maxMovementPoints = Convert.ToInt32(unitsInformation.GetMovementPoints());
        movementPoints = maxMovementPoints;

        if (PlayerManager.Instance.GetCurrentPlayerTag() == GetPlayerTag() && GameManager.Instance.state == GameState.PlayerTurn){
            UIManager.Instance.UpdateCurrentArmyDisplay();
        }
    }


    #endregion

    #region Movement-and-Selection

    public void UpdateArmySelectionAvailability()
    {
        if (PlayerManager.Instance.GetCurrentPlayerTag() == GetPlayerTag()){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    // Updates this army grid position
    public void UpdateArmyGridPosition (){
        GameGrid.Instance.GetCell(gridPosition).RemoveOccupyingObject();
        gridPosition = GameGrid.Instance.GetGridPosFromWorld(this.gameObject.transform.position);
        GameGrid.Instance.GetCell(gridPosition).AddOccupyingObject(this.gameObject);
    }

    // Check this army movement points based on its units
    private void UpdateMaxMovementPoints(){
        maxMovementPoints = Convert.ToInt32(unitsInformation.GetMovementPoints());
    }

    // Restore this army movement points based on its units
    public void RestoreMovementPoints (){
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
    public void RemoveMovementPoints(int pathCost){
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

    public override void Interact<T>(T other)
    {
        Army interactingArmy = other as Army;
        if (interactingArmy.GetPlayerTag() == GetPlayerTag()){
            UIManager.Instance.UpdateArmyInterface(interactingArmy, this);
        }else{
            
        }
    }

    public override void Interact () { UIManager.Instance.UpdateArmyInterface(this); }

    public override void ObjectSelected(){
        if (armyHighlight.IsHighlightActive()){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(this);
            Interact();
        }
        else {
            armyHighlight.EnableHighlight(true);
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(this);
        }
    }

    public override void ObjectDeselected (){
        armyHighlight.EnableHighlight(false);
        movementHandle.StopMoving();
    }

    public void Move (Vector3 pos){
        movementHandle.HandleMovement(pos);
    }

    public void Move (ObjectEnterance enterance){
        movementHandle.HandleMovement(enterance.transform.position, enterance);
    }

    public void Stop (){
        movementHandle.StopMoving();
    }

    public bool IsMoving (){
        return movementHandle.isMoving;
    }

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
        Player player = PlayerManager.Instance.GetPlayer(GetPlayerTag());
        player.AddObject(this);
        base.ChangeOwningPlayer(playerTag);
    }

    #endregion

    #region Getters

    public bool IsArmyEmpty () { return unitsInformation.IsArmyEmpty(); }

    public bool IsSelectableByCurrentPlayer () { return canBeSelectedByCurrentPlayer;}

    public int GetMovementPoints () { return movementPoints; }

    public int GetMaxMovementPoints () { return maxMovementPoints; }

    public UnitsInformation GetUnitsInformation () { return unitsInformation; }

    public Sprite GetIcon () { return unitsInformation.GetArmyIcon(); }
    
    #endregion

    #region Misc

    protected override void OnDestroy ()
    {
        onMovementPointsChanged.RemoveAllListeners();
        GameGrid.Instance.GetCell(gridPosition).RemoveOccupyingObject();

        ObjectSelector.Instance.CancelSelection(this);
        
        try{
            if (CameraManager.Instance.cameraMovement.cameraFollowingObject) CameraManager.Instance.cameraMovement.CameraRemoveObjectToFollow();
        }catch (NullReferenceException){}

        base.OnDestroy();
    }

    #endregion
}