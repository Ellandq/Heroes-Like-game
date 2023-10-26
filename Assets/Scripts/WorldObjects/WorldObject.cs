using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private Vector2Int gridPosition {get; set; }
    private float rotation { get; set; }
    private ObjectType objectType { get; set; } 
    private PlayerTag ownedByPlayer { get; set; }
    
    public void Initialize (Vector2Int gridPosition, float rotation, ObjectType objectType, PlayerTag ownedByPlayer = PlayerTag.None){
        this.objectType = objectType;
        UpdateObjectPosition(GameGrid.Instance.GetWorldPosFromGridPos(gridPosition));
        UpdateObjectRotation(rotation);
        ChangeOwningPlayer(ownedByPlayer);
    }

    public virtual void ChangeOwningPlayer (PlayerTag ownedByPlayer = PlayerTag.None){
        this.ownedByPlayer = ownedByPlayer;
    }

    public virtual void UpdateGridPosition (Vector2Int gridPosition){
        this.gridPosition = gridPosition;
    }

    public virtual void UpdateObjectPosition (Vector3 position){
        transform.position = position;
        UpdateGridPosition(GameGrid.Instance.GetGridPosFromWorld(position));
    }

    public virtual void UpdateObjectRotation (Vector3 rotation){
        transform.localEulerAngles = rotation;
    }

    public virtual void UpdateObjectRotation (float rotation){
        transform.localEulerAngles = new Vector3 (0f, rotation, 0f);
    }

    // Getters
    public Vector2Int GetGridPosition () { return gridPosition; }

    public float GetRotation () { return rotation; }

    public ObjectType GetObjectType () { return objectType; }

    public virtual PlayerTag GetPlayerTag () { return ownedByPlayer; }

    public virtual List<UnitSlot> GetUnitSlots (){
        Debug.LogError("Invalid request of: List<UnitSlot> from: " + this.gameObject.name);
        return null;
    }

    public virtual List<int> GetConvertedObjectInformation (){
        return new List<int>();
    }

    // Object Interactions

    public virtual void InteractWithObject (){
        Debug.Log("Interacting with object: " + this.gameObject.name);
    }

    public virtual void InteractWithObject (WorldObject other){
        Debug.Log("Interacting with object: " + this.gameObject.name + " , Interaction started by: " + other.gameObject.name);
    }

    // On Destroy

    protected virtual void OnDestroy (){
        Debug.Log("Object destroyed (" + this.gameObject.name + ")");
    }
}
