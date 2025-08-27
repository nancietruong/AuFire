using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController player;

    public static event Action OnGameReset;
    public static GameState State { get; private set; } = GameState.Playing;

    void Init()
    {
        player.Init();
    }

    void Start()
    {
        Init();
        player.OnPlayerDeath += HandlePlayerDeath;
        State = GameState.Playing;
    }

    private void HandlePlayerDeath()
    {
        UIManager.Instance.ShowGameOverScreen();
        State = GameState.GameOver;
    }

    public void ResetGame()
    {
        OnGameReset?.Invoke();
        State = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
