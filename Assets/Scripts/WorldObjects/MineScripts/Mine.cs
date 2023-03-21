using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool mineReady = false;
    [SerializeField] private GameObject flag;

    [Header("Mine information")]
    [SerializeField] private GameObject ownedByPlayer;
    [SerializeField] private ResourceType mineType;
    [SerializeField] private Vector2Int gridPosition;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    [Header("Mine Enterance Information")]
    [SerializeField] private GameObject mineEnterance;
    [SerializeField] private List<PathNode> enteranceCells;

    [Header("Mine Garrison references")]
    [SerializeField] private GameObject mineGarrisonSlot1;
    [SerializeField] private GameObject mineGarrisonSlot2;
    [SerializeField] private GameObject mineGarrisonSlot3;
    [SerializeField] private GameObject mineGarrisonSlot4;
    [SerializeField] private GameObject mineGarrisonSlot5;
    [SerializeField] private GameObject mineGarrisonSlot6;
    [SerializeField] private GameObject mineGarrisonSlot7;

    #region Initialization

    public void MineInitialization (PlayerTag _ownedByPLayer, ResourceType _mineType, Vector2Int _gridPosition, float _mineOrientation, int [] _mineGarrisonUnits)
    {
        mineType = _mineType;
        gridPosition = _gridPosition;
        rotation.y = _mineOrientation;
        if (rotation.y < 0){
            rotation.y %= 360f;
            rotation.y += 360f;
        }
        transform.localEulerAngles = rotation;
        
        mineGarrisonSlot1.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[0], _mineGarrisonUnits[1]);
        mineGarrisonSlot2.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[2], _mineGarrisonUnits[3]);
        mineGarrisonSlot3.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[4], _mineGarrisonUnits[5]);
        mineGarrisonSlot4.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[6], _mineGarrisonUnits[7]);
        mineGarrisonSlot5.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[8], _mineGarrisonUnits[9]);
        mineGarrisonSlot6.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[10], _mineGarrisonUnits[11]);
        mineGarrisonSlot7.GetComponent<UnitSlot>().SetSlotStatus(_mineGarrisonUnits[12], _mineGarrisonUnits[13]);  
    }

    private void FinalizeMine ()
    {
        enteranceCells = new List<PathNode>();
        GameGrid.Instance.PlaceBuildingOnGrid(gridPosition, BuildingType.TwoByTwo, rotation.y, this.gameObject);
        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(mineEnterance);
        mineReady = true;
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

    public float GetMineRotation ()
    {
        return rotation.y;
    }

    public ResourceType GetMineType (){
        return mineType;
    }
}
