using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoots : UIGearPiece
{
    [SerializeField] private float[] healthGrowthValues;
    [SerializeField] private float[] armorGrowthValues;
    [SerializeField] private float[] moveSpeedGrowthValues;

    public override void UpgradePlayerStats()
    {
        player.maxHealth += healthGrowthValues[currentLevelIndex];
        player.armor -= armorGrowthValues[currentLevelIndex];
        player.speed += moveSpeedGrowthValues[currentLevelIndex];
        player.health += healthGrowthValues[currentLevelIndex];
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
            float nextSpeedGrowth = moveSpeedGrowthValues[currentIndex];
            string healthGrowthText = $"Health Increase: +{nextHealthGrowth}\n";
            string armorGrowthText = $"Armor: +{nextArmorGrowth.ToString("0.0")}%\n";
            string speedGrowthText = $"Speed Increase: +{nextSpeedGrowth}\n";
            return healthGrowthText + armorGrowthText + speedGrowthText;
        }
        else
        {
            float previousHealthGrowth = healthGrowthValues[currentLevelIndex - 1];
            float previousArmorGrowth = 100 * armorGrowthValues[currentLevelIndex - 1];
            float previousSpeedGrowth = moveSpeedGrowthValues[currentLevelIndex - 1];
            float nextHealthGrowth = healthGrowthValues[currentLevelIndex];
            float nextArmorGrowth = 100 * armorGrowthValues[currentLevelIndex];
            float nextSpeedGrowth = moveSpeedGrowthValues[currentLevelIndex];
            string healthGrowthText = $"Health Increase: +{previousHealthGrowth} -> +{nextHealthGrowth}\n";
            string armorGrowthText = $"Armor Increase: +{previousArmorGrowth.ToString("0.0")}% -> +{nextArmorGrowth.ToString("0.0")}%\n";
            string speedGrowthText = $"Speed Increase: +{previousSpeedGrowth} -> +{nextSpeedGrowth}\n";
            return healthGrowthText + armorGrowthText + speedGrowthText;
        }
    }
}