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
    [SerializeField] private TextMeshProUGUI[] materialCountTexts;
    [SerializeField] private TextMeshProUGUI[] gearPieceTexts; //0 item name, 1 rarity, 2 stats, 3 costs
    [SerializeField] private TextMeshProUGUI[] abilityTexts; //0 ability name, 1 level, 2 stats, 3 costs
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

    public void UpdateUI(string itemName, string rarity, string stats, string cost)
    {
        gearPieceTexts[0].text = itemName;
        gearPieceTexts[1].text = rarity;
        gearPieceTexts[2].text = stats;
        gearPieceTexts[3].text = cost;
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
        Debug.Log(currentGearPiece.name + " clicked.");
        string nextRarity = GetNextRarity(rarities[currentGearPiece.currentRarityIndex]);
        if (currentGearPiece.currentRarityIndex == rarities.Length - 1)
        {
            UpdateUI(currentGearPiece.gearPieceName, nextRarity, string.Empty, string.Empty);
            return;
        }

        StringBuilder stats = new StringBuilder();
        if (currentGearPiece.healthGrowth > 0)
        {
            stats.Append($"Health: +{currentGearPiece.healthGrowth}\n");
        }
        if (currentGearPiece.armorGrowth > 0)
        {
            stats.Append($"Armor: +{(10 * currentGearPiece.armorGrowth).ToString("0.00")}%\n");
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
        UpdateUI(currentGearPiece.gearPieceName, nextRarity, stats.ToString(), costsText.ToString());
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
