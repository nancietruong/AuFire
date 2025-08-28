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
    [SerializeField] GameObject audioSetting;
    bool isAudioSettingOpen = false;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleAudioSettings();
        }
    }

    public void ToggleAudioSettings()
    {
        if (audioSetting != null)
        {
            isAudioSettingOpen = !isAudioSettingOpen;
            audioSetting.SetActive(isAudioSettingOpen);

            Time.timeScale = isAudioSettingOpen ? 0 : 1;
        }
    }

    public void CloseAudioSettings()
    {
        if (audioSetting != null)
        {
            audioSetting.SetActive(false);
        }
        Time.timeScale = 1;
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
