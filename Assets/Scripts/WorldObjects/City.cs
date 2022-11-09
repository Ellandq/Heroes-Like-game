using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameGrid gameGrid;
    [SerializeField] GameObject flag;

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
    bool cityEmpty;
    [SerializeField] public List <GameObject> garrisonSlots;

    #region City buildings

    [Header("Buildings: Basic")]
    public short villageHall = 1;
    public short townHall;
    public short cityHall;
    public short tavern;
    public short prison = 1;
    public short fort;
    public short citadel;
    public short castle;
    public short caravan;
    public short shipyard;

    [Header("Buildings: Bonus")]
    public short bonusBuilding_1;
    public short bonusBuilding_2;
    public short equipementBuilding;
    public short racialBuilding;
    public short graalBuilding;

    [Header("Buildings: Magic")]
    public short magicHall_1;
    public short magicHall_2;
    public short magicHall_3;
    public short magicHall_4;
    public short magicHall_5;
    public short addMagicBuilding_1;
    public short addMagicBuilding_2;

    [Header("Buildings: Unit buildings")]
    public short tier1Up;
    public short tier1Down;
    public short tier2Up;
    public short tier2Down;
    public short tier3Up;
    public short tier3Down;
    public short tier4Up;
    public short tier4Down;

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
        villageHall =           Convert.ToInt16(_cityBuildingStatus[0]);
        townHall =              Convert.ToInt16(_cityBuildingStatus[1]);
        cityHall =              Convert.ToInt16(_cityBuildingStatus[2]);
        tavern =                Convert.ToInt16(_cityBuildingStatus[3]);
        prison =                Convert.ToInt16(_cityBuildingStatus[4]);
        fort =                  Convert.ToInt16(_cityBuildingStatus[5]);
        citadel =               Convert.ToInt16(_cityBuildingStatus[6]);
        castle =                Convert.ToInt16(_cityBuildingStatus[7]);
        caravan =               Convert.ToInt16(_cityBuildingStatus[8]);
        shipyard =              Convert.ToInt16(_cityBuildingStatus[9]);
        //bonus
        bonusBuilding_1 =       Convert.ToInt16(_cityBuildingStatus[10]);
        bonusBuilding_2 =       Convert.ToInt16(_cityBuildingStatus[11]);
        equipementBuilding =    Convert.ToInt16(_cityBuildingStatus[12]);
        racialBuilding =        Convert.ToInt16(_cityBuildingStatus[13]);
        graalBuilding =         Convert.ToInt16(_cityBuildingStatus[14]);
        //magic
        magicHall_1 =           Convert.ToInt16(_cityBuildingStatus[15]);
        magicHall_2 =           Convert.ToInt16(_cityBuildingStatus[16]);
        magicHall_3 =           Convert.ToInt16(_cityBuildingStatus[17]);
        magicHall_4 =           Convert.ToInt16(_cityBuildingStatus[18]);
        magicHall_5 =           Convert.ToInt16(_cityBuildingStatus[19]);
        addMagicBuilding_1 =    Convert.ToInt16(_cityBuildingStatus[20]);
        addMagicBuilding_2 =    Convert.ToInt16(_cityBuildingStatus[21]);
        //unit
        tier1Up =               Convert.ToInt16(_cityBuildingStatus[22]);
        tier1Down =             Convert.ToInt16(_cityBuildingStatus[23]);
        tier2Up =               Convert.ToInt16(_cityBuildingStatus[24]);
        tier2Down =             Convert.ToInt16(_cityBuildingStatus[25]);
        tier3Up =               Convert.ToInt16(_cityBuildingStatus[26]);
        tier3Down =             Convert.ToInt16(_cityBuildingStatus[27]);
        tier4Up =               Convert.ToInt16(_cityBuildingStatus[28]);
        tier4Down =             Convert.ToInt16(_cityBuildingStatus[29]);
        #endregion

        #region City Garrison
        garrisonSlots[0].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[0], _cityGarrison[1]);
        garrisonSlots[1].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[2], _cityGarrison[3]);
        garrisonSlots[2].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[4], _cityGarrison[5]);
        garrisonSlots[3].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[6], _cityGarrison[7]);
        garrisonSlots[4].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[8], _cityGarrison[9]);
        garrisonSlots[5].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[10], _cityGarrison[11]);
        garrisonSlots[6].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[12], _cityGarrison[13]);

        
        #endregion

        CityGoldProductionCheck();
    }

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
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
    }

    private void ChangeOwningPlayer (GameObject _ownedByPlayer)
    {
        ownedByPlayer.GetComponent<Player>().ownedCities.Remove(this.gameObject);
        if (ownedByPlayer.name == "Neutral Player"){
            ownedByPlayer = _ownedByPlayer;
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }else{
            ownedByPlayer = _ownedByPlayer;
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        ownedByPlayer.GetComponent<Player>().ownedCities.Add(this.gameObject);
        ownedByPlayer.GetComponent<Player>().onCityAdded?.Invoke();
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = playerManager.neutralPlayer;
        flag.SetActive(false);
    }

    private void CityGoldProductionCheck()
    {
        if (villageHall == 1){
            cityGoldProduction = 250;
            if (townHall == 1){
                cityGoldProduction = 500;
                if (cityHall == 1){
                    cityGoldProduction = 1000;
                }
            }
        }else{
            villageHall = 1;
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
            SceneStateManager.EnterCity(this.gameObject, cityFraction);
        }else{
            if (IsCityEmpty()){
                ChangeOwningPlayer(interactingArmy.GetComponent<Army>().ownedByPlayer);
            }else{
                Debug.Log("Do battle");
            }
        }
    }

    public void CityInteraction ()
    {
        SceneStateManager.EnterCity(this.gameObject, cityFraction);
    }

    public void GetEnteranceInformation (List <PathNode> _enteranceList)
    {
        enteranceCells = _enteranceList;
    }

    public float GetCityRotation ()
    {
        return rotation.y;
    }

    private bool IsCityEmpty ()
    {
        if (!garrisonSlots[0].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[1].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[2].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[3].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[4].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[5].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[6].GetComponent<UnitSlot>().slotEmpty) return false;
        return true;
    }

    public List<GameObject> GetCityGarrison ()
    {
        return garrisonSlots;
    }

    public void SwapUnitsPosition (short a, short b)
    {
        int id01 = garrisonSlots[a].GetComponent<UnitSlot>().unitID;
        int id02 = garrisonSlots[b].GetComponent<UnitSlot>().unitID;
        int unitCount01 = garrisonSlots[a].GetComponent<UnitSlot>().howManyUnits;
        int unitCount02 = garrisonSlots[b].GetComponent<UnitSlot>().howManyUnits;
        float mPoints01 = garrisonSlots[a].GetComponent<UnitSlot>().movementPoints;
        float mPoints02 = garrisonSlots[b].GetComponent<UnitSlot>().movementPoints;
        garrisonSlots[a].GetComponent<UnitSlot>().RemoveUnits();
        garrisonSlots[b].GetComponent<UnitSlot>().RemoveUnits();

        garrisonSlots[a].GetComponent<UnitSlot>().ChangeSlotStatus(id02, unitCount02, mPoints02);
        garrisonSlots[b].GetComponent<UnitSlot>().ChangeSlotStatus(id01, unitCount01, mPoints01);
    }

    public void AddUnits (short a, short b)
    {
        garrisonSlots[b].GetComponent<UnitSlot>().howManyUnits += garrisonSlots[a].GetComponent<UnitSlot>().howManyUnits;
        garrisonSlots[a].GetComponent<UnitSlot>().RemoveUnits();
    }

    public bool AreGarrisonSlotsSameType (short a, short b)
    {
        if (garrisonSlots[a].GetComponent<UnitSlot>().unitID == garrisonSlots[b].GetComponent<UnitSlot>().unitID) return true;
        else return false;
    }
}
