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
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextBulletCD = Mathf.Abs(currentBulletCD - bulletCDValues[currentIndex]);
            float nextBulletCost = Mathf.Abs(100 * (currentBulletCost - bulletCostValues[currentIndex]));
            float nextBulletDamage = currentBulletDamage + bulletDamageValues[currentIndex];
            float nextBulletVelocity = currentBulletVelocity + bulletVelocityValues[currentIndex];
            float nextBulletExplosionDamage = currentBulletExplosionDamage + bulletExplosionDamageValues[currentIndex];

            string bulletCDText = $"Cooldown: -{nextBulletCD}s\n";
            string bulletCostText = $"Health Cost: -{nextBulletCost.ToString("0.0")}%\n";
            string bulletDamageText = $"Damage: +{nextBulletDamage}\n";
            string bulletVelocityText = $"Velocity: +{nextBulletVelocity}\n";
            string bulletExplosionDamageText = $"Explosion Damage: +{nextBulletExplosionDamage}\n";
            return bulletCDText + bulletCostText + bulletDamageText + bulletVelocityText + bulletExplosionDamageText;
        }
        else
        {
            float previousBulletCD = Mathf.Abs(bulletCDValues[currentLevelIndex - 1]);
            float previousBulletCost = Mathf.Abs(100 * bulletCostValues[currentLevelIndex - 1]);
            float previousBulletDamage = bulletDamageValues[currentLevelIndex - 1];
            float previousBulletVelocity = bulletVelocityValues[currentLevelIndex - 1];
            float previousBulletExplosionDamage = bulletExplosionDamageValues[currentLevelIndex - 1];

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
