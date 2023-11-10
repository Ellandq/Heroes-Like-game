using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnterance : MonoBehaviour
{
    private List <PathNode> enteranceNodes; 

    public List<PathNode> GetEnteranceList() { 
        Debug.Log(enteranceNodes);
        return enteranceNodes; }

    public void SetEnteranceList (List<PathNode> list){ 
        Debug.Log(transform.parent.gameObject.name + ", " + list.Count );
        enteranceNodes = list; }
}


