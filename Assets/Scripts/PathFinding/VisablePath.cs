using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VisablePath : MonoBehaviour
{
    [SerializeField] GameObject movementPathDisplayPrefab;
    [SerializeField] Material walkableTile;
    [SerializeField] Material lockedTile;
    [SerializeField] Material blockedTile;

    private List <GameObject> pathList = new List<GameObject>();

    public void CreateVisablePath (List<Vector3> vectorPathList, int _pathCost, Army _selectedArmy)
    {
        for (int i = 0; i < vectorPathList.Count; i++)
        {
            pathList.Add(Instantiate(movementPathDisplayPrefab, vectorPathList[i], Quaternion.identity, transform.parent));
            pathList[i].transform.parent = this.gameObject.transform;
            pathList[i].gameObject.name = "Path Tile: " + (i + 1);

        }
        pathList[pathList.Count - 1].transform.localScale += Vector3.one;
    }

    public void DestroyVisablePath()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            Destroy(pathList[i]);
        }
        pathList.Clear();
    }
}
