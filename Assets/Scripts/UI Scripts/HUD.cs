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
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        healthSlider.maxValue = player.maxHealth;
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
}
