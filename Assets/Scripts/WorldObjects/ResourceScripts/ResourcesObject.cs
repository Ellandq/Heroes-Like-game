using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObject : WorldObject
{
    private ResourceType resourceType;
    private int resourceCount;

    public void Initialize (ResourceType resourceType, int resourceCount, Vector2Int gridPosition)
    {
        base.Initialize(gridPosition, 0f, ObjectType.Resource);
        this.resourceType = resourceType;
        this.resourceCount = resourceCount;
    }

    public void ResourceInteraction (Army interactingArmy)
    {
        PlayerManager.Instance.GetPlayer(interactingArmy.GetPlayerTag()).AddResources(resourceType, resourceCount);
        Destroy(this.gameObject);
    }

    protected override void OnDestroy()
    {
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
    }
}
