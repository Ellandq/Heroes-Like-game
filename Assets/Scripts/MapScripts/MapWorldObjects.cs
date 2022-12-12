using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable ,CreateAssetMenu(fileName = "MapWorldObjects", menuName = "ScriptableObjects/MapWorldObjects")]
public class MapWorldObjects : ScriptableObject
{
    [SerializeField] public int citiesCount;
    [SerializeField] public List<int> cities;
    [SerializeField] public int armiesCount;
    [SerializeField] public List<int> armies;
    [SerializeField] public int minesCount;
    [SerializeField] public List<int> mines;
    [SerializeField] public int resourcesCount;
    [SerializeField] public List<int> resources;
    [SerializeField] public int dwellingsCount;
    [SerializeField] public List<int> dwellings;
    [SerializeField] public int buildingsCount;
    [SerializeField] public List<int> buildings;
}
