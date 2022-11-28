using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool mineReady = false;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameGrid gameGrid;
    [SerializeField] GameObject flag;

    [Header("Mine information")]
    [SerializeField] GameObject ownedByPlayer;
    public string mineType;
    public Vector2Int gridPosition;
    private Vector3 position;
    private Vector3 rotation;

    [Header("Mine Enterance Information")]
    [SerializeField] GameObject mineEnterance;
    [SerializeField] List<PathNode> enteranceCells;

    [Header("Mine Garrison references")]
    [SerializeField] GameObject mineGarrisonSlot1;
    [SerializeField] GameObject mineGarrisonSlot2;
    [SerializeField] GameObject mineGarrisonSlot3;
    [SerializeField] GameObject mineGarrisonSlot4;
    [SerializeField] GameObject mineGarrisonSlot5;
    [SerializeField] GameObject mineGarrisonSlot6;
    [SerializeField] GameObject mineGarrisonSlot7;

    private void FinalizeMine ()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        gameGrid = GameObject.Find("GameGrid").GetComponent<GameGrid>();
        enteranceCells = new List<PathNode>();
        mineReady = true;
    }

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        if (!mineReady) FinalizeMine();
    }

    private void ChangeOwningPlayer (GameObject _ownedByPlayer)
    {
        ownedByPlayer.GetComponent<Player>().ownedMines.Remove(this.gameObject);
        if (ownedByPlayer.name == "Neutral Player"){
            ownedByPlayer = _ownedByPlayer;
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }else{
            ownedByPlayer = _ownedByPlayer;
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        ownedByPlayer.GetComponent<Player>().ownedMines.Add(this.gameObject);
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = playerManager.neutralPlayer;
        flag.SetActive(false);
    }

    public void MineInitialization (string _ownedByPLayer, string _mineType, Vector2Int _gridPosition, float _mineOrientation, int [] _mineGarrisonUnits)
    {
        mineType = _mineType;
        gridPosition = _gridPosition;
        rotation.y = _mineOrientation;
        transform.localEulerAngles = rotation;
        
        mineGarrisonSlot1.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[0], _mineGarrisonUnits[1]);
        mineGarrisonSlot2.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[2], _mineGarrisonUnits[3]);
        mineGarrisonSlot3.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[4], _mineGarrisonUnits[5]);
        mineGarrisonSlot4.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[6], _mineGarrisonUnits[7]);
        mineGarrisonSlot5.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[8], _mineGarrisonUnits[9]);
        mineGarrisonSlot6.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[10], _mineGarrisonUnits[11]);
        mineGarrisonSlot7.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[12], _mineGarrisonUnits[13]);  
    }

    public void MineInteraction (GameObject interactingArmy)
    {
        Debug.Log("Interacting army, with army: " + interactingArmy.name);
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            //do sth with friendly army
        }else{
            if (IsMineEmpty()){
                ChangeOwningPlayer(interactingArmy.GetComponent<Army>().ownedByPlayer);
            }else{
                Debug.Log("Do battle");
            }
            
        }
    }

    public void GetEnteranceInformation (List <PathNode> _enteranceList)
    {
        enteranceCells = _enteranceList;
    }

    public float GetMineRotation ()
    {
        return rotation.y;
    }

    private bool IsMineEmpty ()
    {
        if (!mineGarrisonSlot1.GetComponent<UnitSlot>().slotEmpty) return false;
        if (!mineGarrisonSlot2.GetComponent<UnitSlot>().slotEmpty) return false;
        if (!mineGarrisonSlot3.GetComponent<UnitSlot>().slotEmpty) return false;
        if (!mineGarrisonSlot4.GetComponent<UnitSlot>().slotEmpty) return false;
        if (!mineGarrisonSlot5.GetComponent<UnitSlot>().slotEmpty) return false;
        if (!mineGarrisonSlot6.GetComponent<UnitSlot>().slotEmpty) return false;
        if (!mineGarrisonSlot7.GetComponent<UnitSlot>().slotEmpty) return false;
        return true;

    }
}
