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
    [SerializeField] private ArmyInterfaceArmyInformation armyInterface;

    [Header ("Army and Town Display UI")]
    [SerializeField] private OwnedTownsDisplay townDisplay;
    [SerializeField] private OwnedArmiesDisplay armyDisplay;

    [Header ("Unit Split Window")]
    [SerializeField] private UnitSplitWindow unitSplitWindow;

    [Header ("Other UI Elements")]
    [SerializeField] private GameObject backgroundDim;

    private void Awake ()
    {
        Instance = this;
        TurnManager.Instance.OnNewDay += NewDayUpdate;
        TurnManager.Instance.OnNewTurn += NewTurnUpdate;
    }

    private void NewTurnUpdate (){
        UpdateResourceDisplay();
    }

    private void NewDayUpdate (){

    }

    public void UpdateResourceDisplay (){
        resourceDisplay.UpdateDisplay();
    }














    #region TownAndArmyDisplay

    // Updates the Player display
    public void UpdatePlayerDisplay ()
    {
        townDisplay.UpdateCityDisplay();
        armyDisplay.UpdateArmyDisplay();
    }

    public void UpdateCurrentArmyDisplay (){
        Debug.Log("Current army display updated.");
        armyDisplay.UpdateArmyDisplay(currentPlayer);
    }

    public void UpdateCurrentCityDisplay (){
        Debug.Log("Current city display updated.");
        townDisplay.UpdateCityDisplay(currentPlayer);
    }

    #endregion

    #region CurrentArmyDisplay

    public void RefreshCurrentArmyDisplay (){
        armyInformation.RefreshElement();
    }

    #endregion

    #region ArmyInterface
    
    public void UpdateArmyInterface (GameObject armyObject, GameObject interactedArmy = null){
        armyInterface.gameObject.SetActive(true);
        if (interactedArmy == null){
            armyInterface.ArmyInterfaceSetup(armyObject);
        }else{
            armyInterface.ArmyInterfaceSetup(armyObject, interactedArmy);
        }
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

    public void OpenUnitSplitWindow (UnitSlot unit01, UnitSlot unit02, ArmyInformation connectedArmy, short id01, short id02)
    {
        unitSplitWindow.PrepareUnitsToSwap(unit01, unit02, connectedArmy, id01, id02);
    }

    public void OpenUnitSplitWindow (UnitSlot unit01, UnitSlot unit02, UnitsInformation connectedArmy, UnitsInformation armyInteractedWith, short id01, short id02)
    {
        unitSplitWindow.PrepareUnitsToSwap(unit01, unit02, connectedArmy, armyInteractedWith, id01, id02);
    }

    public void DisableUnitSplitWindow ()
    {
        unitSplitWindow.DisableUnitSplitWindow();
    }

    public void FinalizeUnitSplit ()
    {

    }

    #endregion

    public void ChangeBackgroundDimStatus(bool status){
        backgroundDim.SetActive(status);
    }
}
