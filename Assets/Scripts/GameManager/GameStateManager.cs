using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoSingleton<GameStateManager>
{
    public GameState State => gameState;
    private GameState gameState;
    private const string mainMenuScene = "MainMenu";
    private const string gameplayScene = "Gameplay";

    public void LoadMainMenu()
    {
        gameState = GameState.MainMenu;
        // Unpause the game
        GameEvents.TriggerPause(false);
        // Load main menu
        SceneManager.LoadScene(mainMenuScene);
    }

    public void StartGame()
    {
        gameState = GameState.Gameplay;
        // Load game scene
        SceneManager.LoadScene(gameplayScene);
        // Unpause the game
        GameEvents.TriggerPause(false);
    }

    protected override void Awake()
    {
        base.Awake();
        try
        {
            DontDestroyOnLoad(gameObject);
        }
        catch
        {
            // Do nothing
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Adjust scene if faulty
        switch (scene.name)
        {
            case mainMenuScene:
                gameState = GameState.MainMenu;
                break;
            case gameplayScene:
                gameState = GameState.Gameplay;
                break;
            default:
                Debug.LogWarning("Something wrong may be happening");
                break;
        }
    }
}
