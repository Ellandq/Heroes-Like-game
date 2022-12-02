using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DwellingUI : MonoBehaviour
{
    public static DwellingUI Instance;

    [SerializeField] private List<CityDwellingButton> dwellingButtons;
    [SerializeField] private List<CityDwellingBuyButton> dwellingBuyButtons;
    [SerializeField] private GameObject unitShopUI;
    public UnityEvent onDwellingUpdate;

    private void Awake ()
    {
        Instance = this;
    }

    public void OpenDwellingUI ()
    {
        transform.parent.gameObject.SetActive(false);
        unitShopUI.SetActive(true);

    }

    public void CloseDwellingUI ()
    {
        transform.parent.gameObject.SetActive(true);
        unitShopUI.SetActive(false);
    }

    private void ResetDwellingDispaly ()
    {
        for (int i = 0; i < CityManager.Instance.currentCity.cityDwellingInformation.cityDwellings.Count; i++){
            dwellingButtons[i].DisableButton();
        }
    }

    public void UpdateDwellingDisplay ()
    {
        ResetDwellingDispaly();
        for (int i = 0; i < CityManager.Instance.currentCity.cityDwellingInformation.cityDwellings.Count; i++){
            if (CityManager.Instance.currentCity.cityDwellingInformation.cityDwellings[i] != null){
                dwellingButtons[i].UpdateButton(CityManager.Instance.currentCity.cityDwellingInformation.cityDwellings[i].unitIcon, Convert.ToString(Math.Floor(CityManager.Instance.currentCity.cityDwellingInformation.cityDwellingUnitCount[i])));
            }else{
                dwellingButtons[i].DisableButton();
            }
        }
    }

    public void DeactivateButtonHighlights (short _buttonIndex){
        for (int i = 0; i < dwellingButtons.Count; i++){
            if (i != _buttonIndex) dwellingButtons[i].DeactivateHighlight();
        }

    }

    public void DeactivateBuyButtonHighlights (short _buttonIndex){
        for (int i = 0; i < dwellingButtons.Count; i++){
            if (i != _buttonIndex) dwellingBuyButtons[i].DeactivateHighlight();
        }
    }

    private void OnDestroy ()
    {
        onDwellingUpdate.RemoveAllListeners();
    }
}
