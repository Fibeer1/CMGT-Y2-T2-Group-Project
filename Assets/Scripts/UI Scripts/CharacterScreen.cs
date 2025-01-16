using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterScreen : MonoBehaviour
{
    [SerializeField] private GameObject characterScreen;
    private bool isMenuOpen = false;
    private Player player;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI ironCountText;
    [SerializeField] private TextMeshProUGUI platinumCountText;
    [SerializeField] private TextMeshProUGUI bloodiumCountText;

    private string selectedPart = "";

    // Equipment stats and rarities
    private int chestplateArmor = 5;
    private int chestplateHealth = 50;
    private string chestplateRarity = "FirstCommon";
    private int chestplateArmorGrowth = 10;
    private int chestplateHealthGrowth = 25;

    private int leggingsArmor = 5;
    private int leggingsHealth = 50;
    private string leggingsRarity = "FirstCommon";
    private int leggingsArmorGrowth = 3;
    private int leggintsHealthGrowth = 10;

    private int bootsArmor = 8;
    private int bootsHealth = 80;
    private string bootsRarity = "FirstCommon";
    private int bootsArmorGrowth = 3;
    private int bootsHealthGrowth = 5;

    private int swordDamage = 25;
    private string swordRarity = "FirstCommon";
    private int swordDamageGrowth;
    private int currentSwordUpgrade = 0;
    [SerializeField] private int[] swordMatTier1Costs;
    [SerializeField] private int[] swordMatTier2Costs;
    [SerializeField] private int[] swordMatTier3Costs;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        characterScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isMenuOpen)
            {
                ToggleCharacterScreen(false);
            }
            else
            {
                ToggleCharacterScreen(true);
            }
        }
        ironCountText.text = player.materialCounts[0] + "x";
        platinumCountText.text = player.materialCounts[1] + "x";
        bloodiumCountText.text = player.materialCounts[2] + "x";
    }

    public void ToggleCharacterScreen(bool shouldEnableScreen)
    {
        Time.timeScale = shouldEnableScreen ? 0 : 1;
        isMenuOpen = shouldEnableScreen;
        characterScreen.SetActive(shouldEnableScreen);
    }

    public void UpdateUI(string rarity, string stats, string cost)
    {
        rarityText.text = rarity;
        statsText.text = stats;
        costText.text = cost;
    }

    private string GetNextRarity(string currentRarity)
    {
        if (currentRarity == "Legendary")
        {
            return "<color=yellow>Legendary (MAX)</color>";
        }

        switch (currentRarity)
        {
            case "FirstCommon":
                return "<color=#696969>Common</color> -> <color=#2E8B57>Uncommon</color>";
            case "Common":
                return "<color=#2E8B57>Uncommon</color> -> <color=blue>Rare</color>";
            case "Uncommon":
                return "<color=blue>Rare</color> -> <color=purple>Epic</color>";
            case "Rare":
                return "<color=purple>Epic</color> -> <color=yellow>Legendary</color>";
            default:
                return "<color=yellow>Legendary (MAX)</color>";
        }
    }

    public void OnChestPlateClicked()
    {
        selectedPart = "ChestPlate";
        string nextRarity = GetNextRarity(chestplateRarity);
        if (chestplateRarity == "Legendary")
        {
            UpdateUI(nextRarity, $"Armor: {chestplateArmor}\nHealth: {chestplateHealth}", "Cost: N/A");
        }
        else
        {
            UpdateUI(
                nextRarity,
                $"Armor: {chestplateArmor} -> {chestplateArmor + 5}\nHealth: {chestplateHealth} -> {chestplateHealth + 50}",
                GetUpgradeCost(chestplateRarity)
            );
        }
    }

    public void OnLeggingsClicked()
    {
        selectedPart = "Leggings";
        string nextRarity = GetNextRarity(leggingsRarity);
        if (leggingsRarity == "Legendary")
        {
            UpdateUI(nextRarity, $"Armor: {leggingsArmor}\nHealth: {leggingsHealth}", "Cost: N/A");
        }
        else
        {
            UpdateUI(
                nextRarity,
                $"Armor: {leggingsArmor} -> {leggingsArmor + 5}\nHealth: {leggingsHealth} -> {leggingsHealth + 50}",
                GetUpgradeCost(leggingsRarity)
            );
        }
    }

    public void OnBootsClicked()
    {
        selectedPart = "Boots";
        string nextRarity = GetNextRarity(bootsRarity);
        if (bootsRarity == "Legendary")
        {
            UpdateUI(nextRarity, $"Armor: {bootsArmor}\nHealth: {bootsHealth}", "Cost: N/A");
        }
        else
        {
            UpdateUI(
                nextRarity,
                $"Armor: {bootsArmor} -> {bootsArmor + 4}\nHealth: {bootsHealth} -> {bootsHealth + 40}",
                GetUpgradeCost(bootsRarity)
            );
        }
    }

    public void OnSwordClicked()
    {
        selectedPart = "Sword";
        string nextRarity = GetNextRarity(swordRarity);
        if (swordRarity == "Legendary")
        {
            UpdateUI(nextRarity, $"Damage: {swordDamage}", "Cost: N/A");
        }
        else
        {
            UpdateUI(
                nextRarity,
                $"Damage: {swordDamage} -> {swordDamage + 10}",
                GetUpgradeCost(swordRarity)
            );
        }
    }

    public void OnUpgradeButtonClicked()
    {
        if (string.IsNullOrEmpty(selectedPart))
        {
            Debug.LogWarning("No equipment part selected for upgrade.");
            return;
        }

        bool canUpgrade = false;

        switch (selectedPart)
        {
            case "ChestPlate":
                canUpgrade = CheckMaterialsAndUpgrade(1, 1, 0);
                if (canUpgrade)
                {
                    chestplateArmor += 5;
                    chestplateHealth += 50;
                    UpgradeRarity(ref chestplateRarity);
                    player.UpdatePlayerStats(player.maxHealth + 50, player.armor + 5);
                    OnChestPlateClicked();
                }
                break;

            case "Leggings":
                canUpgrade = CheckMaterialsAndUpgrade(1, 1, 0);
                if (canUpgrade)
                {
                    leggingsArmor += 5;
                    leggingsHealth += 50;
                    UpgradeRarity(ref leggingsRarity);
                    player.UpdatePlayerStats(player.maxHealth + 50, player.armor + 5);
                    OnLeggingsClicked();
                }
                break;

            case "Boots":
                canUpgrade = CheckMaterialsAndUpgrade(1, 1, 0);
                if (canUpgrade)
                {
                    bootsArmor += 4;
                    bootsHealth += 40;
                    UpgradeRarity(ref bootsRarity);
                    player.UpdatePlayerStats(player.maxHealth + 40, player.armor + 4);
                    OnBootsClicked();
                }
                break;

            case "Sword":
                canUpgrade = CheckMaterialsAndUpgrade(2, 1, 0);
                if (canUpgrade)
                {
                    swordDamage += 10;
                    UpgradeRarity(ref swordRarity);
                    player.meleeDamage = swordDamage;
                    OnSwordClicked();
                }
                break;

            default:
                Debug.LogWarning("Unknown equipment part selected for upgrade.");
                break;
        }

        if (!canUpgrade)
        {
            Debug.LogWarning("Not enough materials to upgrade.");
        }
    }

    private void UpgradeRarity(ref string rarity)
    {
        switch (rarity)
        {
            case "FirstCommon":
                rarity = "Common";
                break;
            case "Common":
                rarity = "Uncommon";
                break;
            case "Uncommon":
                rarity = "Rare";
                break;
            case "Rare":
                rarity = "Epic";
                break;
            case "Epic":
                rarity = "Legendary";
                break;
            default:
                Debug.LogWarning("Rarity is already at maximum (Legendary).");
                break;
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

    private string GetUpgradeCost(string rarity)
    {
        switch (rarity)
        {
            case "FirstCommon":
                return "Cost: 1x Iron";
            case "Common":
                return "Cost: 1x Iron\n1x Platinum";
            case "Uncommon":
                return "Cost: 2x Iron\n2x Platinum";
            case "Rare":
                return "Cost: 3x Iron\n3x Platinum";
            case "Epic":
                return "Cost: 5x Iron\n5x Platinum";
            default:
                return "Cost: N/A";
        }
    }
}
