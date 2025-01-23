using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISword : UIGearPiece
{
    [SerializeField] private float[] meleeDamageGrowthValues;
    public float currentDamageGrowth;

    public override void UpgradePlayerStats()
    {
        currentDamageGrowth += meleeDamageGrowthValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == meleeDamageGrowthValues.Length)
        {
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextMeleeGrowth = meleeDamageGrowthValues[currentIndex];
            string meleeGrowthText = $"Melee Damage: +{nextMeleeGrowth}\n";
            return meleeGrowthText;
        }
        else
        {
            float previousMeleeGrowth = meleeDamageGrowthValues[currentLevelIndex - 1];
            float nextMeleeGrowth = meleeDamageGrowthValues[currentLevelIndex];
            string meleeGrowthText = $"Melee Damage: +{previousMeleeGrowth} -> +{nextMeleeGrowth}\n";            
            return meleeGrowthText;
        }
    }
}
