using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Player player;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        healthSlider.maxValue = player.maxHealth;
    }

    private void Update()
    {
        healthSlider.value = player.health;
        healthSlider.maxValue = player.maxHealth;
    }
}
