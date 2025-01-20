using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class CharacterScreen : MonoBehaviour
{
    [SerializeField] private GameObject characterScreen;
    public bool isMenuOpen = false;
    private Player player;

    [SerializeField] private string[] rarities;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI ironCountText;
    [SerializeField] private TextMeshProUGUI platinumCountText;
    [SerializeField] private TextMeshProUGUI bloodiumCountText;
    private Pause pauseMenu;

    private UIGearPiece currentGearPiece;

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

    public void OnUpgradeButtonClicked()
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

        player.UpdatePlayerStats(currentGearPiece.healthGrowth, currentGearPiece.armorGrowth,
                        currentGearPiece.moveSpeedGrowth, currentGearPiece.meleeDamageGrowth);
        currentGearPiece.currentRarityIndex++;
        UpdateGearPiece();
    }

    private void UpdateGearPiece()
    {        
        string nextRarity = GetNextRarity(rarities[currentGearPiece.currentRarityIndex]);
        if (currentGearPiece.currentRarityIndex == rarities.Length - 1)
        {
            UpdateUI(nextRarity, string.Empty, string.Empty);
            return;
        }

        StringBuilder stats = new StringBuilder();
        if (currentGearPiece.healthGrowth > 0)
        {
            stats.Append($"Health: +{currentGearPiece.healthGrowth}\n");
        }
        if (currentGearPiece.armorGrowth > 0)
        {
            stats.Append($"Armor: +{currentGearPiece.armorGrowth}\n");
        }
        if (currentGearPiece.moveSpeedGrowth > 0)
        {
            stats.Append($"Speed: +{currentGearPiece.moveSpeedGrowth}\n");
        }
        if (currentGearPiece.meleeDamageGrowth > 0)
        {
            stats.Append($"Melee Damage: +{currentGearPiece.meleeDamageGrowth}\n");
        }
        StringBuilder costsText = new StringBuilder();
        int ironCost = GetUpgradeCosts()[0];
        int platinumCost = GetUpgradeCosts()[1];
        int bloodCost = GetUpgradeCosts()[2];
        if (ironCost > 0)
        {
            costsText.Append($"{ironCost}x Iron\n");
        }
        if (platinumCost > 0)
        {
            costsText.Append($"{platinumCost}x Platinum\n");
        }
        if (bloodCost > 0)
        {
            costsText.Append($"{bloodCost}x Blood\n");
        }
        UpdateUI(nextRarity, stats.ToString(), costsText.ToString());
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
        return currentGearPiece.materialCosts[currentGearPiece.currentRarityIndex];
    }
}
