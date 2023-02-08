using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable ,CreateAssetMenu(fileName = "SaveFileScriptableObject", menuName = "ScriptableObjects/SaveFileInformation")]
public class SaveFileScriptableObject : ScriptableObject
{
    [Header ("Player information")]
    [SerializeField] public List<PlayerTag> humanPlayers;
    [SerializeField] public List<PlayerState> playerStateList;

    [Header ("Turn information")]
    [SerializeField] public short turnCount;
}
