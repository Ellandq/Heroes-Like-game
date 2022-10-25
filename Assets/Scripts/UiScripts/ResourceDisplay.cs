using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    Player currentPlayer;
    [SerializeField] GameObject goldCounter;
    [SerializeField] GameObject woodCounter;
    [SerializeField] GameObject oreCounter;
    [SerializeField] GameObject gemCounter;
    [SerializeField] GameObject mercuryCounter;
    [SerializeField] GameObject sulfurCounter;
    [SerializeField] GameObject crystalCounter;

    private TextMeshProUGUI displayedGold;
    private TextMeshProUGUI displayedWood;
    private TextMeshProUGUI displayedOre;
    private TextMeshProUGUI displayedGems;
    private TextMeshProUGUI displayedMercury;
    private TextMeshProUGUI displayedSulfur;
    private TextMeshProUGUI displayedCrystals;

    void Start()
    {
        TurnManager.OnNewPlayerTurn += UpdateDisplay;
        PlayerManager.OnCurrentPlayerResourcesGained += UpdateDisplay;
        displayedGold = goldCounter.GetComponent<TextMeshProUGUI>();
        displayedWood = woodCounter.GetComponent<TextMeshProUGUI>();
        displayedOre = oreCounter.GetComponent<TextMeshProUGUI>();
        displayedGems = gemCounter.GetComponent<TextMeshProUGUI>();
        displayedMercury = mercuryCounter.GetComponent<TextMeshProUGUI>();
        displayedSulfur = sulfurCounter.GetComponent<TextMeshProUGUI>();
        displayedCrystals = crystalCounter.GetComponent<TextMeshProUGUI>();
    }

    private void UpdateDisplay (Player _currentPlayer)
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

    private void UpdateDisplay ()
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
