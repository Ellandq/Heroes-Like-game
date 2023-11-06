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

    private int HandleBuilding (BuildingID id){
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
            // TODO
            return 0;
        }
    }

    private void HandleHall (){
        buildingUIList[0].Image = buildingInformation[(int)BuildingID.TownHall].buildingIcon;
        buildingUIList[0].Name = "Town Hall";
        buildingUIList[0].Level = (int)BuildingID.VillageHall;
        if (!buildingStates[(int)BuildingID.TownHall].Equals(CityBuildingState.Blocked)){
            if (buildingStates[(int)BuildingID.TownHall].Equals(CityBuildingState.Built)){
                buildingUIList[0].Image = buildingInformation[(int)BuildingID.CityHall].buildingIcon;
                buildingUIList[0].Name = "City Hall";
                buildingUIList[0].Level = (int)BuildingID.TownHall;
                if (!buildingStates[(int)BuildingID.TownHall].Equals(CityBuildingState.Blocked)){
                    if (buildingStates[(int)BuildingID.TownHall].Equals(CityBuildingState.Built)){
                    buildingUIList[0].Level = (int)BuildingID.CityHall;
                    buildingUIList[0].Status = buildingMaxed;
                    }else{
                        if (CheckBuildingRequirements((int)BuildingID.CityHall)){
                            buildingUIList[0].Status = buildingReadyToCreate;
                            buildingUIList[0].Interactable = true;
                        }
                        else buildingUIList[0].Status = buildingNotReadyToCreate;
                    }
                }else{
                    buildingUIList[0].Status = buildingNotAvailable;
                    buildingUIList[0].Interactable = false;
                }
            }else{
                if (CheckBuildingRequirements((int)BuildingID.TownHall)){
                    buildingUIList[0].Status = buildingReadyToCreate;
                    buildingUIList[0].Interactable = true;
                }
                else buildingUIList[0].Status = buildingNotReadyToCreate;
            }
        }else{
            buildingUIList[0].Status = buildingNotAvailable;
            buildingUIList[0].Interactable = false;
        }
    }

    private void HandleWalls (){
        fortImage.sprite = buildingInformation[((int)BuildingID.Fort)].buildingIcon;
        fortName.text = "Fort";
        fortStatus = 0;
        if (!currentCity.cityBuildings[((int)BuildingID.Fort)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Fort)].Equals(CityBuildingState.Built)){
                fortImage.sprite = buildingInformation[((int)BuildingID.Citadel)].buildingIcon;
                fortName.text = "Citadel";
                fortStatus = ((int)BuildingID.Fort);
                if (!currentCity.cityBuildings[((int)BuildingID.Citadel)].Equals(CityBuildingState.Blocked)){
                    if (currentCity.cityBuildings[((int)BuildingID.Citadel)].Equals(CityBuildingState.Built)){
                        fortImage.sprite = buildingInformation[((int)BuildingID.Castle)].buildingIcon;
                        fortName.text = "Castle";
                        fortStatus = ((int)BuildingID.Citadel);
                        if (!currentCity.cityBuildings[((int)BuildingID.Castle)].Equals(CityBuildingState.Blocked)){
                            if (currentCity.cityBuildings[((int)BuildingID.Castle)].Equals(CityBuildingState.Built)){
                                fortStatus = ((int)BuildingID.Citadel);
                                fortStatusImage.sprite = buildingMaxed;
                            }else{
                                if (CheckBuildingRequirements(((int)BuildingID.Castle))){
                                    fortStatusImage.sprite = buildingReadyToCreate;
                                    fortButton.interactable = true;
                                }
                                else fortStatusImage.sprite = buildingNotReadyToCreate;
                            }       
                        }else{
                            fortStatusImage.sprite = buildingNotAvailable;
                            fortButton.interactable = false;
                        }
                    
                    }else{
                        if (CheckBuildingRequirements(((int)BuildingID.Citadel))){
                            fortStatusImage.sprite = buildingReadyToCreate;
                            fortButton.interactable = true;
                        }
                        else fortStatusImage.sprite = buildingNotReadyToCreate;
                    }
                }else{
                    fortStatusImage.sprite = buildingNotAvailable;
                    fortButton.interactable = false;
                }
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Fort))){
                    fortStatusImage.sprite = buildingReadyToCreate;
                    fortButton.interactable = true;
                }
                else fortStatusImage.sprite = buildingNotReadyToCreate;
            }
        }else{
            fortStatusImage.sprite = buildingNotAvailable;
            fortButton.interactable = false;
        }
    }

    private void HandleUnit (){

    }

    private void HandleMagic (){

    }

    private void RefreshElement ()
    {
        for (int i = 0; i <= Enum.GetValues(typeof(BuildingID)).Cast<int>().Max(); i++){
            i += HandleBuilding((BuildingID)i);
        }
        // FORT
        fortImage.sprite = buildingInformation[((int)BuildingID.Fort)].buildingIcon;
        fortName.text = "Fort";
        fortStatus = 0;
        if (!currentCity.cityBuildings[((int)BuildingID.Fort)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Fort)].Equals(CityBuildingState.Built)){
                fortImage.sprite = buildingInformation[((int)BuildingID.Citadel)].buildingIcon;
                fortName.text = "Citadel";
                fortStatus = ((int)BuildingID.Fort);
                if (!currentCity.cityBuildings[((int)BuildingID.Citadel)].Equals(CityBuildingState.Blocked)){
                    if (currentCity.cityBuildings[((int)BuildingID.Citadel)].Equals(CityBuildingState.Built)){
                        fortImage.sprite = buildingInformation[((int)BuildingID.Castle)].buildingIcon;
                        fortName.text = "Castle";
                        fortStatus = ((int)BuildingID.Citadel);
                        if (!currentCity.cityBuildings[((int)BuildingID.Castle)].Equals(CityBuildingState.Blocked)){
                            if (currentCity.cityBuildings[((int)BuildingID.Castle)].Equals(CityBuildingState.Built)){
                                fortStatus = ((int)BuildingID.Citadel);
                                fortStatusImage.sprite = buildingMaxed;
                            }else{
                                if (CheckBuildingRequirements(((int)BuildingID.Castle))){
                                    fortStatusImage.sprite = buildingReadyToCreate;
                                    fortButton.interactable = true;
                                }
                                else fortStatusImage.sprite = buildingNotReadyToCreate;
                            }       
                        }else{
                            fortStatusImage.sprite = buildingNotAvailable;
                            fortButton.interactable = false;
                        }
                    
                    }else{
                        if (CheckBuildingRequirements(((int)BuildingID.Citadel))){
                            fortStatusImage.sprite = buildingReadyToCreate;
                            fortButton.interactable = true;
                        }
                        else fortStatusImage.sprite = buildingNotReadyToCreate;
                    }
                }else{
                    fortStatusImage.sprite = buildingNotAvailable;
                    fortButton.interactable = false;
                }
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Fort))){
                    fortStatusImage.sprite = buildingReadyToCreate;
                    fortButton.interactable = true;
                }
                else fortStatusImage.sprite = buildingNotReadyToCreate;
            }
        }else{
            fortStatusImage.sprite = buildingNotAvailable;
            fortButton.interactable = false;
        }
        // TAVERN
        tavernImage.sprite = buildingInformation[((int)BuildingID.Tavern)].buildingIcon;
        if (!currentCity.cityBuildings[((int)BuildingID.Tavern)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tavern)].Equals(CityBuildingState.Built)){
                tavernStatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tavern))){
                    tavernStatusImage.sprite = buildingReadyToCreate;
                    tavernButton.interactable = true;
                }
                else tavernStatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            tavernStatusImage.sprite = buildingNotAvailable;
            tavernButton.interactable = false;
        }
        // EQUIPEMENT
        equipementImage.sprite = buildingInformation[((int)BuildingID.EquipementBuilding)].buildingIcon;
        equipementName.text = buildingInformation[((int)BuildingID.EquipementBuilding)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.EquipementBuilding)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.EquipementBuilding)].Equals(CityBuildingState.Built)){
                equipementStatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.EquipementBuilding))){
                    equipementStatusImage.sprite = buildingReadyToCreate;
                    equipementButton.interactable = true;
                }
                else equipementStatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            equipementStatusImage.sprite = buildingNotAvailable;
            equipementButton.interactable = false;
        }
        // Tier 1.1
        T1_1_Image.sprite = buildingInformation[((int)BuildingID.Tier1_1)].buildingIcon;
        T1_1_Name.text = buildingInformation[((int)BuildingID.Tier1_1)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier1_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier1_1)].Equals(CityBuildingState.Built)){
                T1_1_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier1_1))){
                    T1_1_StatusImage.sprite = buildingReadyToCreate;
                    T1_1_Button.interactable = true;
                }
                else T1_1_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T1_1_StatusImage.sprite = buildingNotAvailable;
            T1_1_Button.interactable = false;
        }
        // Tier 1.2
        T1_2_Image.sprite = buildingInformation[((int)BuildingID.Tier1_2)].buildingIcon;
        T1_2_Name.text = buildingInformation[((int)BuildingID.Tier1_2)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier1_2)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier1_2)].Equals(CityBuildingState.Built)){
                T1_2_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier1_2))){
                    T1_2_StatusImage.sprite = buildingReadyToCreate;
                    T1_2_Button.interactable = true;
                }
                else T1_2_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T1_2_StatusImage.sprite = buildingNotAvailable;
            T1_2_Button.interactable = false;
        }
        // Tier 2.1
        T2_1_Image.sprite = buildingInformation[((int)BuildingID.Tier2_1)].buildingIcon;
        T2_1_Name.text = buildingInformation[((int)BuildingID.Tier2_1)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier2_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier2_1)].Equals(CityBuildingState.Built)){
                T2_1_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier2_1))){
                    T2_1_StatusImage.sprite = buildingReadyToCreate;
                    T2_1_Button.interactable = true;
                }
                else T2_1_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T2_1_StatusImage.sprite = buildingNotAvailable;
            T2_1_Button.interactable = false;
        }
        // Tier 2.2
        T2_2_Image.sprite = buildingInformation[((int)BuildingID.Tier2_2)].buildingIcon;
        T2_2_Name.text = buildingInformation[((int)BuildingID.Tier2_2)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier2_2)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier2_2)].Equals(CityBuildingState.Built)){
                T2_2_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier2_2))){
                    T2_2_StatusImage.sprite = buildingReadyToCreate;
                    T2_2_Button.interactable = true;
                }
                else T2_2_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T2_2_StatusImage.sprite = buildingNotAvailable;
            T2_2_Button.interactable = false;
        }
        // Tier 3.1
        T3_1_Image.sprite = buildingInformation[((int)BuildingID.Tier3_1)].buildingIcon;
        T3_1_Name.text = buildingInformation[((int)BuildingID.Tier3_1)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier3_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier3_1)].Equals(CityBuildingState.Built)){
                T3_1_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier3_1))){
                    T3_1_StatusImage.sprite = buildingReadyToCreate;
                    T3_1_Button.interactable = true;
                }
                else T3_1_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T3_1_StatusImage.sprite = buildingNotAvailable;
            T3_1_Button.interactable = false;
        }
        // Tier 3.2
        T3_2_Image.sprite = buildingInformation[((int)BuildingID.Tier3_2)].buildingIcon;
        T3_2_Name.text = buildingInformation[((int)BuildingID.Tier3_2)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier3_2)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier3_2)].Equals(CityBuildingState.Built)){
                T3_2_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier3_2))){
                    T3_2_StatusImage.sprite = buildingReadyToCreate;
                    T3_2_Button.interactable = true;
                }
                else T3_2_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T3_2_StatusImage.sprite = buildingNotAvailable;
            T3_2_Button.interactable = false;
        }
        // Tier 4.1
        T4_1_Image.sprite = buildingInformation[((int)BuildingID.Tier4_1)].buildingIcon;
        T4_1_Name.text = buildingInformation[((int)BuildingID.Tier4_1)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier4_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier4_1)].Equals(CityBuildingState.Built)){
                T4_1_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier4_1))){
                    T4_1_StatusImage.sprite = buildingReadyToCreate;
                    T4_1_Button.interactable = true;
                }
                else T4_1_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T4_1_StatusImage.sprite = buildingNotAvailable;
            T4_1_Button.interactable = false;
        }
        // Tier 4.2
        T4_2_Image.sprite = buildingInformation[((int)BuildingID.Tier4_2)].buildingIcon;
        T4_2_Name.text = buildingInformation[((int)BuildingID.Tier4_2)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.Tier4_2)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Tier4_2)].Equals(CityBuildingState.Built)){
                T4_2_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Tier4_2))){
                    T4_2_StatusImage.sprite = buildingReadyToCreate;
                    T4_2_Button.interactable = true;
                }
                else T4_2_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            T4_2_StatusImage.sprite = buildingNotAvailable;
            T4_2_Button.interactable = false;
        }
        // MAGIC HALL
        magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_1)].buildingIcon;
        magicGuildStatus = 0;
        magicGuildName.text = "Magic Guild lv. 1";
        if (!currentCity.cityBuildings[((int)BuildingID.MagicHall_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.MagicHall_1)].Equals(CityBuildingState.Built)){
                magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_2)].buildingIcon;
                magicGuildName.text = "Magic Guild lv. 2";
                magicGuildStatus = ((int)BuildingID.MagicHall_1);
                if (!currentCity.cityBuildings[((int)BuildingID.MagicHall_2)].Equals(CityBuildingState.Blocked)){
                    if (currentCity.cityBuildings[((int)BuildingID.MagicHall_2)].Equals(CityBuildingState.Built)){
                        magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_3)].buildingIcon;
                        magicGuildName.text = "Magic Guild lv. 3";
                        magicGuildStatus = ((int)BuildingID.MagicHall_2);
                        if (!currentCity.cityBuildings[((int)BuildingID.MagicHall_3)].Equals(CityBuildingState.Blocked)){
                            if (currentCity.cityBuildings[((int)BuildingID.MagicHall_3)].Equals(CityBuildingState.Built)){
                                magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_4)].buildingIcon;
                                magicGuildName.text = "Magic Guild lv. 4";
                                magicGuildStatus = ((int)BuildingID.MagicHall_3);
                                if (!currentCity.cityBuildings[((int)BuildingID.MagicHall_4)].Equals(CityBuildingState.Blocked)){
                                    if (currentCity.cityBuildings[((int)BuildingID.MagicHall_4)].Equals(CityBuildingState.Built)){
                                        magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_5)].buildingIcon;
                                        magicGuildName.text = "Magic Guild lv. 5";
                                        magicGuildStatus = ((int)BuildingID.MagicHall_4);
                                        if (!currentCity.cityBuildings[((int)BuildingID.MagicHall_5)].Equals(CityBuildingState.Blocked)){
                                            if (currentCity.cityBuildings[((int)BuildingID.MagicHall_5)].Equals(CityBuildingState.Built)){
                                                magicGuildStatusImage.sprite = buildingMaxed;
                                                magicGuildStatus = ((int)BuildingID.MagicHall_5);
                                            }else{
                                                if (CheckBuildingRequirements(((int)BuildingID.MagicHall_5))){
                                                    magicGuildStatusImage.sprite = buildingReadyToCreate;
                                                    magicGuildButton.interactable = true;
                                                }
                                                else magicGuildStatusImage.sprite = buildingNotReadyToCreate;
                                            }       
                                        }else{
                                            magicGuildStatusImage.sprite = buildingNotAvailable;
                                            magicGuildButton.interactable = false;
                                        }
                                    }else{
                                        if (CheckBuildingRequirements(((int)BuildingID.MagicHall_4))){
                                            magicGuildStatusImage.sprite = buildingReadyToCreate;
                                            magicGuildButton.interactable = true;
                                        }
                                        else magicGuildStatusImage.sprite = buildingNotReadyToCreate;
                                    }
                                }else{
                                    magicGuildStatusImage.sprite = buildingNotAvailable;
                                    magicGuildButton.interactable = false;
                                }
                            }else{
                                if (CheckBuildingRequirements(((int)BuildingID.MagicHall_3))){
                                    magicGuildStatusImage.sprite = buildingReadyToCreate;
                                    magicGuildButton.interactable = true;
                                }
                                else magicGuildStatusImage.sprite = buildingNotReadyToCreate;
                            }       
                        }else{
                            magicGuildStatusImage.sprite = buildingNotAvailable;
                            magicGuildButton.interactable = false;
                        }
                    
                    }else{
                        if (CheckBuildingRequirements(((int)BuildingID.MagicHall_2))){
                            magicGuildStatusImage.sprite = buildingReadyToCreate;
                            magicGuildButton.interactable = true;
                        }
                        else magicGuildStatusImage.sprite = buildingNotReadyToCreate;
                    }
                }else{
                    magicGuildStatusImage.sprite = buildingNotAvailable;
                    magicGuildButton.interactable = false;
                }
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.MagicHall_1))){
                    magicGuildStatusImage.sprite = buildingReadyToCreate;
                    magicGuildButton.interactable = true;
                }
                else magicGuildStatusImage.sprite = buildingNotReadyToCreate;
            }
        }else{
            magicGuildStatusImage.sprite = buildingNotAvailable;
            magicGuildButton.interactable = false;
        }
        
        // ADDITIONAL MAGIC 1
        additionalMagic_1_Image.sprite = buildingInformation[((int)BuildingID.AdditionalMagic_1)].buildingIcon;
        additionalMagic_1_Name.text = buildingInformation[((int)BuildingID.AdditionalMagic_1)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_1)].Equals(CityBuildingState.Built)){
                additionalMagic_1_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.AdditionalMagic_1))){
                    additionalMagic_1_StatusImage.sprite = buildingReadyToCreate;
                    additionalMagic_1_Button.interactable = true;
                }
                else additionalMagic_1_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            additionalMagic_1_StatusImage.sprite = buildingNotAvailable;
            additionalMagic_1_Button.interactable = false;
        }

        // ADDITIONAL MAGIC 2
        additionalMagic_2_Image.sprite = buildingInformation[((int)BuildingID.AdditionalMagic_2)].buildingIcon;
        additionalMagic_2_Name.text = buildingInformation[((int)BuildingID.AdditionalMagic_2)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_2)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_2)].Equals(CityBuildingState.Built)){
                additionalMagic_2_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.AdditionalMagic_2))){
                    additionalMagic_2_StatusImage.sprite = buildingReadyToCreate;
                    additionalMagic_2_Button.interactable = true;
                }
                else additionalMagic_2_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            additionalMagic_2_StatusImage.sprite = buildingNotAvailable;
            additionalMagic_2_Button.interactable = false;
        }

        // RACIAL BUILDING 
        racialBuildingImage.sprite = buildingInformation[((int)BuildingID.RacialBuilding)].buildingIcon;
        racialBuildingName.text = buildingInformation[((int)BuildingID.RacialBuilding)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.RacialBuilding)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.RacialBuilding)].Equals(CityBuildingState.Built)){
                racialBuildingStatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.RacialBuilding))){
                    racialBuildingStatusImage.sprite = buildingReadyToCreate;
                    racialBuildingButton.interactable = true;
                }
                else racialBuildingStatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            racialBuildingStatusImage.sprite = buildingNotAvailable;
            racialBuildingButton.interactable = false;
        }

        // CARAVAN 
        caravanImage.sprite = buildingInformation[((int)BuildingID.Caravan)].buildingIcon;
        if (!currentCity.cityBuildings[((int)BuildingID.Caravan)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Caravan)].Equals(CityBuildingState.Built)){
                caravanStatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Caravan))){ 
                    caravanStatusImage.sprite = buildingReadyToCreate;
                    caravanButton.interactable = true;
                }
                else caravanStatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            caravanStatusImage.sprite = buildingNotAvailable;
            caravanButton.interactable = false;
        }

        // SHIPYARD
        shipyardImage.sprite = buildingInformation[((int)BuildingID.Shipyard)].buildingIcon;
        if (!currentCity.cityBuildings[((int)BuildingID.Shipyard)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.Shipyard)].Equals(CityBuildingState.Built)){
                shipyardStatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.Shipyard))){
                    shipyardStatusImage.sprite = buildingReadyToCreate;
                    shipyardButton.interactable = true;
                }
                else shipyardStatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            shipyardStatusImage.sprite = buildingNotAvailable;
            shipyardButton.interactable = false;
        }

        // BONUS BUILDING 1
        bonusBuilding_1_Image.sprite = buildingInformation[((int)BuildingID.BonusBuilding_1)].buildingIcon;
        bonusBuilding_1_Name.text = buildingInformation[((int)BuildingID.BonusBuilding_1)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.BonusBuilding_1)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.BonusBuilding_1)].Equals(CityBuildingState.Built)){
                bonusBuilding_1_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.BonusBuilding_1))){
                    bonusBuilding_1_StatusImage.sprite = buildingReadyToCreate;
                    bonusBuilding_1_Button.interactable = true;
                }
                else bonusBuilding_1_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            bonusBuilding_1_StatusImage.sprite = buildingNotAvailable;
            bonusBuilding_1_Button.interactable = false;
        }

        // BONUS BUILDING 2
        bonusBuilding_2_Image.sprite = buildingInformation[((int)BuildingID.BonusBuilding_2)].buildingIcon;
        bonusBuilding_2_Name.text = buildingInformation[((int)BuildingID.BonusBuilding_2)].buildingName;
        if (!currentCity.cityBuildings[((int)BuildingID.BonusBuilding_2)].Equals(CityBuildingState.Blocked)){
            if (currentCity.cityBuildings[((int)BuildingID.BonusBuilding_2)].Equals(CityBuildingState.Built)){
                bonusBuilding_2_StatusImage.sprite = buildingMaxed;
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.BonusBuilding_2))){
                    bonusBuilding_2_StatusImage.sprite = buildingReadyToCreate;
                    bonusBuilding_2_Button.interactable = true;
                }
                else bonusBuilding_2_StatusImage.sprite = buildingNotReadyToCreate;
            }       
        }else{
            bonusBuilding_2_StatusImage.sprite = buildingNotAvailable;
            bonusBuilding_2_Button.interactable = false;
        }
    }

    public void OpenBuildingUI (int buildingID)
    {
        string currentBuilding = Enum.GetName(typeof (BuildingID), buildingID);
        switch (currentBuilding)
        {
            case "VillageHall":
                if (hallStatus == 0){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "TownHall":
                if (hallStatus == ((int)BuildingID.VillageHall)){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "CityHall":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tavern":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Prison":
                
            break;

            case "Fort":
                if (fortStatus == 0){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "Citadel":
                if (fortStatus == ((int)BuildingID.Fort)){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "Castle":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Caravan":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Shipyard":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "BonusBuilding_1":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "BonusBuilding_2":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "EquipementBuilding":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "RacialBuilding":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "GraalBuilding":
                
            break;

            case "MagicHall_1":
                if (magicGuildStatus == 0){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "MagicHall_2":
                if (magicGuildStatus == ((int)BuildingID.MagicHall_1)){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "MagicHall_3":
                if (magicGuildStatus == ((int)BuildingID.MagicHall_2)){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "MagicHall_4":
                if (magicGuildStatus == ((int)BuildingID.MagicHall_3)){
                    buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
                }else{
                    OpenBuildingUI(buildingID + 1);
                }
            break;

            case "MagicHall_5":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "AdditionalMagic_1":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "AdditionalMagic_2":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier1_1":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier1_2":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier2_1":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier2_2":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier3_1":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier3_2":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier4_1":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;

            case "Tier4_2":
                buildingCostDisplay.UpdateDisplay(buildingID, buildingInformation[buildingID], CheckBuildingRequirements(buildingID));
            break;
        }
    }

    private bool CheckBuildingRequirements (int buildingID)
    {
        if (CityManager.Instance.currentCity.cityBuildingAlreadybuilt) return false;
        if (buildingInformation[buildingID].additionalRequirements.Count > 0){
            for (int i = 0; i < buildingInformation[buildingID].additionalRequirements.Count; i++){
                if (currentCity.cityBuildings[Convert.ToInt32(buildingInformation[buildingID].additionalRequirements[i])] == 0) return false;
            }
        }
        if (owningPlayer.gold < buildingInformation[buildingID].goldCost)           return false;
        if (owningPlayer.wood < buildingInformation[buildingID].woodCost)           return false;
        if (owningPlayer.ore < buildingInformation[buildingID].oreCost)             return false;
        if (owningPlayer.gems < buildingInformation[buildingID].gemCost)            return false;
        if (owningPlayer.mercury < buildingInformation[buildingID].mercuryCost)     return false;
        if (owningPlayer.sulfur < buildingInformation[buildingID].sulfurCost)       return false;
        if (owningPlayer.crystals < buildingInformation[buildingID].crystalCost)    return false;
        return true;
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