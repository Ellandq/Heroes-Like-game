using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;


    private void Awake ()
    {
        Instance = this;
    }
}

public enum ArtifactTag{

}
