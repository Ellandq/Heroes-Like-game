using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapInformationObject", menuName = "ScriptableObjects/MapInformation")]
public class MapScriptableObject : ScriptableObject
{
    [SerializeField] public Vector2Int mapSize;
    [SerializeField] public short numberOfPlayers;
    [SerializeField] public string[] players;
    [SerializeField] public short numberOfHumanPlayers;
    [SerializeField] public string[] humanPlayers;
}
