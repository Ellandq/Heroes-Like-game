using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingCostDisplay : MonoBehaviour
{
    [SerializeField] CityResourceDisplay cityResourceDisplay;
    int currentBuildingID;
    int[] currentBuildingResourceCost;
    [SerializeField] Button cancelButton;
    [SerializeField] Button confirmButton;

    [SerializeField] Image currentBuildingImage;
    [SerializeField] TextMeshProUGUI currentBuildingName;

    [SerializeField] GameObject goldCounter;
    [SerializeField] GameObject woodCounter;
    [SerializeField] GameObject oreCounter;
    [SerializeField] GameObject gemCounter;
    [SerializeField] GameObject mercuryCounter;
    [SerializeField] GameObject sulfurCounter;
    [SerializeField] GameObject crystalCounter;

    private TextMeshProUGUI displayedGold;
    private TextMeshProUGUI displayedWood;
    private TextMeshProUGUI displayedOre;
    private TextMeshProUGUI displayedGems;
    private TextMeshProUGUI displayedMercury;
    private TextMeshProUGUI displayedSulfur;
    private TextMeshProUGUI displayedCrystals;

    private bool isResourceDisplayReady = false;

    public void SetupResourceDisplay ()
    {
        displayedGold = goldCounter.GetComponent<TextMeshProUGUI>();
        displayedWood = woodCounter.GetComponent<TextMeshProUGUI>();
        displayedOre = oreCounter.GetComponent<TextMeshProUGUI>();
        displayedGems = gemCounter.GetComponent<TextMeshProUGUI>();
        displayedMercury = mercuryCounter.GetComponent<TextMeshProUGUI>();
        displayedSulfur = sulfurCounter.GetComponent<TextMeshProUGUI>();
        displayedCrystals = crystalCounter.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDisplay (int buildingID, BuildingInformationObject buildingInformation, bool buildingRequirementsMet)
    {
        currentBuildingImage.sprite = buildingInformation.buildingIcon;
        currentBuildingName.text = buildingInformation.buildingName;
        currentBuildingID = buildingID;
        
        if (buildingRequirementsMet) confirmButton.interactable = true;
        else confirmButton.interactable = false;
        if (!isResourceDisplayReady) SetupResourceDisplay();
        this.transform.parent.gameObject.SetActive(true);
        displayedGold.text = Convert.ToString(buildingInformation.goldCost);
        displayedWood.text = Convert.ToString(buildingInformation.woodCost);
        displayedOre.text = Convert.ToString(buildingInformation.oreCost);
        displayedGems.text = Convert.ToString(buildingInformation.gemCost);
        displayedMercury.text = Convert.ToString(buildingInformation.mercuryCost);
        displayedSulfur.text = Convert.ToString(buildingInformation.sulfurCost);
        displayedCrystals.text = Convert.ToString(buildingInformation.crystalCost);  

        Array.Resize(ref currentBuildingResourceCost, 7);
        currentBuildingResourceCost[0] = buildingInformation.goldCost;
        currentBuildingResourceCost[1] = buildingInformation.woodCost;
        currentBuildingResourceCost[2] = buildingInformation.oreCost;
        currentBuildingResourceCost[3] = buildingInformation.gemCost;
        currentBuildingResourceCost[4] = buildingInformation.mercuryCost;
        currentBuildingResourceCost[5] = buildingInformation.sulfurCost;
        currentBuildingResourceCost[6] = buildingInformation.crystalCost;     
    }

    public void DisableElement ()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void CreateBuilding ()
    {
        CityManager.Instance.currentCity.CreateNewBuilding(currentBuildingID, currentBuildingResourceCost);
        CityManager.Instance.owningPlayer.RemoveResources(currentBuildingResourceCost);
        DisableElement();
        transform.parent.parent.gameObject.SetActive(false);
        cityResourceDisplay.UpdateDisplay();
        CityManager.Instance.onNewBuildingCreated.Invoke(currentBuildingID);
        DwellingUI.Instance.UpdateDwellingDisplay();
    }
}
