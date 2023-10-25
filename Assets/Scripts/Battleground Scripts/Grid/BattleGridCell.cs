using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridCell : MonoBehaviour
{
    private Vector2Int gridPosition;

    [Header ("GridCell Colours")]
    [SerializeField] private List<Color> gridCellColours;

    public void SetCellPosition (int x, int y) { gridPosition = new Vector2Int(x, y); }

    public void ChangeCellStatus (BattleGridCellStatus status, bool unavailableCellVisability = true){
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            Material material = renderer.material;
            if (material != null) {
                material.color = gridCellColours[(int)status];
            }
        }

        if (status == BattleGridCellStatus.unavailable && !unavailableCellVisability){
            renderer.enabled = false;
            
        }else{
            renderer.enabled = true;
        }
    }

    public void HighlightCell (){

    }

    public Vector2Int GetPosition () { return gridPosition; }
}
