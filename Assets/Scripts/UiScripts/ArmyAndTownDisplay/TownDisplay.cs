using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownDisplay : MonoBehaviour
{
    [SerializeField] ObjectSelector objectSelector;
    [SerializeField] private List <GameObject> townDisplay;

    [Header ("Display Images")]
    [SerializeField] Sprite townImage;
    [SerializeField] Sprite emptyCell;

    internal void UpdateTownDisplay (Player player)
    {
        ResetTownDisplay();

        for (int i = 0; i < player.ownedCities.Count; i++){
            townDisplay[i].GetComponent<Image>().sprite = townImage;
            townDisplay[i].GetComponent<TownButton>().UpdateConnectedCity(player.ownedCities[i]);
            townDisplay[i].GetComponent<Button>().interactable = true;
        }
    }

    private void ResetTownDisplay ()
    {
        if (townDisplay.Count > 3){
            foreach (GameObject displayedTown in townDisplay){
                if (displayedTown == townDisplay[0] | displayedTown == townDisplay[1] | displayedTown == townDisplay[2]) continue;
                Destroy(displayedTown);
            }
        }else{
            townDisplay[0].GetComponent<Image>().sprite = emptyCell;
            townDisplay[0].GetComponent<Button>().interactable = false;
            townDisplay[1].GetComponent<Image>().sprite = emptyCell;
            townDisplay[1].GetComponent<Button>().interactable = false;
            townDisplay[2].GetComponent<Image>().sprite = emptyCell;
            townDisplay[2].GetComponent<Button>().interactable = false;
        }
    }
}
