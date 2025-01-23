using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatTracker : MonoBehaviour
{
    //Armor values
    private float basePlayerMaxHP;
    private float basePlayerArmor;
    private float basePlayerMoveSpeed;
    private float basePlayerMeleeDamage;

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

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        basePlayerMaxHP = player.maxHealth;
        basePlayerArmor = player.armor;
        basePlayerMoveSpeed = player.speed;
        basePlayerMeleeDamage = player.meleeDamage;

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

        //Abilities
        player.dashCD = basePlayerDashCD + dash.currentDashCD;
        player.dashHealthCost = basePlayerDashCost + dash.currentDashCost;
        player.dashProjectileDamage = basePlayerDashBulletDamage + dash.currentDashDamage;

        player.rangedAttackCD = basePlayerBulletCD + rangedAttack.currentBulletCD;
        player.rangedAttackCost = basePlayerBulletCost + rangedAttack.currentBulletCost;
        player.rangedDamage = basePlayerBulletDamage + rangedAttack.currentBulletDamage;
        player.rangedAttackVelocity = basePlayerBulletVelocity + rangedAttack.currentBulletVelocity;
        player.rangedAttackExplosionDamage = basePlayerBulletExplosionDamage + rangedAttack.currentBulletExplosionDamage;

        player.shieldCD = basePlayerShieldCD + shield.currentShieldCD;
        player.shieldMaxHPCost = basePlayerShieldCost + shield.currentShieldCost;
        player.fixedShieldAmount = basePlayerShieldFlat + shield.currentShieldFlat;

    }
}
