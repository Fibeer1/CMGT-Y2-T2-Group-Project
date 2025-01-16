using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;
    [SerializeField] private GameObject gameOverMenu;
    private void Awake() => Instance = this;
    private IEnumerator OnDeath()
    {
        Fader.Fade(true, 0.5f, 0);
        yield return new WaitForSeconds(1);
        gameOverMenu.SetActive(true);
    }
    public static void DeathAnimation() => Instance.StartCoroutine(Instance.OnDeath());
}
