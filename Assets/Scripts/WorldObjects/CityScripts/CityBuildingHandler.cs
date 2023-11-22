using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityBuildingHandler : MonoBehaviour
{
    private City city;
    [SerializeField] private CityBuildingState[] cityBuildings;
    [SerializeField] private CityDwellingInformation cityDwellingInformation;
    private bool buildingAlreadybuilt;

    public void InitializeBuildings (byte[] cityBuildingStatus, City city){
        cityBuildings = new CityBuildingState[34];
        cityDwellingInformation.Initialize();
        this.city = city;
        buildingAlreadybuilt = false;

        for (int i = 0; i < cityBuildingStatus.Length; i++){
            cityBuildings[i] = (CityBuildingState)cityBuildingStatus[i];
        }

        if (cityBuildings[(byte)BuildingID.Tier1_1] == CityBuildingState.Built || cityBuildings[(byte)BuildingID.Tier1_2] == CityBuildingState.Built){
            cityBuildings[(byte)BuildingID.Tier_1] = CityBuildingState.Built;
        }
        if (cityBuildings[(byte)BuildingID.Tier2_1] == CityBuildingState.Built || cityBuildings[(byte)BuildingID.Tier2_2] == CityBuildingState.Built){
            cityBuildings[(byte)BuildingID.Tier_2] = CityBuildingState.Built;
        }
        if (cityBuildings[(byte)BuildingID.Tier3_1] == CityBuildingState.Built || cityBuildings[(byte)BuildingID.Tier3_2] == CityBuildingState.Built){
            cityBuildings[(byte)BuildingID.Tier_3] = CityBuildingState.Built;
        }
        if (cityBuildings[(byte)BuildingID.Tier4_1] == CityBuildingState.Built || cityBuildings[(byte)BuildingID.Tier4_2] == CityBuildingState.Built){
            cityBuildings[(byte)BuildingID.Tier_4] = CityBuildingState.Built;
        }
    }

    public void CreateNewBuilding(BuildingID buildingId) {
        buildingAlreadybuilt = true;
        int id = (int)buildingId;
        if (id > 21 && id < 30) {
            int dwellingIndex = id - 22;
            cityBuildings[id] = CityBuildingState.Built;
            cityBuildings[30 + dwellingIndex] = CityBuildingState.Built;
            cityDwellingInformation.AddDwelling(city.GetFraction(), dwellingIndex + 1);
        } else if (id > 29 && id < 34) {
            int tierIndex = id - 30;
            int alternativeIndex = id + (tierIndex % 2 == 0 ? 1 : -1);
            cityBuildings[id] = CityBuildingState.Built;
            cityBuildings[alternativeIndex] = CityBuildingState.Blocked;
            cityBuildings[31 + tierIndex] = CityBuildingState.Built;
            cityDwellingInformation.AddDwelling(city.GetFraction(), 3 + tierIndex);
        } else {
            cityBuildings[id] = CityBuildingState.Built;
        }
    }

    public void ChangeOwningPlayer (Player player) { cityDwellingInformation.ChangeOwningPlayer(player); }

    public void DailyUpdate(){
        buildingAlreadybuilt = false;
        cityDwellingInformation.AddDailyUnits();
    }

    public ResourceIncome GetCityIncome (){
        int cityGoldProduction;
        if (cityBuildings[0] == CityBuildingState.Built){
            cityGoldProduction = 500;
            if (cityBuildings[1] == CityBuildingState.Built){
                cityGoldProduction = 750;
                if (cityBuildings[2] == CityBuildingState.Built){
                    cityGoldProduction = 1000;
                }
            }
        }else{
            cityBuildings[0] = CityBuildingState.Built;
            return GetCityIncome();
        }
        return new ResourceIncome(cityGoldProduction, ResourceType.Gold);
    }

    public bool IsBuildingAlreadyBuilt () { return buildingAlreadybuilt; }

    public CityBuildingState[] GetBuildingStates () { return cityBuildings; }

    public CityDwellingInformation GetCityDwellingInformation () { return cityDwellingInformation; }

}
