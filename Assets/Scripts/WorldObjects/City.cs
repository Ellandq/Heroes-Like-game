using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameGrid gameGrid;

    [Header("Main city information")]
    [SerializeField] GameObject ownedByPlayer;
    public string cityFraction;
    public Vector2Int gridPosition;
    private Vector3 position;
    public Vector3 rotation;
    public int cityGoldProduction;
    public bool canBeSelectedByCurrentPlayer;

    [Header("City Enterance Information")]
    [SerializeField] GameObject cityEnterance;
    [SerializeField] List<PathNode> enteranceCells;

    [Header("Garrison refrences")]
    [SerializeField] GameObject garrisonSlot1;
    [SerializeField] GameObject garrisonSlot2;
    [SerializeField] GameObject garrisonSlot3;
    [SerializeField] GameObject garrisonSlot4;
    [SerializeField] GameObject garrisonSlot5;
    [SerializeField] GameObject garrisonSlot6;
    [SerializeField] GameObject garrisonSlot7;

    void Start ()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();   
        playerManager.OnNextPlayerTurn.AddListener(UpdateCitySelectionAvailability);
        ownedByPlayer.GetComponent<Player>().CheckPlayerStatus();
        gameGrid = FindObjectOfType<GameGrid>();
        enteranceCells = new List<PathNode>();
    }

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
    }

    #region City buildings

    [Header("Buildings: Basic")]
    public bool villageHall = true;
    public bool townHall;
    public bool cityHall;
    public bool tavern;
    public bool prison = true;
    public bool fort;
    public bool citadel;
    public bool castle;
    public bool caravan;
    public bool shipyard;

    [Header("Buildings: Bonus")]
    public bool bonusBuilding_1;
    public bool bonusBuilding_2;
    public bool equipementBuilding;
    public bool racialBuilding;
    public bool graalBuilding;

    [Header("Buildings: Magic")]
    public bool magicHall_1;
    public bool magicHall_2;
    public bool magicHall_3;
    public bool magicHall_4;
    public bool magicHall_5;
    public bool addMagicBuilding_1;
    public bool addMagicBuilding_2;

    [Header("Buildings: Unit buildings")]
    public bool tier1Up;
    public bool tier1Down;
    public bool tier2Up;
    public bool tier2Down;
    public bool tier3Up;
    public bool tier3Down;
    public bool tier4Up;
    public bool tier4Down;

    #endregion

    public void CityInitialization (string _ownedByPlayer, 
        string _cityFraction, Vector2Int _gridPosition,
        float _cityOrientation, int [] _cityBuildingStatus, int [] _cityGarrison)
    {
        cityFraction = _cityFraction;
        gridPosition = _gridPosition;
        rotation.y = _cityOrientation;
        transform.localEulerAngles = rotation;

        #region City buildings

        //basic
        villageHall =           Convert.ToBoolean(_cityBuildingStatus[0]);
        townHall =              Convert.ToBoolean(_cityBuildingStatus[1]);
        cityHall =              Convert.ToBoolean(_cityBuildingStatus[2]);
        tavern =                Convert.ToBoolean(_cityBuildingStatus[3]);
        prison =                Convert.ToBoolean(_cityBuildingStatus[4]);
        fort =                  Convert.ToBoolean(_cityBuildingStatus[5]);
        citadel =               Convert.ToBoolean(_cityBuildingStatus[6]);
        castle =                Convert.ToBoolean(_cityBuildingStatus[7]);
        caravan =               Convert.ToBoolean(_cityBuildingStatus[8]);
        shipyard =              Convert.ToBoolean(_cityBuildingStatus[9]);
        //bonus
        bonusBuilding_1 =       Convert.ToBoolean(_cityBuildingStatus[10]);
        bonusBuilding_2 =       Convert.ToBoolean(_cityBuildingStatus[11]);
        equipementBuilding =    Convert.ToBoolean(_cityBuildingStatus[12]);
        racialBuilding =        Convert.ToBoolean(_cityBuildingStatus[13]);
        graalBuilding =         Convert.ToBoolean(_cityBuildingStatus[14]);
        //magic
        magicHall_1 =           Convert.ToBoolean(_cityBuildingStatus[15]);
        magicHall_2 =           Convert.ToBoolean(_cityBuildingStatus[16]);
        magicHall_3 =           Convert.ToBoolean(_cityBuildingStatus[17]);
        magicHall_4 =           Convert.ToBoolean(_cityBuildingStatus[18]);
        magicHall_5 =           Convert.ToBoolean(_cityBuildingStatus[19]);
        addMagicBuilding_1 =    Convert.ToBoolean(_cityBuildingStatus[20]);
        addMagicBuilding_2 =    Convert.ToBoolean(_cityBuildingStatus[21]);
        //unit
        tier1Up =               Convert.ToBoolean(_cityBuildingStatus[22]);
        tier1Down =             Convert.ToBoolean(_cityBuildingStatus[23]);
        tier2Up =               Convert.ToBoolean(_cityBuildingStatus[24]);
        tier2Down =             Convert.ToBoolean(_cityBuildingStatus[25]);
        tier3Up =               Convert.ToBoolean(_cityBuildingStatus[26]);
        tier3Down =             Convert.ToBoolean(_cityBuildingStatus[27]);
        tier4Up =               Convert.ToBoolean(_cityBuildingStatus[28]);
        tier4Down =             Convert.ToBoolean(_cityBuildingStatus[29]);
        #endregion

        #region City Garrison
       
        garrisonSlot1.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[0], _cityGarrison[1]);
        garrisonSlot2.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[2], _cityGarrison[3]);
        garrisonSlot3.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[4], _cityGarrison[5]);
        garrisonSlot4.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[6], _cityGarrison[7]);
        garrisonSlot5.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[8], _cityGarrison[9]);
        garrisonSlot6.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[10], _cityGarrison[11]);
        garrisonSlot7.GetComponent<GarrisonSlot>().SetSlotStatus(_cityGarrison[12], _cityGarrison[13]);

        
        #endregion

        CityGoldProductionCheck();
    }

    private void CityGoldProductionCheck()
    {
        if (villageHall){
            cityGoldProduction = 250;
            if (townHall){
                cityGoldProduction = 500;
                if (cityHall){
                    cityGoldProduction = 1000;
                }
            }
        }else{
            villageHall = true;
            CityGoldProductionCheck();
        }
    }

    private void UpdateCitySelectionAvailability(Player _player)
    {
        if (_player.gameObject.name  == ownedByPlayer.name){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    public void CityInteraction (GameObject interactingArmy)
    {
        Debug.Log("Interacting army with city: " + interactingArmy.name);
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            //do sth with friendly city
        }else{
            //do sth with other player city
        }
    }

    public void GetEnteranceInformation (List <PathNode> _enteranceList)
    {
        enteranceCells = _enteranceList;
    }

    public float GetCityRotation ()
    {
        return rotation.y;
    }
}
