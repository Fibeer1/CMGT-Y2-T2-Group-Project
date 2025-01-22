using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbility : Upgradable
{
    [SerializeField] private bool shouldSelectOnStart = false;

    //all arrays have a length of 3, each index represents the materials
    //and its number represents the number of required materials
    public List<int[]> materialCosts = new List<int[]>();
    public int[] level1UpgradeMaterialCosts;
    public int[] level2UpgradeMaterialCosts;
    public int[] level3UpgradeMaterialCosts;

    private void Start()
    {
        materialCosts.Add(level1UpgradeMaterialCosts);
        materialCosts.Add(level2UpgradeMaterialCosts);
        materialCosts.Add(level3UpgradeMaterialCosts);
        if (shouldSelectOnStart)
        {
            characterScreen.OnAbilityClick(gameObject);
        }
    }

    public override int[] GetMaterialCosts(int levelIndex)
    {
        return materialCosts[levelIndex];
    }
}
