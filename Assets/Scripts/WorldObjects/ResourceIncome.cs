using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ResourceIncome : MonoBehaviour
{
    private int[] resources = new int[7]; // Array to hold resource values

    public int gold
    {
        get { return resources[0]; }
        set { resources[0] = value; }
    }

    public int wood
    {
        get { return resources[1]; }
        set { resources[1] = value; }
    }

    public int ore
    {
        get { return resources[2]; }
        set { resources[2] = value; }
    }

    public int gems
    {
        get { return resources[3]; }
        set { resources[3] = value; }
    }

    public int mercury
    {
        get { return resources[4]; }
        set { resources[4] = value; }
    }

    public int sulfur
    {
        get { return resources[5]; }
        set { resources[5] = value; }
    }

    public int crystal
    {
        get { return resources[6]; }
        set { resources[6] = value; }
    }

    public ResourceIncome() {}

    public ResourceIncome(int value, ResourceType type){
        resources[(int)type] = value;
    }

    public ResourceIncome(int[] res)
    {
        if (res.Length != 7)
        {
            throw new ArgumentException("The input array must have exactly 7 elements.");
        }

        for (int i = 0; i < 7; i++)
        {
            resources[i] = res[i];
        }
    }

    public ResourceIncome (ResourceIncome baseValues, List<ResourceType> resourceTypes)
    {
        foreach (ResourceType type in resourceTypes){
            resources[(int)type] = baseValues.resources[(int)type];
        }
    }

    public static ResourceIncome operator +(ResourceIncome a, ResourceIncome b)
    {
        int[] result = new int[7];
        for (int i = 0; i < 7; i++)
        {
            result[i] = a.resources[i] + b.resources[i];
        }

        return new ResourceIncome(result);
    }

    public static ResourceIncome operator -(ResourceIncome a, ResourceIncome b)
    {
        int[] result = new int[7];
        for (int i = 0; i < 7; i++)
        {
            result[i] = Mathf.Abs(a.resources[i] - b.resources[i]);
        }

        return new ResourceIncome(result);
    }

    public static bool operator >(ResourceIncome a, ResourceIncome b){
        for (int i = 0; i < 7; i++){
            if (b.resources[i] > a.resources[i]) return false;
        }
        return true;
    }

    public static bool operator <(ResourceIncome a, ResourceIncome b){
        for (int i = 0; i < 7; i++){
            if (a.resources[i] > b.resources[i]) return false;
        }
        return true;
    }

    public static int operator /(ResourceIncome a, ResourceIncome b){
        int value = int.MaxValue;
        for (int i = 0; i < 7; i++){
            value = Mathf.Min(Mathf.FloorToInt(a.resources[i] / b.resources[i]), value);
        }
        return value;
    }
}

