using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGearPiece : MonoBehaviour
{
    [SerializeField] private CharacterScreen characterScreen;
    public List<float[]> statArrays = new List<float[]>();
    public float[] healthGrowthValues;
    public float[] armorGrowthValues;
    public float[] moveSpeedGrowthValues;
    public float[] meleeDamageGrowthValues;
    public string gearPieceName;
    [SerializeField] private bool shouldSelectOnStart = false;

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
        statArrays.Add(healthGrowthValues);
        statArrays.Add(armorGrowthValues);
        statArrays.Add(moveSpeedGrowthValues);
        statArrays.Add(meleeDamageGrowthValues);

        materialCosts.Add(uncommonUpgradeMaterialCosts);
        materialCosts.Add(rareUpgradeMaterialCosts);
        materialCosts.Add(epicUpgradeMaterialCosts);
        materialCosts.Add(legendaryUpgradeMaterialCosts);
        if (shouldSelectOnStart)
        {
            characterScreen.OnGearPieceClick(gameObject);
        }
    }
}
