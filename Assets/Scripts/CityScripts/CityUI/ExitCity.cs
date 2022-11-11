using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCity : MonoBehaviour
{
    AsyncOperation deloadCity;
    City city;

    public void ExitCityScene ()
    {
        city = SceneStateManager.displayedCity.GetComponent<City>();
        
        deloadCity = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(SceneStateManager.displayedCityFraction));
        SceneStateManager.ExitCity();
    }
}
