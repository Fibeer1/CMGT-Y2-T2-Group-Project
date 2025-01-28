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
    public string[] gearPieceRarities;
    [SerializeField] private TextMeshProUGUI[] materialCountTexts;
    [SerializeField] private TextMeshProUGUI[] gearPieceTexts; //0 item name, 1 rarity, 2 stats, 3 costs
    [SerializeField] private TextMeshProUGUI[] abilityTexts; //0 ability name, 1 level, 2 stats, 3 costs
    [SerializeField] private GameObject[] abilityButtons;
    private Pause pauseMenu;

    private UIGearPiece currentGearPiece;
    private UIAbility currentAbility;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        pauseMenu = GetComponent<Pause>();
        characterScreen.SetActive(false);
        RefreshAbilities();   
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

    public void RefreshAbilities()
    {
        for (int i = 0; i < player.abilityLockArray.Length; i++)
        {
            if (player.abilityLockArray[i])
            {
                abilityButtons[i].SetActive(true);
            }
            else
            {
                abilityButtons[i].SetActive(false);
            }
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

    public void UpdateAbilityUI(string itemName, string level, string stats, string cost)
    {
        abilityTexts[0].text = itemName;
        abilityTexts[1].text = level;
        abilityTexts[2].text = stats;
        abilityTexts[3].text = cost;
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
            currentGearPiece.currentLevelIndex == gearPieceRarities.Length - 1))
        {
            Debug.Log("No equipment part selected for upgrade.");
            return;
        }
        int[] materialCosts = currentGearPiece.GetMaterialCosts(currentGearPiece.currentLevelIndex);
        bool canUpgrade = CheckMaterialsAndUpgrade(materialCosts);

        if (!canUpgrade)
        {
            Debug.Log("Missing required materials to upgrade.");
            return;
        }

        currentGearPiece.UpgradePlayerStats();
        UpdateGearPiece();
    }

    private void UpdateGearPiece()
    {
        string nextRarity = currentGearPiece.GetLevel();
        string stats = currentGearPiece.GetStats();

        if (currentGearPiece.currentLevelIndex == gearPieceRarities.Length - 1)
        {
            UpdateGearPieceUI(currentGearPiece.itemName, nextRarity, stats, string.Empty);
            return;
        }

        
        int[] materialCosts = currentGearPiece.GetMaterialCosts(currentGearPiece.currentLevelIndex);
        string materialCostsText = MatCostsToText(materialCosts);
        UpdateGearPieceUI(currentGearPiece.itemName, nextRarity, stats.ToString(), materialCostsText);
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
        int[] materialCosts = currentAbility.GetMaterialCosts(currentAbility.currentLevelIndex);
        bool canUpgrade = CheckMaterialsAndUpgrade(materialCosts);

        if (!canUpgrade)
        {
            Debug.Log("Missing required materials to upgrade.");
            return;
        }

        currentAbility.UpgradePlayerStats();
        UpdateAbility();
    }

    private void UpdateAbility()
    {
        string stats = currentAbility.GetStats();
        if (currentAbility.currentLevelIndex == maxAbilityLevel)
        {
            UpdateAbilityUI(currentAbility.itemName, 
                "Level: " + currentAbility.currentLevelIndex.ToString(), stats, string.Empty);
            return;
        }
        
        int[] materialCosts = currentAbility.GetMaterialCosts(currentAbility.currentLevelIndex);
        string materialCostsText = MatCostsToText(materialCosts);
        
        UpdateAbilityUI(currentAbility.itemName, "Level: " + currentAbility.currentLevelIndex.ToString(), 
            stats.ToString(), materialCostsText);
    }

    private string MatCostsToText(int[] matCosts)
    {
        StringBuilder costsText = new StringBuilder();
        costsText.Append("Cost:\n");

        if (matCosts[0] > 0)
        {
            costsText.Append($"{matCosts[0]} Iron\n");
        }
        if (matCosts[1] > 0)
        {
            costsText.Append($"{matCosts[1]} Platinum\n");
        }
        if (matCosts[2] > 0)
        {
            if (matCosts[2] == 1)
            {
                costsText.Append($"{matCosts[2]} Crystal\n");
            }
            else
            {
                costsText.Append($"{matCosts[2]} Crystals\n");
            }
        }
        return costsText.ToString();
    }

    private bool CheckMaterialsAndUpgrade(int[] materials)
    {
        if (player.materialCounts[0] >= materials[0] &&
            player.materialCounts[1] >= materials[1] &&
            player.materialCounts[2] >= materials[2])
        {
            player.materialCounts[0] -= materials[0];
            player.materialCounts[1] -= materials[1];
            player.materialCounts[2] -= materials[2];
            return true;
        }
        return false;
    }
}
