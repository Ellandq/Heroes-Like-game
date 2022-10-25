using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnterance : MonoBehaviour
{
    public List <PathNode> enteranceNodes; 
    short enteranceNodeCount = 0;

    void Start ()
    {
        enteranceNodes = new List<PathNode>();
    }

    public List<PathNode> GetEnteranceList()
    {
    return enteranceNodes;
    }
}


