using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartGameSequence());                
    }

    private IEnumerator StartGameSequence()
    {
        Fader.Fade(true, 1, 0);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Application.Quit();
    }
}
