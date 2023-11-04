using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class City : WorldObject, IObjectInteraction
{
    [SerializeField] private GameObject flag;

    [Header("Main city information")]
    private CityFraction cityFraction;
    private bool canBeSelectedByCurrentPlayer;

    [Header("City Enterance Information")]
    [SerializeField] private ObjectEnterance cityEnterance;

    [Header("Garrison refrences")]
    [SerializeField] private UnitsInformation unitsInformation;

    [Header ("City Buildings")]
    [SerializeField] private CityBuildingHandler buildingHandler;

    #region Initialization

    public void Initialize (Vector2Int gridPosition, float rotation, 
        PlayerTag ownedByPlayer, CityFraction cityFraction, 
        byte [] cityBuildingStatus, short [] cityGarrison)
    {
        Initialize(gridPosition, rotation, ObjectType.City);
        ChangeOwningPlayer(ownedByPlayer);
        this.cityFraction = cityFraction;

        unitsInformation.SetUnitStatus(cityGarrison);
        buildingHandler.InitializeBuildings(cityBuildingStatus, this);

        GameGrid.Instance.PlaceBuildingOnGrid(gridPosition, BuildingType.FiveByFive, GetRotation(), gameObject);
    }

    #endregion

    #region Player Management

    public override void ChangeOwningPlayer(PlayerTag ownedByPlayer)
    {
        PlayerManager.Instance.GetPlayer(GetPlayerTag()).RemoveObject(this);
        base.ChangeOwningPlayer(ownedByPlayer);
        if (ownedByPlayer != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColour(GetPlayerTag());
        }else{
            flag.SetActive(false);
        }
        Player player = PlayerManager.Instance.GetPlayer(GetPlayerTag());
        player.AddObject(this);
        buildingHandler.ChangeOwningPlayer(player);
    }

    #endregion
    
    #region Updates

    // Check if the city can be selected by a given player (on new turn update)
    public void UpdateCitySelectionAvailability()
    {
        if (PlayerManager.Instance.GetCurrentPlayerTag() == GetPlayerTag()){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    public void CityTurnUpdate (){
        UpdateCitySelectionAvailability();
    }

    // The daily update for a city
    public void CityDailyUpdate (){
        buildingHandler.DailyUpdate();
    }

    #endregion

    #region Interaction Management

    public void Interact<T> (T other){
        Army army = other as Army;
        Debug.Log("Interacting army with city: " + army.gameObject.name);
        if (army.GetPlayerTag() == GetPlayerTag()){
            GameManager.Instance.EnterCity(this, army);
        }else{
            if (IsCityEmpty()){
                ChangeOwningPlayer(army.GetPlayerTag());
            }else{
                Debug.Log("Do battle");
            }
        }
    }

    public void Interact (){
        GameManager.Instance.EnterCity(this);
    }

    public override void ObjectSelected(){
        // TODO
    }

    #endregion

    #region Setters

    public void AddUnits (short unitID, short unitCount, short garrisonIndex){
        unitsInformation.AddUnits(unitID, unitCount, garrisonIndex);
    }

    #endregion

    #region Getters

    public Sprite GetCitySprite (){
        return Resources.Load<Sprite>("CityIcons/" + cityFraction.ToString() + "/" + cityFraction.ToString() + "CityIcon_PlaceHolder");
    }

    public CityFraction GetFraction () { return cityFraction; }

    public UnitsInformation GetUnitsInformation () { return unitsInformation; }

    public ResourceIncome GetIncome (){
        return buildingHandler.GetCityIncome();
    }

    public List<short> GetEmptyGarrisonSlotCount (){
        return unitsInformation.GetEmptySlots();
    }

    public int GetSameUnitSlotIndex (short id){
        return unitsInformation.GetSameUnitSlotIndex(id);
    }

    private bool IsCityEmpty (){
        return unitsInformation.IsArmyEmpty();
    }

    public bool IsSelectableByCurrentPlayer () { return canBeSelectedByCurrentPlayer; }

    #endregion

    protected override void OnDestroy()
    {
        ObjectSelector.Instance.CancelSelection(this);
        base.OnDestroy();
    }

}
