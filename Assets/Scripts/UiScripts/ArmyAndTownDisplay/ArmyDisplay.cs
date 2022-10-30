using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDisplay : MonoBehaviour
{
    [SerializeField] ObjectSelector objectSelector;
    [SerializeField] private List <GameObject> armyDisplay;
    
    [Header ("Display Images")]
    [SerializeField] Sprite armyImage;
    [SerializeField] Sprite emptyCell;

    internal void UpdateArmyDisplay (Player player)
    {
        ResetArmyDisplay();
        
        for (int i = 0; i < player.ownedArmies.Count; i++){
            armyDisplay[i].GetComponent<Image>().sprite = armyImage;
            armyDisplay[i].GetComponent<ArmyButton>().UpdateConnectedArmy(player.ownedArmies[i]);
            armyDisplay[i].GetComponent<Button>().interactable = true;
        }
    }

    private void ResetArmyDisplay ()
    {
        if (armyDisplay.Count > 3){
            foreach (GameObject displayedArmy in armyDisplay){
                if (displayedArmy == armyDisplay[0] | displayedArmy == armyDisplay[1] | displayedArmy == armyDisplay[2]) continue;
                Destroy(displayedArmy);
            }
        }else{
            armyDisplay[0].GetComponent<Image>().sprite = emptyCell;
            armyDisplay[0].GetComponent<Button>().interactable = false;
            armyDisplay[1].GetComponent<Image>().sprite = emptyCell;
            armyDisplay[1].GetComponent<Button>().interactable = false;
            armyDisplay[2].GetComponent<Image>().sprite = emptyCell;
            armyDisplay[2].GetComponent<Button>().interactable = false;
        }
    }
}
