using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRangedAttack : UIAbility
{
    [SerializeField] private float[] bulletCDValues;
    [SerializeField] private float[] bulletCostValues;
    [SerializeField] private float[] bulletDamageValues;
    [SerializeField] private float[] bulletVelocityValues;
    [SerializeField] private float[] bulletExplosionDamageValues;

    public float currentBulletCD;
    public float currentBulletCost;
    public float currentBulletDamage;
    public float currentBulletVelocity;
    public float currentBulletExplosionDamage;

    public override void UpgradePlayerStats()
    {
        currentBulletCD -= bulletCDValues[currentLevelIndex];
        currentBulletCost -= bulletCostValues[currentLevelIndex];
        currentBulletDamage += bulletDamageValues[currentLevelIndex];
        currentBulletVelocity += bulletVelocityValues[currentLevelIndex];
        currentBulletExplosionDamage += bulletExplosionDamageValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == bulletCDValues.Length)
        {
            float nextBulletCD = Mathf.Abs(currentLevelIndex == 0 ? bulletCDValues[currentLevelIndex] : currentBulletCD);
            float nextBulletCost = Mathf.Abs(100 * (currentLevelIndex == 0 ? bulletCostValues[currentLevelIndex] : currentBulletCost));
            float nextBulletDamage = currentLevelIndex == 0 ? bulletDamageValues[currentLevelIndex] : currentBulletDamage;
            float nextBulletVelocity = currentLevelIndex == 0 ? bulletVelocityValues[currentLevelIndex] : currentBulletVelocity;
            float nextBulletExplosionDamage = currentLevelIndex == 0 ? bulletExplosionDamageValues[currentLevelIndex] : currentBulletExplosionDamage;

            string bulletCDText = $"Cooldown: -{nextBulletCD}s\n";
            string bulletCostText = $"Health Cost: -{nextBulletCost.ToString("0.0")}%\n";
            string bulletDamageText = $"Damage: +{nextBulletDamage}\n";
            string bulletVelocityText = $"Velocity: +{nextBulletVelocity}\n";
            string bulletExplosionDamageText = $"Explosion Damage: +{nextBulletExplosionDamage}\n";
            return bulletCDText + bulletCostText + bulletDamageText + bulletVelocityText + bulletExplosionDamageText;
        }
        else
        {
            float previousBulletCD = Mathf.Abs(currentBulletCD);
            float previousBulletCost = Mathf.Abs(100 * currentBulletCost);
            float previousBulletDamage = currentBulletDamage;
            float previousBulletVelocity = currentBulletVelocity;
            float previousBulletExplosionDamage = currentBulletExplosionDamage;

            float nextBulletCD = Mathf.Abs(currentBulletCD - bulletCDValues[currentLevelIndex]);
            float nextBulletCost = Mathf.Abs(100 * (currentBulletCost - bulletCostValues[currentLevelIndex]));
            float nextBulletDamage = currentBulletDamage + bulletDamageValues[currentLevelIndex];
            float nextBulletVelocity = currentBulletVelocity + bulletVelocityValues[currentLevelIndex];
            float nextBulletExplosionDamage = currentBulletExplosionDamage + bulletExplosionDamageValues[currentLevelIndex];

            string bulletCDText = $"Cooldown: -{previousBulletCD}s -> -{nextBulletCD}s\n";
            string bulletCostText = $"Health Cost: -{previousBulletCost.ToString("0.0")}% -> -{nextBulletCost.ToString("0.0")}%\n";
            string bulletDamageText = $"Damage: +{previousBulletDamage} -> +{nextBulletDamage}\n";
            string bulletVelocityText = $"Velocity: +{previousBulletVelocity} -> +{nextBulletVelocity}\n";
            string bulletExplosionDamageText = $"Explosion Damage: +{previousBulletExplosionDamage} -> +{nextBulletExplosionDamage}\n";
            return bulletCDText + bulletCostText + bulletDamageText + bulletVelocityText + bulletExplosionDamageText;
        }
    }
}
