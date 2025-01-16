using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterScreen : MonoBehaviour
{
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI costText;

    private string selectedPart = "";

    // Equipment stats and rarities
    private int chestplateArmor = 5;
    private int chestplateHealth = 50;
    private string chestplateRarity = "FirstCommon";

    private int leggingsArmor = 5;
    private int leggingsHealth = 50;
    private string leggingsRarity = "FirstCommon";

    private int bootsArmor = 8;
    private int bootsHealth = 80;
    private string bootsRarity = "FirstCommon";

    private int swordDamage = 20;
    private string swordRarity = "FirstCommon";

    public void UpdateUI(string rarity, string stats, string cost)
    {
        rarityText.text = rarity;
        statsText.text = stats;
        costText.text = cost;
    }

    private string GetNextRarity(string currentRarity)
    {
        // For Legendary, just show the current stat, no next upgrade.
        if (currentRarity == "Legendary")
        {
            return "<color=yellow>Legendary (MAX)</color=yellow>";
        }

        // Otherwise, show the next rarity
        switch (currentRarity)
        {
            case "FirstCommon":
                return "<color=#696969>Common</color=#696969> -> <color=#2E8B57>Uncommon</color=#2E8B57>";
            case "Common":
                return "<color=#2E8B57>Uncommon</color=#2E8B57> -> <color=blue>Rare</color=blue>";
            case "Uncommon":
                return "<color=blue>Rare</color=blue> -> <color=purple>Epic</color=purple>";
            case "Rare":
                return "<color=purple>Epic</color=purple> -> <color=yellow>Legendary</color=yellow>";
            case "Epic":
                return "<color=yellow>Legendary (MAX)</color=yellow>";
            default:
                return "<color=yellow>Legendary (MAX)</color=yellow>";
        }
    }

    public void OnChestPlateClicked()
    {
        selectedPart = "ChestPlate";
        string nextRarity = GetNextRarity(chestplateRarity);
        if (chestplateRarity == "Legendary")
        {
            UpdateUI(nextRarity, $"Armor: {chestplateArmor}\nHealth: {chestplateHealth}", "Cost: 1x Iron\n1x Platinum");
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
            UpdateUI(nextRarity, $"Armor: {leggingsArmor}\nHealth: {leggingsHealth}", "Cost: 1x Iron\n1x Platinum");
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
            UpdateUI(nextRarity, $"Armor: {bootsArmor}\nHealth: {bootsHealth}", "Cost: 1x Iron\n1x Platinum");
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
            UpdateUI(nextRarity, $"Damage: {swordDamage}", "Cost: 2x Iron\n1x Platinum");
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

        switch (selectedPart)
        {
            case "ChestPlate":
                UpgradeEquipment(ref chestplateRarity, ref chestplateArmor, ref chestplateHealth, 5, 50);
                OnChestPlateClicked();
                break;

            case "Leggings":
                UpgradeEquipment(ref leggingsRarity, ref leggingsArmor, ref leggingsHealth, 5, 50);
                OnLeggingsClicked();
                break;

            case "Boots":
                UpgradeEquipment(ref bootsRarity, ref bootsArmor, ref bootsHealth, 4, 40);
                OnBootsClicked();
                break;

            case "Sword":
                UpgradeEquipment(ref swordRarity, ref swordDamage, 10);
                OnSwordClicked();
                break;

            default:
                Debug.LogWarning("Unknown equipment part for upgrade.");
                break;
        }
    }

    private void UpgradeEquipment(ref string rarity, ref int primaryStat, ref int secondaryStat, int primaryIncrement, int secondaryIncrement)
    {
        if (rarity == "Epic")
        {
            Debug.LogWarning("Equipment is already at Legendary rarity. Stats won't increase.");
            return;
        }

        primaryStat += primaryIncrement;

        if (secondaryStat != -1)
        {
            secondaryStat += secondaryIncrement;
        }

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

    private void UpgradeEquipment(ref string rarity, ref int primaryStat, int primaryIncrement)
    {
        if (rarity == "Epic")
        {
            Debug.LogWarning("Equipment is already at Legendary rarity. Stats won't increase.");
            return;
        }

        primaryStat += primaryIncrement;

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

    private string GetUpgradeCost(string rarity)
    {
        switch (rarity)
        {
            case "FirstCommon":
                return "Cost: 1x Iron";
            case "Common":
                return "Cost: 1x Iron\n1x Platinum";
            case "Uncommon":
                return "Cost: 1x Platinum\n1x Blood Crystal";
            case "Rare":
                return "Cost: 2x Blood Crystal";
            default:
                return "Cost: N/A";
        }
    }
}
