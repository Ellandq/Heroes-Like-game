using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    private Player currentPlayer;

    [Header ("Resource display referances")]
    [SerializeField] private TextMeshProUGUI displayedGold;
    [SerializeField] private TextMeshProUGUI displayedWood;
    [SerializeField] private TextMeshProUGUI displayedOre;
    [SerializeField] private TextMeshProUGUI displayedGems;
    [SerializeField] private TextMeshProUGUI displayedMercury;
    [SerializeField] private TextMeshProUGUI displayedSulfur;
    [SerializeField] private TextMeshProUGUI displayedCrystals;

    private void Start()
    {
        TurnManager.OnNewPlayerTurn += UpdateDisplay;
        PlayerManager.OnCurrentPlayerResourcesGained += UpdateDisplay;
        UIManager.Instance.UIManagerReady();
    }

    public void UpdateDisplay (Player _currentPlayer)
    {
        currentPlayer = _currentPlayer;
        displayedGold.text = Convert.ToString(currentPlayer.gold);
        displayedWood.text = Convert.ToString(currentPlayer.wood);
        displayedOre.text = Convert.ToString(currentPlayer.ore);
        displayedGems.text = Convert.ToString(currentPlayer.gems);
        displayedMercury.text = Convert.ToString(currentPlayer.mercury);
        displayedSulfur.text = Convert.ToString(currentPlayer.sulfur);
        displayedCrystals.text = Convert.ToString(currentPlayer.crystals);
    }

    public void UpdateDisplay ()
    {
        displayedGold.text = Convert.ToString(currentPlayer.gold);
        displayedWood.text = Convert.ToString(currentPlayer.wood);
        displayedOre.text = Convert.ToString(currentPlayer.ore);
        displayedGems.text = Convert.ToString(currentPlayer.gems);
        displayedMercury.text = Convert.ToString(currentPlayer.mercury);
        displayedSulfur.text = Convert.ToString(currentPlayer.sulfur);
        displayedCrystals.text = Convert.ToString(currentPlayer.crystals);
    }
}
