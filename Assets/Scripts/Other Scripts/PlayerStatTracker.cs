using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatTracker : MonoBehaviour
{
    //Armor values
    private float basePlayerMaxHP;
    private float basePlayerArmor;
    private float basePlayerMoveSpeed;
    private float basePlayerMeleeDamage;
    private float basePlayerLifeSteal;

    //Ranged attack values
    private float basePlayerBulletCD;
    private float basePlayerBulletCost;
    private float basePlayerBulletDamage;
    private float basePlayerBulletVelocity;
    private float basePlayerBulletExplosionDamage;

    //Dash values
    private float basePlayerDashCD;
    private float basePlayerDashCost;
    private float basePlayerDashBulletDamage;

    //Shield values
    private float basePlayerShieldCD;
    private float basePlayerShieldCost;
    private float basePlayerShieldFlat;

    [Header("Gear pieces")]
    [SerializeField] private UIBodyArmor chestplate;
    [SerializeField] private UIBodyArmor leggings;
    [SerializeField] private UIBoots boots;
    [SerializeField] private UISword sword;

    [Header("Abilities")]
    [SerializeField] private UIDash dash;
    [SerializeField] private UIRangedAttack rangedAttack;
    [SerializeField] private UIShield shield;

    [Header("Text Objects")]
    [SerializeField] private TextMeshProUGUI playerStats;
    [SerializeField] private TextMeshProUGUI playerBulletStats;
    [SerializeField] private TextMeshProUGUI playerDashStats;
    [SerializeField] private TextMeshProUGUI playerShieldStats;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        basePlayerMaxHP = player.maxHealth;
        basePlayerArmor = player.armor;
        basePlayerMoveSpeed = player.speed;
        basePlayerMeleeDamage = player.meleeDamage;
        basePlayerLifeSteal = player.lifeSteal;

        basePlayerBulletCD = player.rangedAttackCD;
        basePlayerBulletCost = player.rangedAttackCost;
        basePlayerBulletDamage = player.rangedDamage;
        basePlayerBulletVelocity = player.rangedAttackVelocity;
        basePlayerBulletExplosionDamage = player.rangedAttackExplosionDamage;

        basePlayerDashCD = player.dashCD;
        basePlayerDashCost = player.dashHealthCost;
        basePlayerDashBulletDamage = player.dashProjectileDamage;

        basePlayerShieldCD = player.shieldCD;
        basePlayerShieldCost = player.shieldMaxHPCost;
        basePlayerShieldFlat = player.fixedShieldAmount;
    }

    private void Update()
    {
        UpdatePlayerStats();
    }

    private void UpdatePlayerStats()
    {
        //Gear
        player.maxHealth = basePlayerMaxHP + chestplate.currentHealthGrowth + 
            leggings.currentHealthGrowth + boots.currentHealthGrowth;
        player.armor = basePlayerArmor + chestplate.currentArmorGrowth + 
            leggings.currentArmorGrowth + boots.currentArmorGrowth;
        player.speed = basePlayerMoveSpeed + boots.currentSpeedGrowth;
        player.meleeDamage = basePlayerMeleeDamage + sword.currentDamageGrowth;
        player.lifeSteal = basePlayerLifeSteal + sword.currentLifeStealGrowth;

        string playerHealth = $"Health: {(int)player.health}/{player.maxHealth}\n";
        string playerArmor = $"Armor: {((100 / player.armor) - 100).ToString("0.0")}%\n";
        string playerSpeed = $"Move Speed: {player.speed}\n";
        string playerMeleeDamage = $"Melee Damage: {player.meleeDamage}\n";
        string playerLifeSteal = $"Melee Life Steal: {player.lifeSteal} hp\n";

        playerStats.text = "Stats: \n" + playerHealth + playerArmor + playerSpeed + playerMeleeDamage + playerLifeSteal;

        //Abilities
        player.dashCD = basePlayerDashCD + dash.currentDashCD;
        player.dashHealthCost = basePlayerDashCost + dash.currentDashCost;
        player.dashProjectileDamage = basePlayerDashBulletDamage + dash.currentDashDamage;

        string dashCD = $"Cooldown: {player.dashCD}s\n";
        float dashHealthCost = player.health * player.dashHealthCost;
        string dashCost = $"Health Cost: {(100 * player.dashHealthCost).ToString("0.0")}% of current HP ({dashHealthCost.ToString("0.0")} hp)\n";
        string dashDamage = $"Projectile Damage: {player.dashProjectileDamage}\n";

        playerDashStats.text = "Dash stats: \n" + dashCD + dashCost + dashDamage;

        player.rangedAttackCD = basePlayerBulletCD + rangedAttack.currentBulletCD;
        player.rangedAttackCost = basePlayerBulletCost + rangedAttack.currentBulletCost;
        player.rangedDamage = basePlayerBulletDamage + rangedAttack.currentBulletDamage;
        player.rangedAttackVelocity = basePlayerBulletVelocity + rangedAttack.currentBulletVelocity;
        player.rangedAttackExplosionDamage = basePlayerBulletExplosionDamage + rangedAttack.currentBulletExplosionDamage;

        string bulletCD = $"Cooldown: {player.rangedAttackCD}s\n";
        float bulletHealthCost = player.maxHealth * player.rangedAttackCost;
        string bulletCost = $"Health Cost: {(100 * player.rangedAttackCost).ToString("0.0")}% of max HP ({bulletHealthCost.ToString("0.0")} hp)\n";
        string bulletDamage = $"Damage: {player.rangedDamage}\n";
        string bulletVelocity = $"Bullet Speed: {player.rangedAttackVelocity}\n";
        string bulletExplosionDamage = $"Explosion Damage: {player.rangedAttackExplosionDamage}\n";

        playerBulletStats.text = "Blood Bullet stats: \n" + bulletCD + bulletCost + bulletDamage + bulletVelocity + bulletExplosionDamage;

        player.shieldCD = basePlayerShieldCD + shield.currentShieldCD;
        player.shieldMaxHPCost = basePlayerShieldCost + shield.currentShieldCost;
        player.fixedShieldAmount = basePlayerShieldFlat + shield.currentShieldFlat;

        string shieldCD = $"Cooldown: {player.shieldCD}s\n";
        float shieldHealthCost = player.maxHealth * player.shieldMaxHPCost;
        string shieldCost = $"Health Cost: {(100 * player.shieldMaxHPCost).ToString("0.0")}% of max HP ({shieldHealthCost.ToString("0.0")} hp)\n";
        string shieldDuration = $"Duration: {player.shieldDuration}\n";
        string fixedShield = $"Flat Amount: {player.fixedShieldAmount}\n";
        float missingHP = (player.maxHealth - player.health) * player.percentageMissingHealthShield;
        
        string shieldStrength = $"Strength: {(100 * player.percentageMissingHealthShield).ToString("0.0")}% missing HP ({missingHP.ToString("0.0")} hp) + {player.fixedShieldAmount}\n";

        playerShieldStats.text = "Shield stats: \n" + shieldCD + shieldCost + shieldDuration + fixedShield + shieldStrength;

    }
}
