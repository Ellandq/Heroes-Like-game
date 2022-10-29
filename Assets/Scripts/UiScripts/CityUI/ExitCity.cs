using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCity : MonoBehaviour
{
    City city;

    public void ExitCityScene ()
    {
        city = SceneStateManager.displayedCity.GetComponent<City>();
        
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(SceneStateManager.displayedCityFraction));
        SceneStateManager.ExitCity();
    }
}
