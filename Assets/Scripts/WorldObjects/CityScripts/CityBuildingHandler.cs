using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuildingHandler : MonoBehaviour
{
    private City city;
    private CityBuildingState[] cityBuildings;
    private CityDwellingInformation cityDwellingInformation;
    private bool buildingAlreadybuilt;

    public void InitializeBuildings (byte[] cityBuildingStatus, City city){
        CityBuildingState[] cityBuildings = new CityBuildingState[34];
        cityDwellingInformation = new CityDwellingInformation();
        this.city = city;
        buildingAlreadybuilt = false;

        // Initialize basic buildings
        for (int i = 0; i < 22; i++) {
            cityBuildings[i] = (CityBuildingState)cityBuildingStatus[i];
        }

        // Initialize tier 1 dwellings
        for (int i = 22; i <= 23; i++) {
            cityBuildings[i] = CityBuildingState.Built;
            cityBuildings[30] = CityBuildingState.Built;
            cityDwellingInformation.AddDwelling(city.GetFraction(), i - 21);
        }

        // Initialize tier 2, 3, 4 dwellings based on the 24th index
        int tierStartIndex = 24;
        int tierDwellingIndex = 3;  // The tier 2 dwelling corresponds to the 24th index, tier 3 to the 26th, and tier 4 to the 28th.
        for (int i = tierStartIndex; i < tierStartIndex + 6; i++) {
            cityBuildings[i] = CityBuildingState.Built;
            cityBuildings[i + 7] = CityBuildingState.Blocked;
            cityBuildings[31 + tierDwellingIndex] = CityBuildingState.Built;
            cityDwellingInformation.AddDwelling(city.GetFraction(), tierDwellingIndex + 3);
            tierDwellingIndex++;
        }

        // The remaining buildings are initialized in a similar fashion

        // Initialize bonus, magic, and unit buildings
        for (int i = 10; i <= 34; i++) {
            cityBuildings[i] = (CityBuildingState)cityBuildingStatus[i];
        }

        // Initialize the last entry
        cityBuildings[34] = CityBuildingState.Built;  // Or any appropriate initialization for the last building
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
