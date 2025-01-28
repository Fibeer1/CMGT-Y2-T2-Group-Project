using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityUnlocker : MonoBehaviour
{
    private Player player;
    private HUD hud;
    private CharacterScreen charScreen;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        hud = FindObjectOfType<HUD>();
        charScreen = FindObjectOfType<CharacterScreen>();
    }

    public void UnlockPlayerAbility(int abilityIndex)
    {
        player.abilityLockArray[abilityIndex] = true;
        charScreen.RefreshAbilities();
        hud.RefreshAbilities();
    }
}
