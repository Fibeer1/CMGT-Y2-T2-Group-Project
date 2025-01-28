using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Player player;

    [Header("HUD Elements")]
    [SerializeField] private Image MeleeCDOverlay;
    [SerializeField] private Image DashCDOverlay;
    [SerializeField] private Image RangedCDOverlay;
    [SerializeField] private Image ShieldCDOverlay;
    [SerializeField] private Slider healthSlider;

    [Header("HUD Icons")]
    [SerializeField] private GameObject[] abilityIcons;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        healthSlider.maxValue = player.maxHealth;
        RefreshAbilities();
    }

    private void Update()
    {
        MeleeCDOverlay.fillAmount = player.swordSwingCDTimer / player.swordSwingCD;
        DashCDOverlay.fillAmount = player.dashCDTimer / player.dashCD;
        RangedCDOverlay.fillAmount = player.rangedAttackCDTimer / player.rangedAttackCD;
        ShieldCDOverlay.fillAmount= player.shieldCDTimer / player.shieldCD;
        healthSlider.value = player.health;
        healthSlider.maxValue = player.maxHealth;
    }

    public void RefreshAbilities()
    {
        for (int i = 0; i < player.abilityLockArray.Length; i++)
        {
            if (player.abilityLockArray[i])
            {
                abilityIcons[i].SetActive(true);
            }
            else
            {
                abilityIcons[i].SetActive(false);
            }
        }        
    }
}
