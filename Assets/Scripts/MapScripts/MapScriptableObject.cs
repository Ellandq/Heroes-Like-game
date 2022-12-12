using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable ,CreateAssetMenu(fileName = "MapInformationObject", menuName = "ScriptableObjects/MapInformation")]
public class MapScriptableObject : ScriptableObject
{
    [SerializeField] public Vector2Int mapSize;
    [SerializeField] public short numberOfPlayers;
    [SerializeField] public List<PlayerTag> players;
    [SerializeField] public short numberOfHumanPlayers;
    [SerializeField] public List<PlayerTag> humanPlayers;
}
