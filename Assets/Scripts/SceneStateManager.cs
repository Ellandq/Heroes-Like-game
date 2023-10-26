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

    [Header ("Battle Scene Information")]
    public static GameObject siegedCity;
    public static Army attackingArmy;
    public static Army defendingArmy;

    [Header ("Game Information")]
    public static string selectedMapName;
    public static string defaultMap = "TestMap";

    [Header ("Player Information")]
    public static List<PlayerTag> humanPlayers;

    public static void EnterCity (GameObject cityToEnter)
    {
        displayedCity = cityToEnter;
        CityFraction displayedCityFraction = displayedCity.GetComponent<City>().GetFraction();
        GameManager.Instance.EnableWorldObjects(false);
        try{
            SceneManager.LoadScene(Enum.GetName(typeof (CityFraction), displayedCityFraction), LoadSceneMode.Additive);
        }catch (NullReferenceException){
            Debug.Log("No Scene with name: " + displayedCityFraction + " exists.");
        }
    }

    public static void ExitCity (){
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(Enum.GetName(typeof (CityFraction), displayedCity.GetComponent<City>().GetFraction())));
        GameManager.Instance.ExitCity();
    }

    public static void StartBattle (Army _attackingArmy, Army _defendingArmy, GameObject _siegedCity = null){
        attackingArmy = _attackingArmy;
        defendingArmy = _defendingArmy;
        siegedCity = _siegedCity;

        GameManager.Instance.EnableWorldObjects(false);
        try{
            SceneManager.LoadScene("Battleground", LoadSceneMode.Additive);
        }catch (NullReferenceException){
            Debug.Log("No Scene with name: " + "Battleground" + " exists.");
        }
    }
}
