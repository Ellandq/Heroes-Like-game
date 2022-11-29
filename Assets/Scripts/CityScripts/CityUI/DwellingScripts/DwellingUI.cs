using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DwellingUI : MonoBehaviour
{
    public static DwellingUI Instance;

    public UnityEvent onDwellingUpdate;

    private void Start ()
    {
        Instance = this;
    }

    private void OnDestroy ()
    {
        onDwellingUpdate.RemoveAllListeners();
    }
}
