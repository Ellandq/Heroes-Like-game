using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownDisplay : MonoBehaviour
{
    [SerializeField] private GameObject townDisplayPrefab;
    [SerializeField] private GameObject townDisplayCanvas;
    [SerializeField] private List <GameObject> townDisplay;

    [Header ("Display Images")]
    [SerializeField] private Sprite townImage;
    [SerializeField] private Sprite emptyCell;

    // Updates the City display
    internal void UpdateTownDisplay (Player player)
    {
        ResetTownDisplay();
        if (player.ownedArmies.Count > 3){
            for (int i = 3; i < player.ownedCities.Count; i++){
                townDisplay.Add(Instantiate(townDisplayPrefab, new Vector3(townDisplayPrefab.transform.position.x, (-90 * (i - 1)), 0), Quaternion.identity));
                townDisplay[i].transform.SetParent(this.gameObject.transform, false);
                townDisplay[i].gameObject.name = "Town (" + i + ")"; 
            }
        }
        for (int i = 0; i < player.ownedCities.Count; i++){
            townDisplay[i].GetComponent<Image>().sprite = townImage;
            townDisplay[i].GetComponent<TownButton>().UpdateConnectedCity(player.ownedCities[i]);
            townDisplay[i].GetComponent<Button>().interactable = true;
        }
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (3 - townDisplay.Count) * 42, 0);
    }

    // Checks if the cities are correctly placed on the content scroller
    private void Update ()
    {
        if (this.gameObject.GetComponent<RectTransform>().anchoredPosition.y < (3 - townDisplay.Count) * 42 | this.gameObject.GetComponent<RectTransform>().anchoredPosition.y > (townDisplay.Count - 3) * 42){
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, Mathf.Clamp(this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, (3 - townDisplay.Count) * 42,(townDisplay.Count - 3) * 42), 0);
            townDisplayCanvas.GetComponent<ScrollRect>().velocity = Vector2.zero;
        }
        
    }

    // Resets the city display
    private void ResetTownDisplay ()
    {
        if (townDisplay.Count > 3){
            foreach (GameObject displayedTown in townDisplay){
                if (displayedTown == townDisplay[0] | displayedTown == townDisplay[1] | displayedTown == townDisplay[2]) continue;
                Destroy(displayedTown);
            }
            townDisplay.RemoveRange(3, (townDisplay.Count - 3));
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
