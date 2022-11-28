using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MonoBehaviour
{
    private bool cityReady = false;
    [SerializeField] GameObject flag;

    [Header("Main city information")]
    [SerializeField] public GameObject ownedByPlayer;
    public string cityFraction;
    public Vector2Int gridPosition;
    private Vector3 position;
    public Vector3 rotation;
    public int cityGoldProduction;
    public bool canBeSelectedByCurrentPlayer;
    public bool cityBuildingAlreadybuilt = false;

    [Header("City Enterance Information")]
    [SerializeField] private GameObject cityEnterance;
    [SerializeField] public List<PathNode> enteranceCells;

    [Header("Garrison refrences")]
    bool cityEmpty;
    [SerializeField] public List <GameObject> garrisonSlots;

    public List<Int16> cityBuildings;

    public void CityInitialization (string _ownedByPlayer, 
        string _cityFraction, Vector2Int _gridPosition,
        float _cityOrientation, int [] _cityBuildingStatus, int [] _cityGarrison)
    {
        cityBuildingAlreadybuilt = false;
        cityFraction = _cityFraction;
        gridPosition = _gridPosition;
        rotation.y = _cityOrientation;
        transform.localEulerAngles = rotation;

        #region City buildings

        cityBuildings = new List<short>();

        //basic
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[0])); // village hall
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[1])); // town hall
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[2])); // city hall
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[3])); // tavern
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[4])); // prison
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[5])); // fort
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[6])); // citadel
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[7])); // castle
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[8])); // caravan
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[9])); // shipyard
        //bonus
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[10])); // bonus building 1
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[11])); // bonus building 2
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[12])); // equipement 
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[13])); // racial building
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[14])); // graal building
        //magic
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[15])); // magic 1
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[16])); // magic 2
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[17])); // magic 3
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[18])); // magic 4
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[19])); // magic 5
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[20])); // additional magic 1
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[21])); // additional magic 2
        //unit
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[22])); // t1 1
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[23])); // t1 2
        
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[24])); // t2 1
        if (cityBuildings[24] != 1){
            cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[25])); // t2 2
        }else{
            cityBuildings.Add(2); // t2 2
        }
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[26])); // t3 1
        if (cityBuildings[26] != 1){
            cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[27])); // t3 2
        }else{
            cityBuildings.Add(2); // t3 2
        }
        cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[28])); // t4 1
        if (cityBuildings[28] != 1){
            cityBuildings.Add(Convert.ToInt16(_cityBuildingStatus[29])); // t4 2
        }else{
            cityBuildings.Add(2); // t4 2
        }

        if (cityBuildings[22] == 1 | cityBuildings[23] == 1){ // 30 - tier 1 built
            cityBuildings.Add(1);
        }else{
            cityBuildings.Add(0);
        }
        if (cityBuildings[24] == 1 | cityBuildings[25] == 1){ // 31 - tier 2 built
            cityBuildings.Add(1);
        }else{
            cityBuildings.Add(0);
        }
        if (cityBuildings[26] == 1 | cityBuildings[27] == 1){ // 32 - tier 3 built 
            cityBuildings.Add(1);
        }else{
            cityBuildings.Add(0);
        }
        if (cityBuildings[28] == 1 | cityBuildings[29] == 1){ // 33 - tier 4 built
            cityBuildings.Add(1);
        }else{
            cityBuildings.Add(0);
        }

        #endregion

        #region City Garrison
        garrisonSlots[0].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[0], _cityGarrison[1]);
        garrisonSlots[1].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[2], _cityGarrison[3]);
        garrisonSlots[2].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[4], _cityGarrison[5]);
        garrisonSlots[3].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[6], _cityGarrison[7]);
        garrisonSlots[4].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[8], _cityGarrison[9]);
        garrisonSlots[5].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[10], _cityGarrison[11]);
        garrisonSlots[6].GetComponent<UnitSlot>().SetSlotStatus(_cityGarrison[12], _cityGarrison[13]);

        #endregion

        CityGoldProductionCheck();
    }

    private void FinalizeCity ()
    {
        PlayerManager.Instance.OnNextPlayerTurn.AddListener(UpdateCitySelectionAvailability);
        enteranceCells = new List<PathNode>();
        cityReady = true;
    }

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }  
        if (!cityReady) FinalizeCity();
    }

    private void ChangeOwningPlayer (GameObject _ownedByPlayer)
    {
        ownedByPlayer.GetComponent<Player>().ownedCities.Remove(this.gameObject);
        if (ownedByPlayer.name == "Neutral Player"){
            ownedByPlayer = _ownedByPlayer;
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }else{
            ownedByPlayer = _ownedByPlayer;
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
        ownedByPlayer.GetComponent<Player>().ownedCities.Add(this.gameObject);
        ownedByPlayer.GetComponent<Player>().onCityAdded?.Invoke();
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = PlayerManager.Instance.neutralPlayer;
        flag.SetActive(false);
    }

    private void CityGoldProductionCheck()
    {
        if (cityBuildings[0] == 1){
            cityGoldProduction = 250;
            if (cityBuildings[1] == 1){
                cityGoldProduction = 500;
                if (cityBuildings[2] == 1){
                    cityGoldProduction = 1000;
                }
            }
        }else{
            cityBuildings[0] = 1;
            CityGoldProductionCheck();
        }
    }

    private void UpdateCitySelectionAvailability(Player _player)
    {
        if (_player.gameObject.name  == ownedByPlayer.name){
            cityBuildingAlreadybuilt = false;
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    public void CityInteraction (GameObject interactingArmy)
    {
        Debug.Log("Interacting army with city: " + interactingArmy.name);
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            GameManager.Instance.EnterCity(this.gameObject, cityFraction, interactingArmy.GetComponentInParent<Army>());
        }else{
            if (IsCityEmpty()){
                ChangeOwningPlayer(interactingArmy.GetComponent<Army>().ownedByPlayer);
            }else{
                Debug.Log("Do battle");
            }
        }
    }

    public void CityInteraction ()
    {
        GameManager.Instance.EnterCity(this.gameObject, cityFraction);
    }

    public void GetEnteranceInformation (List <PathNode> _enteranceList)
    {
        enteranceCells = _enteranceList;
    }

    public float GetCityRotation ()
    {
        return rotation.y;
    }

    private bool IsCityEmpty ()
    {
        if (!garrisonSlots[0].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[1].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[2].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[3].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[4].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[5].GetComponent<UnitSlot>().slotEmpty) return false;
        if (!garrisonSlots[6].GetComponent<UnitSlot>().slotEmpty) return false;
        return true;
    }

    public void CreateNewBuilding (int id, int[] resourceCost)
    {
        cityBuildingAlreadybuilt = true;
        if (id > 21 && id < 30){
            switch (id)
            {
                case 22:
                    cityBuildings[id] = 1;
                    cityBuildings[30] = 1;
                break;

                case 23:
                    cityBuildings[id] = 1;
                    cityBuildings[30] = 1;
                break;

                case 24:
                    cityBuildings[id] = 1;
                    cityBuildings[25] = 2;
                    cityBuildings[31] = 1;
                break;

                case 25:
                    cityBuildings[id] = 1;
                    cityBuildings[24] = 2;
                    cityBuildings[31] = 1;
                break;

                case 26:
                    cityBuildings[id] = 1;
                    cityBuildings[27] = 2;
                    cityBuildings[32] = 1;
                break;

                case 27:
                    cityBuildings[id] = 1;
                    cityBuildings[26] = 2;
                    cityBuildings[32] = 1;
                break;

                case 28:
                    cityBuildings[id] = 1;
                    cityBuildings[29] = 2;
                    cityBuildings[33] = 1;
                break;

                case 29:
                    cityBuildings[id] = 1;
                    cityBuildings[28] = 2;
                    cityBuildings[33] = 1;
                break;
            }
        }else{
            cityBuildings[id] = 1;
        }
        CityGoldProductionCheck();
    }
}
