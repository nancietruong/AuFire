using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject winGameScreen;
    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void ShowWinGameScreen()
    {
        if (winGameScreen != null)
        {
            winGameScreen.SetActive(true);
        }

    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
        if (winGameScreen != null)
        {
            winGameScreen.SetActive(false);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
