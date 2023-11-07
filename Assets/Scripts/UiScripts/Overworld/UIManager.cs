using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance;

    [Header ("Resources UI")]
    [SerializeField] private ResourceDisplay resourceDisplay;

    [Header ("Current Army Information UI")]
    [SerializeField] private ArmyDisplayInformation armyInformation;

    [Header ("Army Interface UI")]
    [SerializeField] private ArmyInterface armyInterface;

    [Header ("Army and Town Display UI")]
    [SerializeField] private OwnedTownsDisplay townDisplay;
    [SerializeField] private OwnedArmiesDisplay armyDisplay;

    [Header ("Unit Split Window")]
    [SerializeField] private UnitSplitWindow unitSplitWindow;

    [Header ("Other UI Elements")]
    [SerializeField] private GameObject backgroundDim;

    private void Awake (){
        Instance = this;
    }

    public void SetupUIManager (){
        TurnManager.Instance.OnNewDay += NewDayUpdate;
        TurnManager.Instance.OnNewTurn += NewTurnUpdate;
    }

    public void StartGame (){
        NewTurnUpdate();
    }

    private void NewTurnUpdate (){
        UpdateResourceDisplay();
        UpdateCurrentArmyDisplay();
        UpdateCurrentCityDisplay();
        RefreshCurrentArmyDisplay();
    }

    private void NewDayUpdate (){

    }

    public void UpdateResourceDisplay (){
        resourceDisplay.UpdateDisplay();
    }

    public void UpdatePlayerDisplay (){
        UpdateCurrentArmyDisplay();
        UpdateCurrentCityDisplay();
    }

    #region TownAndArmyDisplay

    public void UpdateCurrentArmyDisplay (){
        Debug.Log("Current army display updated.");
        armyDisplay.UpdateArmyDisplay();
    }

    public void UpdateCurrentCityDisplay (){
        Debug.Log("Current city display updated.");
        townDisplay.UpdateCityDisplay();
    }

    #endregion

    #region ArmyDisplay

    public void RefreshCurrentArmyDisplay (){
        armyInformation.RefreshElement();
    }

    #endregion

    #region ArmyInterface
    
    public void UpdateArmyInterface (Army armyObject, Army interactedArmy = null){
        armyInterface.gameObject.SetActive(true);
        armyInterface.ArmyInterfaceSetup(armyObject, interactedArmy);
        
        ChangeBackgroundDimStatus(true);

        CameraManager.Instance.DisableCamera();
        InputManager.Instance.keyboardInput.onEscPressed.AddListener(DisableArmyInterface);
        Debug.Log("Updated army interface.");
    }

    public void DisableArmyInterface ()
    {
        armyInterface.ResetElement();
        armyInterface.gameObject.SetActive(false);
        ChangeBackgroundDimStatus(false);

        CameraManager.Instance.EnableCamera();
        InputManager.Instance.keyboardInput.onEscPressed.RemoveListener(DisableArmyInterface);
        UpdateCurrentArmyDisplay();
        Debug.Log("Disabled the army interface.");
    }

    public void RefreshArmyInterface (){
        armyInterface.RefreshElement();
    }

    #endregion

    #region UnitSplitWindow

    public void OpenUnitSplitWindow (UnitsInformation uInfo01, byte id01, byte id02, UnitsInformation uInfo02 = null)
    {
        unitSplitWindow.PrepareUnitsToSwap(uInfo01, id01, id02, uInfo02);
    }

    public void EndUnitSplit (){
        if (GameManager.Instance.IsSceneOpened()){
            CityManager.Instance.RefreshArmyInterface();
        }else{  
            RefreshCurrentArmyDisplay();
            RefreshArmyInterface();
        }
    }

    public void DisableUnitSplitWindow (){
        unitSplitWindow.DisableUnitSplitWindow();
    }

    public void FinalizeUnitSplit (){
        RefreshCurrentArmyDisplay();
        RefreshArmyInterface();
    }

    #endregion

    public void ChangeBackgroundDimStatus(bool status){
        backgroundDim.SetActive(status);
    }
}
