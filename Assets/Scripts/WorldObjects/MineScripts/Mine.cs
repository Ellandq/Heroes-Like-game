using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : WorldObject, IEnteranceInteraction
{
    [SerializeField] private GameObject flag;

    [Header("Mine information")]
    [SerializeField] private GameObject ownedByPlayer;
    [SerializeField] private ResourceType mineType;

    [Header("Mine Enterance Information")]
    [SerializeField] private GameObject mineEnterance;
    [SerializeField] private List<PathNode> enteranceCells;

    [Header("Mine Garrison references")]
    [SerializeField] private List<UnitSlot> mineGarrison;

    #region Initialization

    private void Awake (){
        enteranceCells = new List<PathNode>();
        mineGarrison = new List<UnitSlot>();
    }

    public void Initialize (Vector2Int gridPosition, float rotation, PlayerTag playerTag, ResourceType mineType, int [] garrisonUnits)
    {
        Initialize(gridPosition, rotation, ObjectType.Mine, playerTag);
        this.mineType = mineType;

        for (int i = 0; i < 7; i++){
            mineGarrison[i].SetSlotStatus(garrisonUnits[i * 2], garrisonUnits[i * 2 + 1]);
        }
    }

    private void FinalizeMine ()
    {
        GameGrid.Instance.PlaceBuildingOnGrid(gridPosition, BuildingType.TwoByTwo, GetRotation(), gameObject);
        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(mineEnterance);
    }

    #endregion

    #region Player Management

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        
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
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
    }

    #endregion

    #region Interaction Manager

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

    public void GetEnteranceInformation (List <PathNode> _enteranceList)
    {
        enteranceCells = _enteranceList;
    }

    #endregion

    public ResourceType GetMineType (){
        return mineType;
    }
}
