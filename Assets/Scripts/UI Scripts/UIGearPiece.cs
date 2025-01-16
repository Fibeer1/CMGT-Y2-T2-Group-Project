using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGearPiece : MonoBehaviour
{
    [SerializeField] private CharacterScreen characterScreen;
    public float healthGrowth;
    public float armorGrowth;
    public float moveSpeedGrowth;
    public float meleeDamageGrowth;

    public int currentRarityIndex;

    //all arrays have a length of 3, each index represents the materials
    //and its number represents the number of required materials
    public List<int[]> materialCosts = new List<int[]>();
    public int[] uncommonUpgradeMaterialCosts;
    public int[] rareUpgradeMaterialCosts;
    public int[] epicUpgradeMaterialCosts;
    public int[] legendaryUpgradeMaterialCosts;

    private void Start()
    {
        materialCosts.Add(uncommonUpgradeMaterialCosts);
        materialCosts.Add(rareUpgradeMaterialCosts);
        materialCosts.Add(epicUpgradeMaterialCosts);
        materialCosts.Add(legendaryUpgradeMaterialCosts);
    }

    public void OnClick()
    {

    }
}
