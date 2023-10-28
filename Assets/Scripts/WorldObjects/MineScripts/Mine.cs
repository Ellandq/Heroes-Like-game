using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : WorldObject, IEnteranceInteraction
{
    [SerializeField] private GameObject flag;

    [Header("Mine information")]
    [SerializeField] private List<ResourceType> mineType;
    private ResourceIncome mineIncome;

    [Header("Mine Enterance Information")]
    [SerializeField] private GameObject mineEnterance;
    private List<PathNode> enteranceCells;

    [Header("Mine Garrison references")]
    private UnitsInformation unitsInformation;

    #region Initialization

    private void Awake (){
        enteranceCells = new List<PathNode>();
        mineIncome = new ResourceIncome(new int[]{
            1000, 2, 2, 1, 1, 1, 1
        });
        
    }

    public void Initialize (Vector2Int gridPosition, float rotation, PlayerTag playerTag, ResourceType mineType, int [] garrisonUnits)
    {
        Initialize(gridPosition, rotation, ObjectType.Mine, playerTag);
        unitsInformation = new UnitsInformation(garrisonUnits);
        this.mineType = mineType;
    }

    public override void FinalizeObject()
    {
        GameGrid.Instance.PlaceBuildingOnGrid(gridPosition, BuildingType.TwoByTwo, GetRotation(), gameObject);
        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(mineEnterance);
    }

    #endregion

    #region Player Management

    public void AddOwningPlayer(PlayerTag ownedByPlayer)
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

    private bool IsMineEmpty (){
        return unitsInformation.IsArmyEmpty();
    }

    public void SetEnteranceInformation (List <PathNode> enteranceList)
    {
        enteranceCells = enteranceList;
    }

    #endregion

    public ResourceIncome GetIncome (){
        return new ResourceIncome(mineIncome, mineType);
    }
}
