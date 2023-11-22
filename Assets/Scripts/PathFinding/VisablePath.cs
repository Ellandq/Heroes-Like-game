using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VisablePath : MonoBehaviour
{
    public static VisablePath Instance;

    [Header ("Prefabs")]
    [SerializeField] private GameObject movementPathDisplayPrefab;

    [Header ("Tile Materials")]
    [SerializeField] private Material walkableTile;
    [SerializeField] private Material lockedTile;
    [SerializeField] private Material blockedTile;

    [Header ("Path Objects")]
    private List <GameObject> pathList = new List<GameObject>();

    private void Awake (){
        Instance = this;
    }

    // Creates a visable path for the player
    public void CreateVisiblePath (List<Vector3> vectorPathList, List <int> _pathCost, Army selectedArmy)
    {
        int simulatedMovementPoints = selectedArmy.GetMovementPoints();
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
    public void DestroyVisiblePath()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            Destroy(pathList[i]);
        }
        pathList.Clear();
    }
}
