using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityDwellingButton : MonoBehaviour
{
    [SerializeField] Sprite unitButtonDefault; 
    [SerializeField] Sprite unitButtonHighlight;  
    [SerializeField] Button thisButton;  
    public bool isSlotEmpty;
    private bool objectSelected;

    private void Start ()
    {
        objectSelected = false;
        DwellingUI.Instance.onDwellingUpdate.AddListener(DeactivateHighlight);
    }

    public void ActivateHighlight ()
    {
        if (objectSelected){
            
        }else{
            DwellingUI.Instance.onDwellingUpdate?.Invoke();
            objectSelected = true;
            thisButton.gameObject.GetComponent<Image>().sprite = unitButtonHighlight;
        }
        
    }

    private void DeactivateHighlight ()
    {
        thisButton.gameObject.GetComponent<Image>().sprite = unitButtonDefault;
        objectSelected = false;
    }

    public void OpenUnitBuyInterface ()
    {
        if (objectSelected){
            
        }else{
            ActivateHighlight();
        }
    }
}
