using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CityDwellingInformation : MonoBehaviour
{
    [SerializeField] City connectedCity;
    private Player ownedByPlayer;
    [SerializeField] public List<DwellingObject> cityDwellings;
    [SerializeField] public List<float> cityDwellingUnitCount;
    private List<int> unitCalculatedCost;

    private void Start ()
    {
        ownedByPlayer = connectedCity.ownedByPlayer.GetComponent<Player>();
    }

    public void AddDailyUnits ()
    {
        if (connectedCity.ownedByPlayer.name != "Neutral Player"){
            if (cityDwellings.Count > 0){
                for (int i = 0; i < cityDwellings.Count; i++){
                    if (cityDwellings[i] != null){
                        cityDwellingUnitCount[i] += (cityDwellings[i].unitWeeklyGain / 7);
                    }
                }
            }
        }
    }

    public void AddDwelling (CityFraction fraction, int index)
    {
        DwellingObject tmp = DwellingManager.Instance.GetDwellingObject(fraction, index);
        cityDwellings[tmp.dwellingIndex - 1] = tmp;
        cityDwellingUnitCount[tmp.dwellingIndex - 1] = (tmp.unitWeeklyGain / 2);
    }

    public void AddDwelling (UnitName unitName)
    {
        // DwellingObject tmp = DwellingManager.Instance.GetDwellingObject(fraction, index);
        // cityDwellings[tmp.dwellingIndex - 1] = tmp;
        // cityDwellingUnitCount[tmp.dwellingIndex - 1] = (tmp.unitWeeklyGain / 2);
    }

    public void BuyUnits (int index, int unitCount)
    {
        int[] arr = {cityDwellings[index].goldCost * unitCount, cityDwellings[index].woodCost * unitCount, cityDwellings[index].oreCost * unitCount, cityDwellings[index].gemCost * unitCount, 
        cityDwellings[index].mercuryCost * unitCount, cityDwellings[index].sulfurCost * unitCount, cityDwellings[index].crystalCost * unitCount};
        ownedByPlayer.RemoveResources(arr);
        cityDwellingUnitCount[index] -= unitCount;
    } 

    public void BuyUnits (int[] index, int[] unitCount)
    {
        for (int i = 0; i < index.Length; i++){
            BuyUnits(index[i], unitCount[i]);
        }
    } 

    public short CalculateUnitsAvailableToBuy (short index)
    {
        if (cityDwellings[index].goldCost > ownedByPlayer.gold) return 0;
        if (cityDwellings[index].woodCost > ownedByPlayer.wood) return 0;
        if (cityDwellings[index].oreCost > ownedByPlayer.ore) return 0;
        if (cityDwellings[index].gemCost > ownedByPlayer.gems) return 0;
        if (cityDwellings[index].mercuryCost > ownedByPlayer.mercury) return 0;
        if (cityDwellings[index].sulfurCost > ownedByPlayer.sulfur) return 0;
        if (cityDwellings[index].crystalCost > ownedByPlayer.crystals) return 0;

        unitCalculatedCost = new List<int>();
        if (cityDwellings[index].goldCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.gold / cityDwellings[index].goldCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (cityDwellings[index].woodCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.wood / cityDwellings[index].woodCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (cityDwellings[index].oreCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.ore / cityDwellings[index].oreCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (cityDwellings[index].gemCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.gems / cityDwellings[index].gemCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (cityDwellings[index].mercuryCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.mercury / cityDwellings[index].mercuryCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (cityDwellings[index].sulfurCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.sulfur / cityDwellings[index].sulfurCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (cityDwellings[index].crystalCost != 0){
            unitCalculatedCost.Add(Math.Min((ownedByPlayer.crystals / cityDwellings[index].crystalCost), Convert.ToInt32(Math.Floor(cityDwellingUnitCount[index]))));
        }
        if (unitCalculatedCost.Count > 0) return Convert.ToInt16(unitCalculatedCost.Min());
        else return 0;
        
    }
}
