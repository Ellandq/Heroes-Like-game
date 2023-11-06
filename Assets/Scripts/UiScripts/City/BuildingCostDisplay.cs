using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingCostDisplay : MonoBehaviour
{
    [SerializeField] private CityResourceDisplay cityResourceDisplay;
    private BuildingInformationObject buildingInformation;

    [Header ("UI References")]
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Image currentBuildingImage;
    [SerializeField] private TextMeshProUGUI currentBuildingName;
    [SerializeField] private List<TextMeshProUGUI> buildingCost; 

    public void UpdateDisplay (BuildingInformationObject buildingInformation, bool buildingRequirementsMet)
    {
        this.buildingInformation = buildingInformation;
        currentBuildingImage.sprite = buildingInformation.buildingIcon;
        currentBuildingName.text = buildingInformation.buildingName;
        
        if (buildingRequirementsMet) confirmButton.interactable = true;
        else confirmButton.interactable = false;

        transform.parent.gameObject.SetActive(true);

        for (int i = 0; i < 7; i++){
            buildingCost[i].text = Convert.ToString(buildingInformation.cost[i]);
        } 
    }

    public void DisableElement ()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void CreateBuilding ()
    {
        CityManager.Instance.GetCity().GetBuildingHandler().CreateNewBuilding(buildingInformation.buildingID);
        PlayerManager.Instance.GetCurrentPlayer().RemoveResources(buildingInformation.cost);
        DisableElement();
        transform.parent.parent.gameObject.SetActive(false);
        cityResourceDisplay.UpdateDisplay();
        CityManager.Instance.onNewBuildingCreated.Invoke(buildingInformation.buildingID);
        DwellingUI.Instance.UpdateDwellingDisplay();
    }
}
