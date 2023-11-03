using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObject : WorldObject, IObjectInteraction
{
    private ResourceIncome resource;

    public void Initialize (ResourceType resourceType, int resourceCount, Vector2Int gridPosition)
    {
        Initialize(gridPosition, 0f, ObjectType.Resource);
        resource = new ResourceIncome(resourceCount, resourceType);
    }

    public void Interact<T> (T other)
    {
        Army interactingArmy = other as Army;
        PlayerManager.Instance.GetPlayer(interactingArmy.GetPlayerTag()).AddResources(resource);
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
        GameGrid.Instance.GetCell(gridPosition).RemoveOccupyingObject();
    }
}
