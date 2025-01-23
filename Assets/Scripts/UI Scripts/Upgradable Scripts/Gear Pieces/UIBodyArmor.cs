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
            float healthGrowth = currentLevelIndex == 0 ? healthGrowthValues[currentLevelIndex] : currentHealthGrowth;
            float armorGrowth = Mathf.Abs(100 * (currentLevelIndex == 0 ? armorGrowthValues[currentLevelIndex] : currentArmorGrowth));

            string healthGrowthText = $"Health: +{healthGrowth}\n";
            string armorGrowthText = $"Armor: +{armorGrowth.ToString("0.0")}%\n";
            return healthGrowthText + armorGrowthText;
        }
        else
        {
            float previousHealthGrowth = currentHealthGrowth;
            float previousArmorGrowth = Mathf.Abs(100 * currentArmorGrowth);

            float nextHealthGrowth = currentHealthGrowth + healthGrowthValues[currentLevelIndex];
            float nextArmorGrowth = Mathf.Abs(100 * (currentArmorGrowth - armorGrowthValues[currentLevelIndex]));

            string healthGrowthText = $"Health: +{previousHealthGrowth} -> +{nextHealthGrowth}\n";
            string armorGrowthText = $"Armor: +{previousArmorGrowth.ToString("0.0")}% -> +{nextArmorGrowth.ToString("0.0")}%\n";
            return healthGrowthText + armorGrowthText;
        }
    }
}
