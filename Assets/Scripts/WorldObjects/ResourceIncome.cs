using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ResourceIncome : MonoBehaviour
{
    public int gold;
    public int wood;
    public int ore;
    public int gems;
    public int mercury;
    public int sulfur;
    public int crystal;

    public ResourceIncome () { }
    
    public ResourceIncome (int amount, ResourceType resourceType){
        switch (resourceType)
        {
            case ResourceType.Gold:
                gold = amount;
                break;
            case ResourceType.Wood:
                wood = amount;
                break;
            case ResourceType.Ore:
                ore = amount;
                break;
            case ResourceType.Gems:
                gems = amount;
                break;
            case ResourceType.Mercury:
                mercury = amount;
                break;
            case ResourceType.Sulfur:
                sulfur = amount;
                break;
            case ResourceType.Crystal:
                crystal = amount;
                break;
            default:
                // Handle any other resource types as needed.
                break;
        }
    }

    public ResourceIncome(int[] res)
    {
        if (res.Length != 7)
        {
            throw new ArgumentException("The input array must have exactly 7 elements.");
        }

        // Assign the values from the input array to the fields
        gold = res[0];
        wood = res[1];
        ore = res[2];
        gems = res[3];
        mercury = res[4];
        sulfur = res[5];
        crystal = res[6];
    }

    public static ResourceIncome operator +(ResourceIncome a, ResourceIncome b){
        return new ResourceIncome
        {
            gold = a.gold + b.gold,
            wood = a.wood + b.wood,
            ore = a.ore + b.ore,
            gems = a.gems + b.gems,
            mercury = a.mercury + b.mercury,
            sulfur = a.sulfur + b.sulfur,
            crystal = a.crystal + b.crystal
        };
    }

    public static ResourceIncome operator -(ResourceIncome a, ResourceIncome b){
        return new ResourceIncome
        {
            gold = Mathf.Abs(a.gold - b.gold),
            wood = Mathf.Abs(a.wood - b.wood),
            ore = Mathf.Abs(a.ore - b.ore),
            gems = Mathf.Abs(a.gems - b.gems),
            mercury = Mathf.Abs(a.mercury - b.mercury),
            sulfur = Mathf.Abs(a.sulfur - b.sulfur),
            crystal = Mathf.Abs(a.crystal - b.crystal)
        };
    }
}
