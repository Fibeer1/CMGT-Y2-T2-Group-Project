using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBodyArmor : UIGearPiece
{
    [SerializeField] private float[] healthGrowthValues;
    [SerializeField] private float[] armorGrowthValues;
    public float currentHealthGrowth;
    public float currentArmorGrowth;

    public override void UpgradePlayerStats()
    {
        currentHealthGrowth += healthGrowthValues[currentLevelIndex];
        currentArmorGrowth -= armorGrowthValues[currentLevelIndex];
        player.health += currentHealthGrowth;
        if (player.health > player.maxHealth)
        {
            player.health = player.maxHealth;
        }
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == healthGrowthValues.Length)
        {
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextHealthGrowth = healthGrowthValues[currentIndex];
            float nextArmorGrowth = 100 * armorGrowthValues[currentIndex];
            string healthGrowthText = $"Health: +{nextHealthGrowth}\n";
            string armorGrowthText = $"Armor: +{nextArmorGrowth.ToString("0.0")}%\n";
            return healthGrowthText + armorGrowthText;
        }
        else
        {
            float previousHealthGrowth = healthGrowthValues[currentLevelIndex - 1];
            float previousArmorGrowth = 100 * armorGrowthValues[currentLevelIndex - 1];
            float nextHealthGrowth = healthGrowthValues[currentLevelIndex];
            float nextArmorGrowth = 100 * armorGrowthValues[currentLevelIndex];
            string healthGrowthText = $"Health: +{previousHealthGrowth} -> +{nextHealthGrowth}\n";
            string armorGrowthText = $"Armor: +{previousArmorGrowth.ToString("0.0")}% -> +{nextArmorGrowth.ToString("0.0")}%\n";
            return healthGrowthText + armorGrowthText;
        }
    }
}
