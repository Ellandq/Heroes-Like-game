using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class City : WorldObject
{
    [SerializeField] private GameObject flag;

    [Header("Main city information")]
    private Player player;
    private CityFraction cityFraction;
    private Sprite cityIcon;
    private bool canBeSelectedByCurrentPlayer;
    private bool cityBuildingAlreadybuilt;

    [Header("City Enterance Information")]
    [SerializeField] private GameObject cityEnterance;
    private List<PathNode> enteranceCells;

    [Header("Garrison refrences")]
    [SerializeField] private UnitsInformation unitsInformation;

    [Header ("City Buildings")]
    [SerializeField] private CityBuildingHandler buildingHandler;

    #region Initialization

    public void Initialize (Vector2Int gridPosition, float rotation, 
        PlayerTag ownedByPlayer, CityFraction cityFraction, 
        int [] cityBuildingStatus, int [] cityGarrison)
    {
        Initialize(gridPosition, rotation, ObjectType.City);
        ChangeOwningPlayer(ownedByPlayer);
        this.cityFraction = cityFraction;
        
        cityIcon = GetCitySprite();
        cityBuildingAlreadybuilt = false;


        unitsInformation.SetUnitStatus(cityGarrison);
        buildingHandler.InitializeBuildings(cityBuildingStatus, this);

        FinalizeCity();
    }

    private void FinalizeCity ()
    {
        PlayerManager.Instance.OnNextPlayerTurn.AddListener(UpdateCitySelectionAvailability);
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(CityDailyUpdate);
        GameGrid.Instance.PlaceBuildingOnGrid(gridPosition, BuildingType.FiveByFive, GetRotation(), gameObject);
        GameGrid.Instance.GetGridCellInformation(GameGrid.Instance.GetGridPosFromWorld(cityEnterance.transform.position)).AddOccupyingObject(cityEnterance);
        enteranceCells = new List<PathNode>();
    }

    private Sprite GetCitySprite (){
        return Resources.Load<Sprite>("CityIcons/" + cityFraction.ToString() + "/" + cityFraction.ToString() + "CityIcon_PlaceHolder");
    }

    #region Player Management

    public override void ChangeOwningPlayer(PlayerTag playerTag)
    {
        if (player != null){
            player
        }
        if (playerTag != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColour(playerTag);
        }else{
            flag.SetActive(false);
        }
        base.ChangeOwningPlayer(playerTag);
    }

    private void ChangeOwningPlayer (PlayerTag playerTag)
    {
        ownedByPlayer.GetComponent<Player>().ownedCities.Remove(this.gameObject);
        if (playerTag == PlayerTag.None){
            ownedByPlayer = _ownedByPlayer;
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }else{
            ownedByPlayer = _ownedByPlayer;
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        cityDwellingInformation.ChangeOwningPlayer(ownedByPlayer.GetComponent<Player>());
        ownedByPlayer.GetComponent<Player>().ownedCities.Add(this.gameObject);
        ownedByPlayer.GetComponent<Player>().onCityAdded?.Invoke();
    }

    // Remove the owning player
    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
    }

    #endregion
    
    // Check if the city can be selected by a given player (on new turn update)
    private void UpdateCitySelectionAvailability(Player _player)
    {
        if (_player.gameObject.name  == ownedByPlayer.name){
            cityBuildingAlreadybuilt = false;
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    // The daily update for a city
    private void CityDailyUpdate ()
    {
        cityDwellingInformation.AddDailyUnits();
    }

    #region Interaction Management

    // Interaction with a city by a given army
    public void CityInteraction (GameObject interactingArmy)
    {
        Debug.Log("Interacting army with city: " + interactingArmy.name);
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            GameManager.Instance.EnterCity(this.gameObject, cityFraction, interactingArmy.GetComponentInParent<Army>());
        }else{
            if (IsCityEmpty()){
                ChangeOwningPlayer(interactingArmy.GetComponent<Army>().ownedByPlayer);
            }else{
                Debug.Log("Do battle");
            }
        }
    }

    // Interaction with a city
    public void CityInteraction ()
    {
        GameManager.Instance.EnterCity(this.gameObject, cityFraction);
    }

    #endregion

    // Return a list of city enterences nodes
    public void GetEnteranceInformation (List <PathNode> _enteranceList)
    {
        enteranceCells = _enteranceList;
    }

    // Return the current city rotation
    public float GetCityRotation ()
    {
        return rotation.y;
    }

    // Check if City is empty
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

    // Return a list of id's of empty garrison slots
    public List<int> GetEmptyGarrisonSlotCount ()
    {
        List<int> emptySlots = new List<int>();
        if (garrisonSlots[0].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(0);
        if (garrisonSlots[1].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(1);
        if (garrisonSlots[2].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(2);
        if (garrisonSlots[3].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(3);
        if (garrisonSlots[4].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(4);
        if (garrisonSlots[5].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(5);
        if (garrisonSlots[6].GetComponent<UnitSlot>().slotEmpty) emptySlots.Add(6);
        return emptySlots;
    }

    public CityFraction GetFraction () { return cityFraction; }
    
    // Return an id of the same unit type
    public int GetSameUnitSlotIndex (int id)
    {
        if (garrisonSlots[0].GetComponent<UnitSlot>().unitID == id) return 0;
        if (garrisonSlots[1].GetComponent<UnitSlot>().unitID == id) return 1;
        if (garrisonSlots[2].GetComponent<UnitSlot>().unitID == id) return 2;
        if (garrisonSlots[3].GetComponent<UnitSlot>().unitID == id) return 3;
        if (garrisonSlots[4].GetComponent<UnitSlot>().unitID == id) return 4;
        if (garrisonSlots[5].GetComponent<UnitSlot>().unitID == id) return 5;
        if (garrisonSlots[6].GetComponent<UnitSlot>().unitID == id) return 6;
        return 7;
    }

    // Add units to a selected garrison slot
    public void AddUnits (int unitID, int unitCount, int garrisonIndex){
        if (garrisonSlots[garrisonIndex].GetComponent<UnitSlot>().slotEmpty){
            garrisonSlots[garrisonIndex].GetComponent<UnitSlot>().SetSlotStatus(unitID, unitCount);
        }else{
            garrisonSlots[garrisonIndex].GetComponent<UnitSlot>().AddUnits(unitCount);
        }
    }

}
