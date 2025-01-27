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
    [SerializeField] private GameObject dashIcon;
    [SerializeField] private GameObject projectileIcon;
    [SerializeField] private GameObject shieldIcon;

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


        //Awful code, remake it to a method which gets called when the player kills a miniboss
        if (player.canDash)
        {
            dashIcon.SetActive(true);
        }
        else
        {
            dashIcon.SetActive(false);
        }
        if (player.canShootBullet)
        {
            projectileIcon.SetActive(true);
        }
        else
        {
            projectileIcon.SetActive(false);
        }
        if (player.canUseShield)
        {
            shieldIcon.SetActive(true);
        }
        else
        {
            shieldIcon.SetActive(false);
        }
    }
}
