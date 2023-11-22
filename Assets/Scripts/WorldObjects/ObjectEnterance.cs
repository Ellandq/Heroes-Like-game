using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnterance : MonoBehaviour
{
    private List <PathNode> enteranceNodes; 

    public List<PathNode> GetEnteranceList() { 
        return enteranceNodes; }

    public void SetEnteranceList (List<PathNode> list){ 
        enteranceNodes = list; }
}


