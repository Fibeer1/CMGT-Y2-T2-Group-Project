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
        UpdateUI(
            nextRarity,
            $"Armor: {chestplateArmor} -> {chestplateArmor + 5}\nHealth: {chestplateHealth} -> {chestplateHealth + 50}",
            "Cost: 1x Iron\n1x Platinum"
        );
    }

    public void OnLeggingsClicked()
    {
        selectedPart = "Leggings";
        string nextRarity = GetNextRarity(leggingsRarity);
        UpdateUI(
            nextRarity,
            $"Armor: {leggingsArmor} -> {leggingsArmor + 5}\nHealth: {leggingsHealth} -> {leggingsHealth + 50}",
            "Cost: 1x Iron\n1x Platinum"
        );
    }

    public void OnBootsClicked()
    {
        selectedPart = "Boots";
        string nextRarity = GetNextRarity(bootsRarity);
        UpdateUI(
            nextRarity,
            $"Armor: {bootsArmor} -> {bootsArmor + 4}\nHealth: {bootsHealth} -> {bootsHealth + 40}",
            "Cost: 1x Iron\n1x Platinum"
        );
    }

    public void OnSwordClicked()
    {
        selectedPart = "Sword";
        string nextRarity = GetNextRarity(swordRarity);
        UpdateUI(
            nextRarity,
            $"Damage: {swordDamage} -> {swordDamage + 10}",
            "Cost: 2x Iron\n1x Platinum"
        );
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
                UpgradeEquipment(ref chestplateRarity, ref chestplateArmor, chestplateHealth, 5, 50);
                OnChestPlateClicked();
                break;

            case "Leggings":
                UpgradeEquipment(ref leggingsRarity, ref leggingsArmor, leggingsHealth, 5, 50);
                OnLeggingsClicked();
                break;

            case "Boots":
                UpgradeEquipment(ref bootsRarity, ref bootsArmor, bootsHealth, 4, 40);
                OnBootsClicked();
                break;

            case "Sword":
                UpgradeEquipment(ref swordRarity, ref swordDamage, null, 10, 0);
                OnSwordClicked();
                break;

            default:
                Debug.LogWarning("Unknown equipment part for upgrade.");
                break;
        }
    }

    private void UpgradeEquipment(ref string rarity, ref int primaryStat, int? secondaryStat, int primaryIncrement, int secondaryIncrement)
    {

        if (rarity == "Epic")
        {
            Debug.LogWarning("Equipment is already at Legendary rarity.");
            return;
        }

        primaryStat += primaryIncrement;


        if (secondaryStat.HasValue)
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
                Debug.LogWarning("Rarity is already at maximum.");
                break;
        }
    }
}
