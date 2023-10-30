using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObject : WorldObject, IObjectInteraction
{
    private ResourceType resourceType;
    private int resourceCount;

    public void Initialize (ResourceType resourceType, int resourceCount, Vector2Int gridPosition)
    {
        Initialize(gridPosition, 0f, ObjectType.Resource);
        this.resourceType = resourceType;
        this.resourceCount = resourceCount;
    }

    public void Interact<T> (T other)
    {
        Army interactingArmy = other as Army;
        PlayerManager.Instance.GetPlayer(interactingArmy.GetPlayerTag()).AddResources(resourceType, resourceCount);
        Destroy(gameObject);
    }

    public void Interact (){
        Destroy(gameObject);
    }

    public override void ObjectSelected(){
        // TODO
    }

    protected override void OnDestroy()
    {
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
    }
}
