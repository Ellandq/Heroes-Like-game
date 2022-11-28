using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TownHallUI : MonoBehaviour
{
    private Player owningPlayer;
    private City currentCity;
    [SerializeField] BuildingCostDisplay buildingCostDisplay;
    [SerializeField] private List <BuildingInformationObject> buildingInformation;
    
    [Header ("Building Status Sprites")]
    [SerializeField] Sprite buildingMaxed;
    [SerializeField] Sprite buildingReadyToCreate;
    [SerializeField] Sprite buildingNotReadyToCreate;
    [SerializeField] Sprite buildingNotAvailable;

    [Header ("Hall")]
    [SerializeField] Button hallButton;
    [SerializeField] Image hallImage;
    [SerializeField] TextMeshProUGUI hallName;
    [SerializeField] Image hallStatusImage;
    private int hallStatus;

    [Header ("Fort")]
    [SerializeField] Button fortButton;
    [SerializeField] Image fortImage;
    [SerializeField] TextMeshProUGUI fortName;
    [SerializeField] Image fortStatusImage;
    private int fortStatus;

    [Header ("Magic guild")]
    [SerializeField] Button magicGuildButton;
    [SerializeField] Image magicGuildImage;
    [SerializeField] TextMeshProUGUI magicGuildName;
    [SerializeField] Image magicGuildStatusImage;
    private int magicGuildStatus;

    [Header ("Tavern")]
    [SerializeField] Button tavernButton;
    [SerializeField] TextMeshProUGUI tavernName;
    [SerializeField] Image tavernStatusImage;
    [SerializeField] Image tavernImage;

    [Header ("Equipement")]
    [SerializeField] Button equipementButton;
    [SerializeField] TextMeshProUGUI equipementName;
    [SerializeField] Image equipementStatusImage;
    [SerializeField] Image equipementImage;

    [Header ("Unit buildings")]
    [SerializeField] Button T1_1_Button;
    [SerializeField] TextMeshProUGUI T1_1_Name;
    [SerializeField] Image T1_1_StatusImage;
    [SerializeField] Image T1_1_Image;

    [SerializeField] Button T1_2_Button;
    [SerializeField] TextMeshProUGUI T1_2_Name;
    [SerializeField] Image T1_2_StatusImage;
    [SerializeField] Image T1_2_Image;

    [SerializeField] Button T2_1_Button;
    [SerializeField] TextMeshProUGUI T2_1_Name;
    [SerializeField] Image T2_1_StatusImage;
    [SerializeField] Image T2_1_Image;

    [SerializeField] Button T2_2_Button;
    [SerializeField] TextMeshProUGUI T2_2_Name;
    [SerializeField] Image T2_2_StatusImage;
    [SerializeField] Image T2_2_Image;

    [SerializeField] Button T3_1_Button;
    [SerializeField] TextMeshProUGUI T3_1_Name;
    [SerializeField] Image T3_1_StatusImage;
    [SerializeField] Image T3_1_Image;

    [SerializeField] Button T3_2_Button;
    [SerializeField] TextMeshProUGUI T3_2_Name;
    [SerializeField] Image T3_2_StatusImage;
    [SerializeField] Image T3_2_Image;

    [SerializeField] Button T4_1_Button;
    [SerializeField] TextMeshProUGUI T4_1_Name;
    [SerializeField] Image T4_1_StatusImage;
    [SerializeField] Image T4_1_Image;

    [SerializeField] Button T4_2_Button;
    [SerializeField] TextMeshProUGUI T4_2_Name;
    [SerializeField] Image T4_2_StatusImage;
    [SerializeField] Image T4_2_Image;

    [Header ("Additional Magic Buildings")]
    [SerializeField] Button additionalMagic_1_Button;
    [SerializeField] TextMeshProUGUI additionalMagic_1_Name;
    [SerializeField] Image additionalMagic_1_StatusImage;
    [SerializeField] Image additionalMagic_1_Image;

    [SerializeField] Button additionalMagic_2_Button;
    [SerializeField] TextMeshProUGUI additionalMagic_2_Name;
    [SerializeField] Image additionalMagic_2_StatusImage;
    [SerializeField] Image additionalMagic_2_Image;

    [Header ("Racial Building")]
    [SerializeField] Button racialBuildingButton;
    [SerializeField] TextMeshProUGUI racialBuildingName;
    [SerializeField] Image racialBuildingStatusImage;
    [SerializeField] Image racialBuildingImage;

    [Header ("Caravan Building")]
    [SerializeField] Button caravanButton;
    [SerializeField] TextMeshProUGUI caravanName;
    [SerializeField] Image caravanStatusImage;
    [SerializeField] Image caravanImage;

    [Header ("Shipyard Building")]
    [SerializeField] Button shipyardButton;
    [SerializeField] TextMeshProUGUI shipyardName;
    [SerializeField] Image shipyardStatusImage;
    [SerializeField] Image shipyardImage;

    [Header ("Bonus Buildings Building")]
    [SerializeField] Button bonusBuilding_1_Button;
    [SerializeField] TextMeshProUGUI bonusBuilding_1_Name;
    [SerializeField] Image bonusBuilding_1_StatusImage;
    [SerializeField] Image bonusBuilding_1_Image;

    [SerializeField] Button bonusBuilding_2_Button;
    [SerializeField] TextMeshProUGUI bonusBuilding_2_Name;
    [SerializeField] Image bonusBuilding_2_StatusImage;
    [SerializeField] Image bonusBuilding_2_Image;

    public void ActivateElement ()
    {
        currentCity = CityManager.Instance.currentCity;
        owningPlayer = currentCity.ownedByPlayer.GetComponent<Player>();
        RefreshElement();
        this.gameObject.SetActive(true);
    }

    public void DisableElement ()
    {
        this.gameObject.SetActive(false);
    }

    private void RefreshElement ()
    {
        // HALL
        hallImage.sprite = buildingInformation[((int)BuildingID.TownHall)].buildingIcon;
        hallName.text = "Town Hall";
        hallStatus = ((int)BuildingID.VillageHall);
        if (currentCity.cityBuildings[((int)BuildingID.TownHall)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.TownHall)] == 1){
                hallImage.sprite = buildingInformation[((int)BuildingID.CityHall)].buildingIcon;
                hallName.text = "City Hall";
                hallStatus = ((int)BuildingID.TownHall);
                if (currentCity.cityBuildings[((int)BuildingID.CityHall)] != 2){
                    if (currentCity.cityBuildings[((int)BuildingID.CityHall)] == 1){
                    hallStatus = ((int)BuildingID.CityHall);
                    hallStatusImage.sprite = buildingMaxed;
                    }else{
                        if (CheckBuildingRequirements(((int)BuildingID.CityHall))){
                            hallStatusImage.sprite = buildingReadyToCreate;
                            hallButton.interactable = true;
                        }
                        else hallStatusImage.sprite = buildingNotReadyToCreate;
                    }
                }else{
                    hallStatusImage.sprite = buildingNotAvailable;
                    hallButton.interactable = false;
                }
            }else{
                if (CheckBuildingRequirements(((int)BuildingID.TownHall))){
                    hallStatusImage.sprite = buildingReadyToCreate;
                    hallButton.interactable = true;
                }
                else hallStatusImage.sprite = buildingNotReadyToCreate;
            }
        }else{
            hallStatusImage.sprite = buildingNotAvailable;
            hallButton.interactable = false;
        }
        // FORT
        fortImage.sprite = buildingInformation[((int)BuildingID.Fort)].buildingIcon;
        fortName.text = "Fort";
        fortStatus = 0;
        if (currentCity.cityBuildings[((int)BuildingID.Fort)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Fort)] == 1){
                fortImage.sprite = buildingInformation[((int)BuildingID.Citadel)].buildingIcon;
                fortName.text = "Citadel";
                fortStatus = ((int)BuildingID.Fort);
                if (currentCity.cityBuildings[((int)BuildingID.Citadel)] != 2){
                    if (currentCity.cityBuildings[((int)BuildingID.Citadel)] == 1){
                        fortImage.sprite = buildingInformation[((int)BuildingID.Castle)].buildingIcon;
                        fortName.text = "Castle";
                        fortStatus = ((int)BuildingID.Citadel);
                        if (currentCity.cityBuildings[((int)BuildingID.Castle)] != 2){
                            if (currentCity.cityBuildings[((int)BuildingID.Castle)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tavern)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tavern)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.EquipementBuilding)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.EquipementBuilding)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier1_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier1_1)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier1_2)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier1_2)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier2_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier2_1)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier2_2)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier2_2)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier3_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier3_1)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier3_2)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier3_2)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier4_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier4_1)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Tier4_2)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Tier4_2)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.MagicHall_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.MagicHall_1)] == 1){
                magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_2)].buildingIcon;
                magicGuildName.text = "Magic Guild lv. 2";
                magicGuildStatus = ((int)BuildingID.MagicHall_1);
                if (currentCity.cityBuildings[((int)BuildingID.MagicHall_2)] != 2){
                    if (currentCity.cityBuildings[((int)BuildingID.MagicHall_2)] == 1){
                        magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_3)].buildingIcon;
                        magicGuildName.text = "Magic Guild lv. 3";
                        magicGuildStatus = ((int)BuildingID.MagicHall_2);
                        if (currentCity.cityBuildings[((int)BuildingID.MagicHall_3)] != 2){
                            if (currentCity.cityBuildings[((int)BuildingID.MagicHall_3)] == 1){
                                magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_4)].buildingIcon;
                                magicGuildName.text = "Magic Guild lv. 4";
                                magicGuildStatus = ((int)BuildingID.MagicHall_3);
                                if (currentCity.cityBuildings[((int)BuildingID.MagicHall_4)] != 2){
                                    if (currentCity.cityBuildings[((int)BuildingID.MagicHall_4)] == 1){
                                        magicGuildImage.sprite = buildingInformation[((int)BuildingID.MagicHall_5)].buildingIcon;
                                        magicGuildName.text = "Magic Guild lv. 5";
                                        magicGuildStatus = ((int)BuildingID.MagicHall_4);
                                        if (currentCity.cityBuildings[((int)BuildingID.MagicHall_5)] != 2){
                                            if (currentCity.cityBuildings[((int)BuildingID.MagicHall_5)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_1)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_2)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.AdditionalMagic_2)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.RacialBuilding)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.RacialBuilding)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Caravan)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Caravan)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.Shipyard)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.Shipyard)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.BonusBuilding_1)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.BonusBuilding_1)] == 1){
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
        if (currentCity.cityBuildings[((int)BuildingID.BonusBuilding_2)] != 2){
            if (currentCity.cityBuildings[((int)BuildingID.BonusBuilding_2)] == 1){
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