using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : MonoBehaviour
{
    [SerializeField] private protected CharacterScreen characterScreen;
    public int currentLevelIndex;
    public string itemName;
    private protected Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public virtual void UpgradePlayerStats()
    {
        currentLevelIndex++;
    }

    public virtual void CheckMaterialCosts()
    {

    }

    public virtual string GetLevel()
    {
        return string.Empty;
    }

    public virtual string GetStats()
    {
        return string.Empty;
    }

    public virtual int[] GetMaterialCosts(int levelIndex)
    {
        return new int[3];
    }
}
