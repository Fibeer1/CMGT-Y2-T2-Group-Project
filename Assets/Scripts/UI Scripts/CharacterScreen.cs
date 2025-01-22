using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class CharacterScreen : MonoBehaviour
{
    [SerializeField] private GameObject characterScreen;
    public bool isMenuOpen = false;
    private Player player;

    [SerializeField] private int maxAbilityLevel;
    [SerializeField] private string[] rarities;
    [SerializeField] private TextMeshProUGUI[] materialCountTexts;
    [SerializeField] private TextMeshProUGUI[] gearPieceTexts; //0 item name, 1 rarity, 2 stats, 3 costs
    [SerializeField] private TextMeshProUGUI[] abilityTexts; //0 ability name, 1 level, 2 stats, 3 costs
    private Pause pauseMenu;

    private UIGearPiece currentGearPiece;
    private UIAbility currentAbility;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        pauseMenu = GetComponent<Pause>();
        characterScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !pauseMenu.paused)
        {
            ToggleCharacterScreen(!isMenuOpen);
        }
        for (int i = 0; i < materialCountTexts.Length; i++)
        {
            materialCountTexts[i].text = player.materialCounts[i].ToString();
        }
    }

    public void ToggleCharacterScreen(bool shouldEnableScreen)
    {
        player.enabled = !shouldEnableScreen;
        Time.timeScale = shouldEnableScreen ? 0f : 1f;
        isMenuOpen = shouldEnableScreen;
        characterScreen.SetActive(shouldEnableScreen);
    }

    public void UpdateGearPieceUI(string itemName, string rarity, string stats, string cost)
    {
        gearPieceTexts[0].text = itemName;
        gearPieceTexts[1].text = rarity;
        gearPieceTexts[2].text = stats;
        gearPieceTexts[3].text = cost;
    }

    private string GetNextGearPieceRarity(string currentRarity)
    {
        switch (currentRarity)
        {
            case "Common":
                return "<color=#696969>Common</color> -> <color=#2E8B57>Uncommon</color>";
            case "Uncommon":
                return "<color=#2E8B57>Uncommon</color> -> <color=blue>Rare</color>";
            case "Rare":
                return "<color=blue>Rare</color> -> <color=purple>Epic</color>";
            case "Epic":
                return "<color=purple>Epic</color> -> <color=yellow>Legendary</color>";
            default:
                return "<color=yellow>Legendary (MAX)</color>";
        }
    }

    public void OnGearPieceClick(GameObject gearPiece)
    {
        currentGearPiece = gearPiece.GetComponent<UIGearPiece>();
        UpdateGearPiece();
    }

    public void OnGearPieceUpgradeButtonClicked()
    {
        if (currentGearPiece == null ||
            (currentGearPiece != null &&
            currentGearPiece.currentRarityIndex == rarities.Length - 1))
        {
            Debug.Log("No equipment part selected for upgrade.");
            return;
        }
        int ironCost = GetUpgradeCosts()[0];
        int platinumCost = GetUpgradeCosts()[1];
        int bloodCost = GetUpgradeCosts()[2];
        bool canUpgrade = CheckMaterialsAndUpgrade(ironCost, platinumCost, bloodCost);

        if (!canUpgrade)
        {
            Debug.Log("Missing required materials to upgrade.");
            return;
        }
        currentGearPiece.currentRarityIndex++;
        player.UpdatePlayerStats(currentGearPiece.healthGrowthValues[currentGearPiece.currentRarityIndex], 
                        currentGearPiece.armorGrowthValues[currentGearPiece.currentRarityIndex],
                        currentGearPiece.moveSpeedGrowthValues[currentGearPiece.currentRarityIndex], 
                        currentGearPiece.meleeDamageGrowthValues[currentGearPiece.currentRarityIndex]);        
        UpdateGearPiece();
    }

    private void UpdateGearPiece()
    {
        Debug.Log(currentGearPiece.name + " clicked.");
        string nextRarity = GetNextGearPieceRarity(rarities[currentGearPiece.currentRarityIndex]);
        if (currentGearPiece.currentRarityIndex == rarities.Length - 1)
        {
            UpdateGearPieceUI(currentGearPiece.gearPieceName, nextRarity, string.Empty, string.Empty);
            return;
        }

        StringBuilder stats = new StringBuilder();
        if (currentGearPiece.healthGrowthValues[currentGearPiece.currentRarityIndex] > 0)
        {
            stats.Append($"Health: +{currentGearPiece.healthGrowthValues[currentGearPiece.currentRarityIndex]}\n");
        }
        if (currentGearPiece.armorGrowthValues[currentGearPiece.currentRarityIndex] > 0)
        {
            stats.Append($"Armor: +{(100 * currentGearPiece.armorGrowthValues[currentGearPiece.currentRarityIndex]).ToString("0.00")}%\n");
        }
        if (currentGearPiece.moveSpeedGrowthValues[currentGearPiece.currentRarityIndex] > 0)
        {
            stats.Append($"Speed: +{currentGearPiece.moveSpeedGrowthValues[currentGearPiece.currentRarityIndex]}\n");
        }
        if (currentGearPiece.meleeDamageGrowthValues[currentGearPiece.currentRarityIndex] > 0)
        {
            stats.Append($"Melee Damage: +{currentGearPiece.meleeDamageGrowthValues[currentGearPiece.currentRarityIndex]}\n");
        }
        StringBuilder costsText = new StringBuilder();
        costsText.Append("Cost:\n");
        int ironCost = GetUpgradeCosts()[0];
        int platinumCost = GetUpgradeCosts()[1];
        int bloodCost = GetUpgradeCosts()[2];
        if (ironCost > 0)
        {
            costsText.Append($"{ironCost} Iron\n");
        }
        if (platinumCost > 0)
        {
            costsText.Append($"{platinumCost} Platinum\n");
        }
        if (bloodCost > 0)
        {
            costsText.Append($"{bloodCost} Blood\n");
        }
        UpdateGearPieceUI(currentGearPiece.gearPieceName, nextRarity, stats.ToString(), costsText.ToString());
    }

    public void UpdateAbilityUI(string itemName, string level, string stats, string cost)
    {
        gearPieceTexts[0].text = itemName;
        gearPieceTexts[1].text = level;
        gearPieceTexts[2].text = stats;
        gearPieceTexts[3].text = cost;
    }

    public void OnAbilityClick(GameObject gearPiece)
    {
        currentAbility = gearPiece.GetComponent<UIAbility>();
        UpdateAbility();
    }

    public void OnAbilityUpgradeButtonClicked()
    {
        if (currentAbility == null ||
            (currentAbility != null &&
            currentAbility.currentLevelIndex == maxAbilityLevel))
        {
            Debug.Log("No ability selected for upgrade.");
            return;
        }
        int ironCost = GetUpgradeCosts()[0];
        int platinumCost = GetUpgradeCosts()[1];
        int bloodCost = GetUpgradeCosts()[2];
        bool canUpgrade = CheckMaterialsAndUpgrade(ironCost, platinumCost, bloodCost);

        if (!canUpgrade)
        {
            Debug.Log("Missing required materials to upgrade.");
            return;
        }

        currentAbility.UpgradePlayerAbility(currentAbility.abilityName);
        currentAbility.currentLevelIndex++;
        UpdateAbility();
    }

    private void UpdateAbility()
    {
        Debug.Log(currentAbility.name + " clicked.");
        int nextLevel = currentAbility.currentLevelIndex + 1;
        if (nextLevel >= maxAbilityLevel)
        {
            UpdateAbilityUI(currentAbility.abilityName, 
                nextLevel.ToString(), string.Empty, string.Empty);
            return;
        }

        StringBuilder stats = new StringBuilder();
        float currentAbilityCooldown = currentAbility.abilityCDGrowthValues[currentAbility.currentLevelIndex];
        float currentAbilityCost = currentAbility.abilityCostGrowthValues[currentAbility.currentLevelIndex];
        float currentAbilityDamage = currentAbility.abilityDamageGrowthValues[currentAbility.currentLevelIndex];
        if (currentAbilityCooldown > 0)
        {
            stats.Append($"Cooldown Duration: -{currentAbilityCooldown}\n");
        }
        if (currentAbilityCost > 0)
        {
            stats.Append($"Health Cost: +{(100 * currentAbilityCost).ToString("0.00")}%\n");
        }
        if (currentAbilityDamage > 0)
        {
            stats.Append($"Damage: +{currentAbilityDamage}\n");
        }
        StringBuilder costsText = new StringBuilder();
        costsText.Append("Cost:\n");
        int ironCost = GetUpgradeCosts()[0];
        int platinumCost = GetUpgradeCosts()[1];
        int bloodCost = GetUpgradeCosts()[2];
        if (ironCost > 0)
        {
            costsText.Append($"{ironCost} Iron\n");
        }
        if (platinumCost > 0)
        {
            costsText.Append($"{platinumCost} Platinum\n");
        }
        if (bloodCost > 0)
        {
            costsText.Append($"{bloodCost} Blood\n");
        }
        UpdateAbilityUI(currentAbility.abilityName, nextLevel.ToString(), 
            stats.ToString(), costsText.ToString());
    }

    private bool CheckMaterialsAndUpgrade(int ironRequired, int platinumRequired, int tier3Required)
    {
        if (player.materialCounts[0] >= ironRequired &&
            player.materialCounts[1] >= platinumRequired &&
            player.materialCounts[2] >= tier3Required)
        {
            player.materialCounts[0] -= ironRequired;
            player.materialCounts[1] -= platinumRequired;
            player.materialCounts[2] -= tier3Required;
            return true;
        }
        return false;
    }

    private int[] GetUpgradeCosts()
    {
        if (currentGearPiece != null)
        {
            return currentGearPiece.materialCosts[currentGearPiece.currentRarityIndex];
        }
        if (currentAbility != null)
        {
            return currentAbility.materialCosts[currentAbility.currentLevelIndex];
        }
        else
        {
            Debug.Log("ERROR: No gear piece/ability to get costs from.");
            return new int[3];
        }
    }
}
