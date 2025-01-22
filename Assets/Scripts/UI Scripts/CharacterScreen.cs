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
        int ironCost = GetGearUpgradeCosts()[0];
        int platinumCost = GetGearUpgradeCosts()[1];
        int bloodCost = GetGearUpgradeCosts()[2];
        bool canUpgrade = CheckMaterialsAndUpgrade(ironCost, platinumCost, bloodCost);

        if (!canUpgrade)
        {
            Debug.Log("Missing required materials to upgrade.");
            return;
        }
        float healthStat = CheckGearStat(0);
        float armorStat = CheckGearStat(1);
        float moveSpeedStat = CheckGearStat(2);
        float meleeDamageStat = CheckGearStat(3);

        player.UpdatePlayerStats(healthStat, armorStat, moveSpeedStat, meleeDamageStat);
        currentGearPiece.currentRarityIndex++;
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
        float healthGrowth = CheckGearStat(0);
        float armorGrowth = CheckGearStat(1);
        float moveSpeedGrowth = CheckGearStat(2);
        float meleeDamageGrowth = CheckGearStat(3);

        if (healthGrowth > 0)
        {
            stats.Append($"Health: +{healthGrowth}\n");
        }
        if (armorGrowth > 0)
        {
            stats.Append($"Armor: +{(100 * armorGrowth).ToString("0.00")}%\n");
        }
        if (moveSpeedGrowth > 0)
        {
            stats.Append($"Speed: +{moveSpeedGrowth}\n");
        }
        if (meleeDamageGrowth > 0)
        {
            stats.Append($"Melee Damage: +{meleeDamageGrowth}\n");
        }
        StringBuilder costsText = new StringBuilder();
        costsText.Append("Cost:\n");
        int ironCost = GetGearUpgradeCosts()[0];
        int platinumCost = GetGearUpgradeCosts()[1];
        int bloodCost = GetGearUpgradeCosts()[2];
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
            costsText.Append($"{bloodCost} Crystals\n");
        }
        UpdateGearPieceUI(currentGearPiece.gearPieceName, nextRarity, stats.ToString(), costsText.ToString());
    }

    private float CheckGearStat(int statIndex)
    {
        if (currentGearPiece.currentRarityIndex < currentGearPiece.statArrays[statIndex].Length)
        {
            return currentGearPiece.statArrays[statIndex][currentGearPiece.currentRarityIndex];
        }
        else
        {
            return 0;
        }
    }

    private int[] GetGearUpgradeCosts()
    {
        if (currentGearPiece != null)
        {
            return currentGearPiece.materialCosts[currentGearPiece.currentRarityIndex];
        }
        else
        {
            Debug.Log("ERROR: No gear piece to get costs from.");
            return new int[3];
        }

    }

    public void UpdateAbilityUI(string itemName, string level, string stats, string cost)
    {
        abilityTexts[0].text = itemName;
        abilityTexts[1].text = "Level: " + level;
        abilityTexts[2].text = stats;
        abilityTexts[3].text = cost;
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
        int ironCost = GetAbilityUpgradeCosts()[0];
        int platinumCost = GetAbilityUpgradeCosts()[1];
        int bloodCost = GetAbilityUpgradeCosts()[2];
        bool canUpgrade = CheckMaterialsAndUpgrade(ironCost, platinumCost, bloodCost);

        if (!canUpgrade)
        {
            Debug.Log("Missing required materials to upgrade.");
            return;
        }

        currentAbility.UpgradePlayerAbility(currentAbility.abilityName);
        currentAbility.currentLevelIndex++;
        currentAbility.SetCurrentPlayerStats();
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
        currentAbility.SetCurrentPlayerStats();
        StringBuilder stats = new StringBuilder();
        float currentAbilityCooldown = CheckAbilityStat(0);
        float currentAbilityCost = CheckAbilityStat(1);
        float currentAbilityDamage = CheckAbilityStat(2);
        if (currentAbilityCooldown > 0)
        {
            stats.Append($"Cooldown Duration: {currentAbility.playerCurrentCD} -> {currentAbilityCooldown}\n");
        }
        if (currentAbilityCost > 0)
        {
            stats.Append($"Health Cost: {currentAbility.playerCurrentCost} -> {currentAbilityCost}\n");
        }
        if (currentAbilityDamage > 0)
        {
            stats.Append($"Damage: {currentAbility.playerCurrentDamage} -> {currentAbilityDamage}\n");
        }
        StringBuilder costsText = new StringBuilder();
        costsText.Append("Cost:\n");
        int ironCost = GetAbilityUpgradeCosts()[0];
        int platinumCost = GetAbilityUpgradeCosts()[1];
        int bloodCost = GetAbilityUpgradeCosts()[2];
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
            if (bloodCost == 1)
            {
                costsText.Append($"{bloodCost} Crystal\n");
            }
            else
            {
                costsText.Append($"{bloodCost} Crystals\n");
            }
        }
        UpdateAbilityUI(currentAbility.abilityName, nextLevel.ToString(), 
            stats.ToString(), costsText.ToString());
    }

    private float CheckAbilityStat(int statIndex)
    {
        if (currentAbility.currentLevelIndex < currentAbility.statArrays[statIndex].Length)
        {
            return currentAbility.statArrays[statIndex][currentAbility.currentLevelIndex];
        }
        else
        {
            return 0;
        }
    }

    private int[] GetAbilityUpgradeCosts()
    {
        if (currentAbility != null)
        {
            return currentAbility.materialCosts[currentAbility.currentLevelIndex];
        }
        else
        {
            Debug.Log("ERROR: No ability to get costs from.");
            return new int[3];
        }
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
}
