using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager
{
    [Header ("City scene information")]
    public static GameObject displayedCity;
    public static Army interactingArmy;
    public static CityFraction displayedCityFraction;

    [Header ("Game Information")]
    public static string selectedMapName;
    public static string defaultMap = "TestMap";

    [Header ("Player Information")]
    public static List<PlayerTag> humanPlayers;

    public static void EnterCity (GameObject cityToEnter, CityFraction _cityFraction)
    {
        SceneStateManager.displayedCity = cityToEnter;
        displayedCityFraction = _cityFraction;
        GameManager.Instance.DisableWorldObjects();
        try{
            SceneManager.LoadScene(Enum.GetName(typeof (CityFraction), displayedCityFraction), LoadSceneMode.Additive);
        }catch (NullReferenceException){
            Debug.Log("No Scene with name: " + displayedCityFraction + " exists.");
        }
    }

    public static void ExitCity ()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(Enum.GetName(typeof (CityFraction), displayedCityFraction)));
        GameManager.Instance.ExitCity();
    }
}
