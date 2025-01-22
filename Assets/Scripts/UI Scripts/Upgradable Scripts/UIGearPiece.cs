using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGearPiece : Upgradable
{
    [SerializeField] private bool shouldSelectOnStart = false;

    //all arrays have a length of 3, each index represents the materials
    //and its number represents the number of required materials
    private List<int[]> materialCosts = new List<int[]>();
    [SerializeField] private int[] uncommonUpgradeMaterialCosts;
    [SerializeField] private int[] rareUpgradeMaterialCosts;
    [SerializeField] private int[] epicUpgradeMaterialCosts;
    [SerializeField] private int[] legendaryUpgradeMaterialCosts;


    private void Start()
    {
        materialCosts.Add(uncommonUpgradeMaterialCosts);
        materialCosts.Add(rareUpgradeMaterialCosts);
        materialCosts.Add(epicUpgradeMaterialCosts);
        materialCosts.Add(legendaryUpgradeMaterialCosts);
        if (shouldSelectOnStart)
        {
            characterScreen.OnGearPieceClick(gameObject);
        }
    }

    public override int[] GetMaterialCosts(int levelIndex)
    {
        return materialCosts[levelIndex];
    }

    public override string GetLevel()
    {
        string currentRarity = characterScreen.gearPieceRarities[currentLevelIndex];
        switch (currentRarity)
        {
            case "Common":
                return "<color=#696969>Common</color> -> <color=#2E8B57>Uncommon</color>";
            case "Uncommon":
                return "<color=#2E8B57>Uncommon</color> -> <color=blue>Rare</color>";
            case "Rare":
                return "<color=blue>Rare</color> -> <color=purple>Epic</color>";
            case "Epic":
                return "<color=purple>Epic</color> -> <color=yellow>Legendary</color>";
            default:
                return "<color=yellow>Legendary (MAX)</color>";
        }
    }
}
