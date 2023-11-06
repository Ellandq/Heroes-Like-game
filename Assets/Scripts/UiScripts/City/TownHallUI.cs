using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TownHallUI : MonoBehaviour
{

    [SerializeField] private BuildingCostDisplay buildingCostDisplay;
    [SerializeField] private List <BuildingInformationObject> buildingInformation;
    
    [Header ("Building Status Sprites")]
    [SerializeField] Sprite buildingMaxed;
    [SerializeField] Sprite buildingReadyToCreate;
    [SerializeField] Sprite buildingNotReadyToCreate;
    [SerializeField] Sprite buildingNotAvailable;

    [Header ("Building information")]
    private List<BuildingUI> buildingUIList;
    private CityBuildingState[] buildingStates;
    private CityFraction fraction;

    private void Start (){
        buildingUIList = new List<BuildingUI>(20);
        buildingStates = CityManager.Instance.GetCity().GetBuildingHandler().GetBuildingStates();
        fraction = CityManager.Instance.GetCity().GetFraction();
    }

    public void ActivateElement ()
    {
        RefreshElement();
        gameObject.SetActive(true);
    }

    public void DisableElement ()
    {
        gameObject.SetActive(false);
    }

    private int HandleBuilding (BuildingID id, int convertedId){
        int index = (int)id;
        if (index < 3){
            HandleHall();
            return 3;
        }else if (index >= 5 && index <= 7){
            HandleWalls();
            return 3;
        }else if (index >= 15 && index <= 19){
            HandleMagic();
            return 5;
        }else if (index >= 22 && index <= 29){
            HandleUnit();
            return 8;
        }else{
            if (index == 4 || index == 14) return 0;
            HandleMisc(id, convertedId);
            return 0;
        }
    }

    private void HandleHall()
    {
        UpdateBuildingUI(0, "Village Hall", BuildingID.VillageHall, GetSprite(BuildingID.VillageHall), buildingStates[(int)BuildingID.VillageHall], true);
        UpdateBuildingUI(0, "Town Hall", BuildingID.TownHall, GetSprite(BuildingID.TownHall), buildingStates[(int)BuildingID.TownHall]);
        UpdateBuildingUI(0, "City Hall", BuildingID.CityHall, GetSprite(BuildingID.CityHall), buildingStates[(int)BuildingID.CityHall]);
    }

    private void HandleWalls()
    {
        UpdateBuildingUI(2, "Fort", BuildingID.Fort, GetSprite(BuildingID.Fort), buildingStates[(int)BuildingID.Fort], true);
        UpdateBuildingUI(2, "Citadel", BuildingID.Citadel, GetSprite(BuildingID.Citadel), buildingStates[(int)BuildingID.Citadel]);
        UpdateBuildingUI(2, "Castle", BuildingID.Castle, GetSprite(BuildingID.Castle), buildingStates[(int)BuildingID.Castle]);
    }

    private void HandleUnit()
    {
        for (int i = 13; i <= 20; i++){
            BuildingID id = (BuildingID)(i + 9);
            UpdateBuildingUI(i, id.ToString(), id, GetSprite(id), buildingStates[i + 9], true);
        }
        
    }

    private void HandleMagic()
    {
        UpdateBuildingUI(9, "Magic Guild lv. 1", BuildingID.MagicHall_1, GetSprite(BuildingID.MagicHall_1), buildingStates[(int)BuildingID.MagicHall_1], true);
        UpdateBuildingUI(9, "Magic Guild lv. 2", BuildingID.MagicHall_2, GetSprite(BuildingID.MagicHall_2), buildingStates[(int)BuildingID.MagicHall_2]);
        UpdateBuildingUI(9, "Magic Guild lv. 3", BuildingID.MagicHall_3, GetSprite(BuildingID.MagicHall_3), buildingStates[(int)BuildingID.MagicHall_3]);
        UpdateBuildingUI(9, "Magic Guild lv. 4", BuildingID.MagicHall_4, GetSprite(BuildingID.MagicHall_4), buildingStates[(int)BuildingID.MagicHall_4]);
        UpdateBuildingUI(9, "Magic Guild lv. 5", BuildingID.MagicHall_5, GetSprite(BuildingID.MagicHall_5), buildingStates[(int)BuildingID.MagicHall_5]);
    }

    private void HandleMisc(BuildingID id, int convertedId)
    {
        UpdateBuildingUI(convertedId, id.ToString(), id, GetSprite(id), buildingStates[convertedId], true);
    }

    void UpdateBuildingUI(int index, string name, BuildingID id, Sprite icon, CityBuildingState state, bool baseLevel = false)
    {
        if (baseLevel){
            buildingUIList[index].Image = icon;
            buildingUIList[index].Name = name;
        }
        

        if (!state.Equals(CityBuildingState.Blocked))
        {
            if (state.Equals(CityBuildingState.Built))
            {
                buildingUIList[index].Status = buildingMaxed;
                buildingUIList[index].Image = icon;
                buildingUIList[index].Name = name;
            }
            else
            {
                if (CheckBuildingRequirements(id))
                {
                    buildingUIList[index].Status = buildingReadyToCreate;
                    buildingUIList[index].Interactable = true;
                }
                else
                {
                    buildingUIList[index].Status = buildingNotReadyToCreate;
                }
            }
        }
        else
        {
            buildingUIList[index].Status = buildingNotAvailable;
            buildingUIList[index].Interactable = false;
        }
    }

    private Sprite GetSprite (BuildingID id){
        return Resources.Load<Sprite>("TownBuildingIcons/" + fraction.ToString() + "/" + id.ToString());
    }


    private void RefreshElement ()
    {
        int index = 0;
        for (int i = 0; i <= Enum.GetValues(typeof(BuildingID)).Cast<int>().Max(); i++){
            index += HandleBuilding((BuildingID)i, index);
            index++;
        }
    }

    public void OpenBuildingUI(BuildingID id) {
        bool shouldUpdateDisplay = false;

        switch (id) {
            case BuildingID.VillageHall:
                shouldUpdateDisplay = buildingUIList[0].Level == 0;
                break;

            case BuildingID.TownHall:
                shouldUpdateDisplay = buildingUIList[0].Level == 1;
                break;

            case BuildingID.CityHall:
            case BuildingID.Tavern:
            case BuildingID.Prison:
            case BuildingID.Castle:
            case BuildingID.Caravan:
            case BuildingID.Shipyard:
            case BuildingID.BonusBuilding_1:
            case BuildingID.BonusBuilding_2:
            case BuildingID.EquipementBuilding:
            case BuildingID.RacialBuilding:
                shouldUpdateDisplay = true;
                break;

            case BuildingID.Fort:
                shouldUpdateDisplay = buildingUIList[2].Level == 0;
                break;

            case BuildingID.Citadel:
                shouldUpdateDisplay = buildingUIList[2].Level == 1;
                break;

            case BuildingID.MagicHall_1:
                shouldUpdateDisplay = buildingUIList[9].Level == 0;
                break;

            case BuildingID.MagicHall_2:
                shouldUpdateDisplay = buildingUIList[9].Level == 1;
                break;

            case BuildingID.MagicHall_3:
                shouldUpdateDisplay = buildingUIList[9].Level == 2;
                break;

            case BuildingID.MagicHall_4:
                shouldUpdateDisplay = buildingUIList[9].Level == 3;
                break;

            case BuildingID.MagicHall_5:
            case BuildingID.AdditionalMagic_1:
            case BuildingID.AdditionalMagic_2:
            case BuildingID.Tier1_1:
            case BuildingID.Tier1_2:
            case BuildingID.Tier2_1:
            case BuildingID.Tier2_2:
            case BuildingID.Tier3_1:
            case BuildingID.Tier3_2:
            case BuildingID.Tier4_1:
            case BuildingID.Tier4_2:
                shouldUpdateDisplay = true;
                break;
        }

        if (shouldUpdateDisplay) {
            buildingCostDisplay.UpdateDisplay(buildingInformation[(int)id], CheckBuildingRequirements(id));
        } else {
            OpenBuildingUI(id + 1);
        }
    }



    private bool CheckBuildingRequirements (BuildingID buildingID)
    {
        if (CityManager.Instance.GetCity().isBuildingAlreadyBuilt()) return false;
        if (buildingInformation[(int)buildingID].additionalRequirements.Count > 0){
            for (int i = 0; i < buildingInformation[(int)buildingID].additionalRequirements.Count; i++){
                if (buildingStates[Convert.ToInt32(buildingInformation[(int)buildingID].additionalRequirements[i])] == 0) return false;
            }
        }
        return PlayerManager.Instance.GetCurrentPlayer().GetAvailableResources() > buildingInformation[(int)buildingID].cost;
    }
}

public enum BuildingID{
    // Basic buildings
    VillageHall, TownHall, CityHall, Tavern, Prison, Fort, Citadel, Castle, Caravan, Shipyard,
    // Bonus buildings
    BonusBuilding_1, BonusBuilding_2, EquipementBuilding, RacialBuilding, GraalBuilding,
    // Magic buildings
    MagicHall_1, MagicHall_2, MagicHall_3, MagicHall_4, MagicHall_5, AdditionalMagic_1, AdditionalMagic_2,
    // Unit Buildings
    Tier1_1, Tier1_2, Tier2_1, Tier2_2, Tier3_1, Tier3_2, Tier4_1, Tier4_2,
    // Conditional ID's
    Tier_1, Tier_2, Tier_3, Tier_4
}