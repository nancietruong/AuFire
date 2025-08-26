using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController player;


    public static event Action OnGameReset;
    void Init()
    {
        player.Init();

    }

    void Start()
    {
        Init();

        player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        UIManager.Instance.ShowGameOverScreen();
    }

    public void ResetGame()
    {
        OnGameReset?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
