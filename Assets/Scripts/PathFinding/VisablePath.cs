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

    // Creates a visable path for the player
    public void CreateVisablePath (List<Vector3> vectorPathList, List <int> _pathCost, Army _selectedArmy)
    {
        int simulatedMovementPoints = _selectedArmy.movementPoints;
        for (int i = 0; i < vectorPathList.Count; i++)
        {
            pathList.Add(Instantiate(movementPathDisplayPrefab, vectorPathList[i], Quaternion.identity, transform.parent));
            pathList[i].transform.parent = this.gameObject.transform;
            pathList[i].gameObject.name = "Path Tile: " + (i + 1);
            if (simulatedMovementPoints >= _pathCost[i]) pathList[i].GetComponent<MeshRenderer>().material =  walkableTile;
            else pathList[i].GetComponent<MeshRenderer>().material =  blockedTile;
            simulatedMovementPoints -= _pathCost[i];
        }
        pathList[pathList.Count - 1].transform.localScale += Vector3.one;
    }

    // Destroys the visable path
    public void DestroyVisablePath()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            Destroy(pathList[i]);
        }
        pathList.Clear();
    }
}
