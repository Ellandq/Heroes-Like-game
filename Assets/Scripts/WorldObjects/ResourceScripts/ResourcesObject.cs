using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObject : MonoBehaviour
{
    private ResourceType resourceType;
    private int count;
    private Vector2Int gridPosition;

    public void ResourceInitialization (ResourceType _resourceType, int _count, Vector2Int _gridPosition)
    {
        count = _count;
        resourceType = _resourceType;
        gridPosition = _gridPosition;
    }

    public void ResourceInteraction (GameObject interactingArmy)
    {
        interactingArmy.GetComponent<Army>().ownedByPlayer.GetComponent<Player>().AddResources(resourceType, count);
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        GameGrid.Instance.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
    }
}
