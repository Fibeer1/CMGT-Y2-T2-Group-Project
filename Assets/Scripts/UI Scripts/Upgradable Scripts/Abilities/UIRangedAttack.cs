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

    public override void UpgradePlayerStats()
    {
        player.rangedAttackCD -= bulletCDValues[currentLevelIndex];
        player.rangedAttackCost -= bulletCostValues[currentLevelIndex];
        player.rangedDamage += bulletDamageValues[currentLevelIndex];
        player.rangedAttackVelocity += bulletVelocityValues[currentLevelIndex];
        player.rangedAttackExplosionDamage += bulletExplosionDamageValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == bulletCDValues.Length)
        {
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextBulletCD = bulletCDValues[currentIndex];
            float nextBulletCost = 100 * bulletCostValues[currentIndex];
            float nextBulletDamage = bulletDamageValues[currentIndex];
            float nextBulletVelocity = bulletVelocityValues[currentIndex];
            float nextBulletExplosionDamage = bulletExplosionDamageValues[currentIndex];
            string bulletCDText = $"Cooldown: -{nextBulletCD}s\n";
            string bulletCostText = $"Health Cost: -{nextBulletCost.ToString("0.0")}%\n";
            string bulletDamageText = $"Damage: +{nextBulletDamage}\n";
            string bulletVelocityText = $"Velocity: +{nextBulletVelocity}\n";
            string bulletExplosionDamageText = $"Explosion Damage: +{nextBulletExplosionDamage}\n";
            return bulletCDText + bulletCostText + bulletDamageText + bulletVelocityText + bulletExplosionDamageText;
        }
        else
        {
            float previousBulletCD = bulletCDValues[currentLevelIndex - 1];
            float previousBulletCost = 100 * bulletCostValues[currentLevelIndex - 1];
            float previousBulletDamage = bulletDamageValues[currentLevelIndex - 1];
            float previousBulletVelocity = bulletVelocityValues[currentLevelIndex - 1];
            float previousBulletExplosionDamage = bulletExplosionDamageValues[currentLevelIndex - 1];
            float nextBulletCD = bulletCDValues[currentLevelIndex];
            float nextBulletCost = 100 * bulletCostValues[currentLevelIndex];
            float nextBulletDamage = bulletDamageValues[currentLevelIndex];
            float nextBulletVelocity = bulletVelocityValues[currentLevelIndex];
            float nextBulletExplosionDamage = bulletExplosionDamageValues[currentLevelIndex];
            string bulletCDText = $"Cooldown: -{previousBulletCD}s -> -{nextBulletCD}s\n";
            string bulletCostText = $"Health Cost: -{previousBulletCost.ToString("0.0")}% -> -{nextBulletCost.ToString("0.0")}%\n";
            string bulletDamageText = $"Damage: +{previousBulletDamage} -> +{nextBulletDamage}\n";
            string bulletVelocityText = $"Velocity: +{previousBulletVelocity} -> +{nextBulletVelocity}\n";
            string bulletExplosionDamageText = $"Explosion Damage: +{previousBulletExplosionDamage} -> +{nextBulletExplosionDamage}\n";
            return bulletCDText + bulletCostText + bulletDamageText + bulletVelocityText + bulletExplosionDamageText;
        }
    }
}
