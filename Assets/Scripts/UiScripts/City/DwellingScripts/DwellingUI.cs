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
    private CityDwellingInformation dInfo;
    public List<byte> emptyCitySlots;
    public UnityEvent onDwellingUpdate;

    private void Awake ()
    {
        Instance = this;
    }

    public void OpenDwellingUI ()
    {
        dInfo = CityManager.Instance.GetCity().GetBuildingHandler().GetCityDwellingInformation();
        transform.parent.gameObject.SetActive(false);
        unitShopUI.SetActive(true);
        UpdateEmptyCitySlots();
    }

    public void CloseDwellingUI ()
    {
        transform.parent.gameObject.SetActive(true);
        unitShopUI.SetActive(false);
        CityManager.Instance.RefreshArmyInterface();
    }

    private void ResetDwellingDispaly ()
    {
        for (int i = 0; i < dInfo.GetDwellings().Count; i++){
            dwellingButtons[i].DisableButton();
        }
    }

    public void UpdateDwellingDisplay ()
    {
        UpdateEmptyCitySlots();
        ResetDwellingDispaly();
        List<DwellingObject> dwObj = dInfo.GetDwellings();
        for (int i = 0; i < dwObj.Count; i++){
            if (dwObj[i] != null){
                dwellingButtons[i].UpdateButton(dwObj[i].unitIcon, 
                Convert.ToString(Math.Floor(dInfo.GetUnitCount(i))), 
                (short)dwObj[i].unit);
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

    public void UpdateEmptyCitySlots ()
    {
        emptyCitySlots = CityManager.Instance.GetCity().GetEmptyGarrisonSlotCount();
    }

    private void OnDestroy ()
    {
        onDwellingUpdate.RemoveAllListeners();
    }
}
