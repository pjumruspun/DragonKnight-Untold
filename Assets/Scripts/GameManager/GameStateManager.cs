using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoSingleton<GameStateManager>
{
    public GameState State => gameState;
    private GameState gameState;

    public void LoadMainMenu()
    {
        gameState = GameState.MainMenu;
        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        gameState = GameState.Gameplay;
        // Load game scene
        SceneManager.LoadScene("Gameplay");
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
