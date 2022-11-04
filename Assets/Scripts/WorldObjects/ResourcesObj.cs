using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesObj : MonoBehaviour
{
    private string resourceType;
    private int count;
    private Vector2Int gridPosition;

    public void ResourceInitialization (string _resourceType, int _count, Vector2Int _gridPosition)
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
        GameManager.Instance.gameGrid.GetGridCellInformation(gridPosition).RemoveOccupyingObject();
    }
}
