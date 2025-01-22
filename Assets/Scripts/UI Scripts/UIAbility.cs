using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbility : MonoBehaviour
{
    [SerializeField] private CharacterScreen characterScreen;
    public float playerCurrentCD;
    public float playerCurrentCost;
    public float playerCurrentDamage;
    public List<float[]> statArrays = new List<float[]>();
    public float[] abilityCDGrowthValues;
    public float[] abilityCostGrowthValues;
    public float[] abilityDamageGrowthValues;
    public string abilityName;
    [SerializeField] private bool shouldSelectOnStart = false;

    public int currentLevelIndex;

    //all arrays have a length of 3, each index represents the materials
    //and its number represents the number of required materials
    public List<int[]> materialCosts = new List<int[]>();
    public int[] level1UpgradeMaterialCosts;
    public int[] level2UpgradeMaterialCosts;
    public int[] level3UpgradeMaterialCosts;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        statArrays.Add(abilityCDGrowthValues);
        statArrays.Add(abilityCostGrowthValues);
        statArrays.Add(abilityDamageGrowthValues);

        materialCosts.Add(level1UpgradeMaterialCosts);
        materialCosts.Add(level2UpgradeMaterialCosts);
        materialCosts.Add(level3UpgradeMaterialCosts);
        if (shouldSelectOnStart)
        {
            characterScreen.OnAbilityClick(gameObject);
        }
    }

    public void UpgradePlayerAbility(string abilityName)
    {
        if (abilityName == "Dash")
        {
            player.dashCD = abilityCDGrowthValues[currentLevelIndex];
            player.dashHealthCost = abilityCostGrowthValues[currentLevelIndex];
            player.dashProjectileDamage = abilityDamageGrowthValues[currentLevelIndex];
        }
        else if (abilityName == "Blood Projectile")
        {
            player.rangedAttackCD = abilityCDGrowthValues[currentLevelIndex];
            player.rangedAttackCost = abilityCostGrowthValues[currentLevelIndex];
            player.rangedDamage = abilityDamageGrowthValues[currentLevelIndex];
        }
        else if (abilityName == "Blood Shield")
        {
            player.shieldCD = abilityCDGrowthValues[currentLevelIndex];
            player.shieldMaxHPCost = abilityCostGrowthValues[currentLevelIndex];
        }        
    }

    public void SetCurrentPlayerStats()
    {
        if (abilityName == "Dash")
        {
            playerCurrentCD = player.dashCD;
            playerCurrentCost = player.dashHealthCost;
            playerCurrentDamage = player.dashProjectileDamage;
        }
        else if (abilityName == "Blood Projectile")
        {
            playerCurrentCD = player.rangedAttackCD;
            playerCurrentCost = player.rangedAttackCost;
            playerCurrentDamage = player.rangedDamage;
        }
        else if (abilityName == "Blood Shield")
        {
            playerCurrentCD = player.shieldCD;
            playerCurrentCost = player.shieldMaxHPCost;
        }
    }
}
