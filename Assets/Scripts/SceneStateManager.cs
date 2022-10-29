using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager
{
    public static GameObject displayedCity;
    public static string displayedCityFraction;


    public static void EnterCity (GameObject cityToEnter, string _cityFraction)
    {
        SceneStateManager.displayedCity = cityToEnter;
        displayedCityFraction = _cityFraction;
        GameManager.Instance.DisableWorldObjects();
        try{
            SceneManager.LoadScene(displayedCityFraction, LoadSceneMode.Additive);
        }catch (NullReferenceException){
            Debug.Log("No Scene with name: " + displayedCityFraction + " exists.");
        }
    }

    public static void ExitCity ()
    {
        GameManager.Instance.EnableWorldObjects();
    }

    private IEnumerator WaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
    }
}
