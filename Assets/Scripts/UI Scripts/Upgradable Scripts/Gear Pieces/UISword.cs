using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISword : UIGearPiece
{
    [SerializeField] private float[] meleeDamageGrowthValues;
    [SerializeField] private float[] lifeStealGrowthValues;
    public float currentDamageGrowth;
    public float currentLifeStealGrowth;

    public override void UpgradePlayerStats()
    {
        currentDamageGrowth += meleeDamageGrowthValues[currentLevelIndex];
        currentLifeStealGrowth += lifeStealGrowthValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == meleeDamageGrowthValues.Length)
        {
            float damageGrowth = currentLevelIndex == 0 ? meleeDamageGrowthValues[currentLevelIndex] : currentDamageGrowth;
            float lifeStealGrowth = currentLevelIndex == 0 ? lifeStealGrowthValues[currentLevelIndex] : currentLifeStealGrowth;
            string meleeGrowthText = $"Melee Damage: +{damageGrowth}\n";
            string lifeStealText = $"Life Steal: +{lifeStealGrowth} hp\n";
            return meleeGrowthText + lifeStealText;
        }
        else
        {
            float nextMeleeGrowth = currentDamageGrowth + meleeDamageGrowthValues[currentLevelIndex];
            float nextLifeStealGrowth = currentLifeStealGrowth + lifeStealGrowthValues[currentLevelIndex];
            string meleeGrowthText = $"Melee Damage: +{currentDamageGrowth} -> +{nextMeleeGrowth}\n";
            string lifeStealGrowthText = $"Life Steal: +{currentLifeStealGrowth} -> +{nextLifeStealGrowth} hp\n";
            return meleeGrowthText + lifeStealGrowthText;
        }
    }
}
