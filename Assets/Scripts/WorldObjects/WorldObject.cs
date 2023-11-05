using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour, IObjectInteraction
{
    protected Vector2Int gridPosition {get; set; }
    private float rotation { get; set; }
    private ObjectType objectType { get; set; } 
    private PlayerTag ownedByPlayer { get; set; }
    
    public void Initialize (Vector2Int gridPosition, float rotation = 0f, ObjectType objectType = ObjectType.None, PlayerTag ownedByPlayer = PlayerTag.None){
        this.objectType = objectType;
        UpdateObjectPosition(GameGrid.Instance.GetWorldPosFromGridPos(gridPosition));
        UpdateObjectRotation(rotation);
        ChangeOwningPlayer(ownedByPlayer);
    }

    public void Initialize (Vector2Int gridPosition, float rotation = 0f, ObjectType objectType = ObjectType.None){
        this.objectType = objectType;
        UpdateObjectPosition(GameGrid.Instance.GetWorldPosFromGridPos(gridPosition));
        UpdateObjectRotation(rotation);
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
        if (rotation.y < 0){
            rotation.y %= 360f;
            rotation.y += 360f;
        }
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

    public virtual List<int> GetConvertedObjectInformation (){
        return new List<int>();
    }

    // Object Interactions

    public virtual void Interact (){
        Debug.Log("Interacting with object: " + this.gameObject.name);
    }

    public virtual void Interact<T> (T other){
        Debug.Log("Interacting with object: " + this.gameObject.name + " , Interaction started by: " + (other as WorldObject).gameObject.name);
    }

    public virtual void ObjectSelected (){
        Debug.Log("Object selected: " + gameObject.name);
    }

    public virtual void ObjectDeselected(){
        Debug.Log("Object deselected: " + gameObject.name);
    }

    // On Destroy

    protected virtual void OnDestroy (){
        PlayerManager.Instance.GetPlayer(GetPlayerTag()).RemoveObject(this);
        Debug.Log("Object destroyed (" + this.gameObject.name + ")");
    }
}
