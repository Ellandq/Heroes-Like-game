using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDisplay : MonoBehaviour
{
    [SerializeField] private GameObject armyDisplayPrefab;
    [SerializeField] private GameObject armyDisplayCanvas;
    [SerializeField] private List <GameObject> armyDisplay;
    
    [Header ("Display Images")]
    [SerializeField] private Sprite armyImage;
    [SerializeField] private Sprite emptyCell;

    // Updates the army display
    internal void UpdateArmyDisplay (Player player)
    {
        ResetArmyDisplay();
        if (player.ownedArmies.Count > 3){
            for (int i = 3; i < player.ownedArmies.Count; i++){
                armyDisplay.Add(Instantiate(armyDisplayPrefab, new Vector3(armyDisplayPrefab.transform.position.x, (-90 * (i - 1)), 0), Quaternion.identity));
                armyDisplay[i].transform.SetParent(this.gameObject.transform, false);
                armyDisplay[i].gameObject.name = "Army (" + i + ")"; 
            }
        }
        for (int i = 0; i < player.ownedArmies.Count; i++){
            armyDisplay[i].GetComponent<Image>().sprite = armyImage;
            armyDisplay[i].GetComponent<ArmyButton>().UpdateConnectedArmy(player.ownedArmies[i]);
            armyDisplay[i].GetComponent<Button>().interactable = true;
            armyDisplay[i].GetComponent<ArmyButton>().movementSlider.gameObject.SetActive(true);
        }
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (3 - armyDisplay.Count) * 42, 0);
    }

    // Checks if the armies are correctly placed on the content scroller
    private void Update ()
    {
        if (this.gameObject.GetComponent<RectTransform>().anchoredPosition.y < (3 - armyDisplay.Count) * 42 | this.gameObject.GetComponent<RectTransform>().anchoredPosition.y > (armyDisplay.Count - 3) * 42){
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, Mathf.Clamp(this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, (3 - armyDisplay.Count) * 42,(armyDisplay.Count - 3) * 42), 0);
            armyDisplayCanvas.GetComponent<ScrollRect>().velocity = Vector2.zero;
        }
        
    }

    // Resets the army display
    private void ResetArmyDisplay ()
    {
        if (armyDisplay.Count > 3){
            foreach (GameObject displayedArmy in armyDisplay){
                if (displayedArmy == armyDisplay[0] | displayedArmy == armyDisplay[1] | displayedArmy == armyDisplay[2]) continue;
                Destroy(displayedArmy);
            }
            armyDisplay.RemoveRange(3, (armyDisplay.Count - 3));
        }
        armyDisplay[0].GetComponent<Image>().sprite = emptyCell;
        armyDisplay[0].GetComponent<Button>().interactable = false;
        armyDisplay[0].GetComponent<ArmyButton>().ResetArmyButton();
        armyDisplay[1].GetComponent<Image>().sprite = emptyCell;
        armyDisplay[1].GetComponent<Button>().interactable = false;
        armyDisplay[1].GetComponent<ArmyButton>().ResetArmyButton();
        armyDisplay[2].GetComponent<Image>().sprite = emptyCell;
        armyDisplay[2].GetComponent<Button>().interactable = false;
        armyDisplay[2].GetComponent<ArmyButton>().ResetArmyButton();
        
    }
}
