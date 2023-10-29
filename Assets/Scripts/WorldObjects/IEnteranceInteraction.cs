using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnteranceInteraction
{
    public void SetEnteranceCells();

    public List<PathNode> GetEnteranceList ();

    public T GetConnectedObject <T> ();
}
