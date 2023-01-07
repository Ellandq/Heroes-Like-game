using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class ArmyInterfaceArmyInformation : MonoBehaviour
{
    // Singleton instance of this script
    public static ArmyInterfaceArmyInformation Instance;

    // Lists of units for the two armies being displayed in the interface
    [SerializeField] List <GameObject> units01;
    [SerializeField] List <GameObject> units02;

    // Default background image for the interface
    [SerializeField] Sprite defaultBackground;

    // Placeholder army used when interacting with an empty cell on the game grid
    [SerializeField] GameObject placeHolderArmy;

    // Currently selected unit in the interface
    public UnitSlot selectedUnit;

    // UI elements for the unit information display
    [Header("UI Referances")]
    [SerializeField] List <GameObject> unitInfoSlot;
    [SerializeField] List <Button> unitInfoButtons;
    [SerializeField] List <GameObject> unitCountDisplay;

    // GameObjects for the two armies being displayed in the interface
    [SerializeField] internal GameObject army01;
    [SerializeField] internal GameObject army02;

    // Flag to indicate whether the placeholder army is being used
    private bool interactingWithPlaceholder;

    // List of empty cells surrounding the active army
    private List <GridCell> neighbourCells;

    // Event that is triggered when the interface is reloaded
    public UnityEvent onArmyInterfaceReload;

    // Initialization function
    private void Awake ()
    {
        // Set the singleton instance
        Instance = this;

        // Initialize selectedUnit to null
        selectedUnit = null;
    }

    // Function to set up the interface for the given army
    public void GetArmyUnits(GameObject armyObject)
    {
        // Disable camera movement
        CameraManager.Instance.cameraEnabled = false;

        // Set interactingWithPlaceholder flag
        interactingWithPlaceholder = true;

        // Set the two armies being displayed in the interface
        army01 = armyObject;
        army02 = placeHolderArmy;

        // Get the units for each army
        units01 = new List<GameObject>(armyObject.GetComponentInParent<UnitsInformation>().unitSlots);
        units02 = new List<GameObject>(placeHolderArmy.GetComponent<UnitsInformation>().unitSlots);

        // Enable the interface gameObject
        this.transform.GetChild(0).gameObject.SetActive(true);

        // Update the display of units in the interface
        UpdateUnitDisplay();

        // Get the empty cells surrounding the active army
        neighbourCells = GameGrid.Instance.GetEmptyNeighbourCell(army01.GetComponent<Army>().gridPosition);
    }

    // Function to set up the interface for the given army and the army it is interacting with
    public void GetArmyUnits(GameObject armyObject, GameObject interactedArmy)
    {
        // Disable camera movement
        CameraManager.Instance.cameraEnabled = false;

        // Clear interactingWithPlaceholder flag
        interactingWithPlaceholder = false;

        // Set the two armies being displayed in the interface
        army01 = armyObject;
        army02 = interactedArmy;

        // Get the units for each army
        units01 = new List<GameObject>(armyObject.GetComponentInParent<UnitsInformation>().unitSlots);
        units02 = new List<GameObject>(interactedArmy.GetComponentInParent<UnitsInformation>().unitSlots);

        // Enable the interface gameObject
        this.transform.GetChild(0).gameObject.SetActive(true);

        // Update the display of units in the interface
        UpdateUnitDisplay();

        // Get the empty cells surrounding the active army
        neighbourCells = GameGrid.Instance.GetEmptyNeighbourCell(army01.GetComponent<Army>().gridPosition);
    }

    // Function to reset the state of the interface
    private void ClearSelection()
    {
        // Remove highlights from all unit buttons
        RemoveButtonHighlights();

        // Clear the units and unit slots in both armies and the placeholder army
        for (int i = 0; i < 7; i++){
            units01[i] = null;
            units02[i] = null;
            placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().RemoveUnits();
        }

        // Clear the army gameObjects
        army01 = null;
        army02 = null;

        // Update the display of units in the interface
        UpdateUnitDisplay();

        // Disable the interface gameObject
        this.transform.GetChild(0).gameObject.SetActive(false);

        // Disable the unit split window
        UnitSplitWindow.Instance.DisableUnitSwapWindow();
    }

    private void UpdateUnitDisplay ()
    {
        for (int i = 0; i < units01.Count; i++){
            if (units01[i] != null){
                if (!units01[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i].interactable = true;
                    // Check if the selected unit is a hero
                    if (units01[i].GetComponent<UnitSlot>().isSlotHero){
                        unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), units01[i].GetComponent<UnitSlot>().unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
                        unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = false;
                        unitCountDisplay[i].SetActive(false);
                    }else{
                        unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units01[i].GetComponent<UnitSlot>().unitID));
                        unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = false;
                        unitCountDisplay[i].SetActive(true);
                        unitCountDisplay[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units01[i].GetComponent<UnitSlot>().howManyUnits);                       
                    }
                }else{
                    unitInfoButtons[i].interactable = false;
                    unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i].SetActive(false);
                }
            }else{
                unitInfoButtons[i].interactable = false;
                unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                unitCountDisplay[i].SetActive(false);
            }
        }  

        for (int i = 0; i < units02.Count; i++){
            if (units02[i] != null){
                if (!units02[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i + 7].interactable = true;
                    // Check if the selected unit is a hero
                    if (units02[i].GetComponent<UnitSlot>().isSlotHero){
                        unitInfoSlot[i + 7].GetComponent<Image>().sprite = Resources.Load<Sprite>("HeroIcons/" + Enum.GetName(typeof(HeroTag), units02[i].GetComponent<UnitSlot>().unitID - Enum.GetValues(typeof(UnitName)).Cast<int>().Max()));
                        unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = false;
                        unitCountDisplay[i + 7].SetActive(false);
                    }else{
                        unitInfoSlot[i + 7].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units02[i].GetComponent<UnitSlot>().unitID));
                        unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = false;
                        unitCountDisplay[i + 7].SetActive(true);
                        unitCountDisplay[i + 7].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units02[i].GetComponent<UnitSlot>().howManyUnits);
                    }
                }else{
                    unitInfoButtons[i + 7].interactable = false;
                    unitInfoSlot[i + 7].GetComponent<Image>().sprite = defaultBackground;
                    unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                    unitCountDisplay[i + 7].SetActive(false);
                }
            }else{
                unitInfoButtons[i + 7].interactable = false;
                unitInfoSlot[i + 7].GetComponent<Image>().sprite = defaultBackground;
                unitInfoSlot[i + 7].GetComponentInParent<ArmyInterfaceUnitButton>().isSlotEmpty = true;
                unitCountDisplay[i + 7].SetActive(false);
            }
        } 
    }

    public bool AreUnitsHeroes(int id01, int id02)
    {
        if (id01 < 7){
            if (id02 < 7){
                if (units01[id01].GetComponent<UnitSlot>().isSlotHero || units01[id02].GetComponent<UnitSlot>().isSlotHero){
                    return true;
                }else{
                    return false;
                }
            }else{
                if (units01[id01].GetComponent<UnitSlot>().isSlotHero || units02[id02 - 7].GetComponent<UnitSlot>().isSlotHero){
                    return true;
                }else{
                    return false;
                }
            }
        }else if (id02 < 7){
            if (units02[id01 - 7].GetComponent<UnitSlot>().isSlotHero || units01[id02].GetComponent<UnitSlot>().isSlotHero){
                return true;
            }else{
                return false;
            }
        }else{
            if (units02[id01 - 7].GetComponent<UnitSlot>().isSlotHero || units02[id02 - 7].GetComponent<UnitSlot>().isSlotHero){
                return true;
            }else{
                return false;
            }
        }
        
    }

    public void RemoveButtonHighlights ()
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnit = null;
    }

    public void RemoveButtonHighlights (Player player)
    {
        onArmyInterfaceReload?.Invoke();
        selectedUnit = null;
    }

    public void ChangeSelectedUnit (short slotID)
    {
        if (slotID < 7){
            selectedUnit = units01[slotID].GetComponent<UnitSlot>();
        }else{
            selectedUnit = units02[slotID - 7].GetComponent<UnitSlot>();
        }
        
    }

    public void SwapUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SwapUnitsPosition(a, army02.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponent<UnitsInformation>().SwapUnitsPosition(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
        
        RefreshElement();
    }

    public void AddUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponentInParent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponentInParent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().AddUnits(a, army02.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b]);
            }else{
                army02.GetComponent<UnitsInformation>().AddUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
        RefreshElement();
    }

    public void SplitUnits (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, Convert.ToInt16(b - 7), army02.GetComponentInParent<UnitsInformation>(), army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponentInParent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), b, army01.GetComponentInParent<UnitsInformation>(), army01.GetComponentInParent<UnitsInformation>().unitSlots[a - 7]);
            }else{
                army02.GetComponentInParent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }else{
            if (a < 7){
                if (b < 7){
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, b);
                }else{
                    army01.GetComponentInParent<UnitsInformation>().SplitUnits(a, Convert.ToInt16(b - 7), army02.GetComponent<UnitsInformation>(), army02.GetComponent<UnitsInformation>().unitSlots[b - 7]);
                }
            }else if (b < 7){
                army02.GetComponent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), b, army01.GetComponentInParent<UnitsInformation>(), army01.GetComponentInParent<UnitsInformation>().unitSlots[a - 7]);
            }else{
                army02.GetComponent<UnitsInformation>().SplitUnits(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7));
            }
        }
    }

    public bool AreUnitsSameType (short a, short b)
    {
        if (!interactingWithPlaceholder){
            if (a < 7){
                if (b < 7){
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, b)) return true;
                    else return false;
                }else{
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, army02.GetComponentInParent<UnitsInformation>().unitSlots[b - 7])) return true;
                    else return false;
                }
            }else if (b < 7){
                if (army02.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b])) return true;
                else return false;
            }else{
                if (army02.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7))) return true;
                else return false;
            }
        }else{
            if (a < 7){
                if (b < 7){
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, b)) return true;
                    else return false;
                }else{
                    if (army01.GetComponentInParent<UnitsInformation>().AreUnitSlotsSameType(a, army02.GetComponent<UnitsInformation>().unitSlots[b - 7])) return true;
                    else return false;
                }
            }else if (b < 7){
                if (army02.GetComponent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), army01.GetComponentInParent<UnitsInformation>().unitSlots[b])) return true;
                else return false;
            }else{
                if (army02.GetComponent<UnitsInformation>().AreUnitSlotsSameType(Convert.ToInt16(a - 7), Convert.ToInt16(b - 7))) return true;
                else return false;
            }
        }
    }

    public void RefreshElement ()
    {
        ArmyInformation.Instance.RefreshElement();
        RemoveButtonHighlights();
        UpdateUnitDisplay();
    }

    public void ResetElement ()
    {
        if (interactingWithPlaceholder){
            if (!IsPlaceHolderArmyEmpty()){
                if (army01.GetComponent<Army>().IsArmyEmpty()){
                    ReturnUnits();
                }else{
                    CreateNewArmy();
                }
            }
        }else{
            if (IsArmyEmpty(army01)){
                WorldObjectManager.Instance.RemoveArmy(army01);
            }else if(IsArmyEmpty(army02)){
                WorldObjectManager.Instance.RemoveArmy(army02);
            }
        }
        ClearSelection();
        CameraManager.Instance.cameraEnabled = true;
    }

    private bool IsArmyEmpty(GameObject army)
    {
        foreach (GameObject unit in army.GetComponent<Army>().unitsInformation.unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    private bool IsPlaceHolderArmyEmpty ()
    {
        foreach (GameObject unit in placeHolderArmy.GetComponent<UnitsInformation>().unitSlots){
            if (!unit.GetComponent<UnitSlot>().slotEmpty) return false;
            else continue;
        }
        return true;
    }

    private void CreateNewArmy ()
    {
        if (neighbourCells.Count > 0){
            int[] _unitType = new int[7]; 
            int[] _unitCount = new int[7]; 
            float[] _unitMovement = new float[7]; 
            for (int i = 0; i < 7; i++){
                _unitType[i] = placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().unitID;
                _unitCount[i] = placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().howManyUnits;
                _unitMovement[i] = placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().movementPoints;
            }
            WorldObjectManager.Instance.CreateNewArmy(PlayerManager.Instance.currentPlayer.GetComponent<Player>().thisPlayerTag, 
            neighbourCells[0].GetPosition(), _unitType, _unitCount);
        }else{
            Debug.Log("No available spaces");
        }
    }

    private void ReturnUnits ()
    {
        for (int i = 0; i < 7; i++){
            if (placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().slotEmpty) continue;
            army01.GetComponent<Army>().unitsInformation.unitSlots[i].GetComponent<UnitSlot>().ChangeSlotStatus(placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().unitID, 
            placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().howManyUnits, placeHolderArmy.GetComponent<UnitsInformation>().unitSlots[i].GetComponent<UnitSlot>().movementPoints);
        }
        ArmyInformation.Instance.RefreshElement();
    }
}