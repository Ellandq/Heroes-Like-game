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
        SceneStateManager.ExitCity();
    }
}
