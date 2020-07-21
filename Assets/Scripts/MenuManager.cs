using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        ObjectPool.Initialize();
    }

    public void HandlePlayButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void HandleQuitButton()
    {
        Application.Quit();
    }

    public void HandlePlayAgainButton()
    {
        ObjectPool.DestoryAllBullets();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void HandleHelpButton()
    {
        SceneManager.LoadScene("HelpScene");
    }

    public void HandleBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
