using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArmyInformation : MonoBehaviour
{
    [SerializeField] List <GameObject> units;
    [SerializeField] Sprite defaultBackground;

    [Header("UI Referances")]
    [SerializeField] List <GameObject> unitInfoSlot;
    [SerializeField] List <Button> unitInfoButtons;
    [SerializeField] List <GameObject> unitCountDisplay;

    internal GameObject selectedArmy;
    private string unitIconsFilePath;


    void Start ()
    {
        ObjectSelector.Instance.onSelectedObjectChange.AddListener(ChangeSelectedArmy);
    }

    private void ChangeSelectedArmy ()
    {
        if (ObjectSelector.Instance.lastObjectSelected != null ){
            if (ObjectSelector.Instance.lastObjectSelected.tag == "Army"){
                GetArmyUnits(ObjectSelector.Instance.lastObjectSelected);
            }else if (ObjectSelector.Instance.lastObjectSelected.tag == "City"){
                GetCityGarrison(ObjectSelector.Instance.lastObjectSelected);
            }else{
                Debug.Log(ObjectSelector.Instance.lastObjectSelected.tag);
                ClearSelection();
            }
        }else{
            ClearSelection();
        }
    }

    private void GetArmyUnits(GameObject armyObject)
    {
        units = new List<GameObject>(armyObject.GetComponentInParent<Army>().unitSlots);
        UpdateUnitDisplay();
    }

    private void GetCityGarrison(GameObject cityObject)
    {
        units = new List<GameObject>(cityObject.GetComponent<City>().garrisonSlots);
        UpdateUnitDisplay();
    }

    private void ClearSelection()
    {
        units[0] = null;
        units[1] = null;
        units[2] = null;
        units[3] = null;
        units[4] = null;
        units[5] = null;
        units[6] = null;
        UpdateUnitDisplay();
    }

    private void UpdateUnitDisplay ()
    {
        for (int i = 0; i < units.Count; i++){
            if (units[i] != null){
                if (!units[i].GetComponent<UnitSlot>().slotEmpty){
                    unitInfoButtons[i].interactable = true;
                    unitInfoSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UnitIcons/" + Enum.GetName(typeof(UnitName), units[i].GetComponent<UnitSlot>().unitID));
                    unitCountDisplay[i].SetActive(true);
                    unitCountDisplay[i].GetComponentInChildren<TMP_Text>().text = Convert.ToString(units[i].GetComponent<UnitSlot>().howManyUnits);

                }else{
                    unitInfoButtons[i].interactable = false;
                    unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                    unitCountDisplay[i].SetActive(false);
                }
            }else{
                unitInfoButtons[i].interactable = false;
                unitInfoSlot[i].GetComponent<Image>().sprite = defaultBackground;
                unitCountDisplay[i].SetActive(false);
            }
        }  
    }
}
