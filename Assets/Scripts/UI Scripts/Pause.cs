using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Pause : MonoBehaviour
{
    public bool paused = false;
    public GameObject pauseMenu;
    private CharacterScreen characterScreen;
    private Player player;

    private void Start()
    {
        pauseMenu.SetActive(false);
        characterScreen = GetComponent<CharacterScreen>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (characterScreen.isMenuOpen)
            {
                characterScreen.ToggleCharacterScreen(false);
                return;
            }
            if (paused)
            {
                Resume();
            }
            else if (!paused)
            {
                PauseGame();
            }
        }
    }

    public void Resume()
    {
        player.enabled = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        paused = false;
    }

    public void PauseGame()
    {
        player.enabled = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        paused = true;
    }
    
    public void ResetLevel()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Application.Quit();
    }
}
