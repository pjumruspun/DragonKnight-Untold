using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoSingleton<GameStateManager>
{
    public static GameState State => Instance.gameState;
    private GameState gameState;

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

        if (scene.name == LevelChanger.SceneNames[Scenes.MainMenu])
        {
            gameState = GameState.MainMenu;
        }
        else if (scene.name == LevelChanger.SceneNames[Scenes.Tutorial])
        {
            gameState = GameState.Gameplay;
            GameEvents.TriggerPause(false);
        }
        else if (scene.name == LevelChanger.SceneNames[Scenes.Camp])
        {
            gameState = GameState.Gameplay;
            GameEvents.TriggerPause(false);
            CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.CampBGM), 0.5f);
        }
        else if (scene.name.Split('_')[0] == "Level")
        {
            gameState = GameState.Gameplay;
            GameEvents.TriggerPause(false);
            CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.StageBGM), 0.5f);
        }
        else if (scene.name.Split('_')[0] == "Boss")
        {
            gameState = GameState.Gameplay;
            GameEvents.TriggerPause(false);
            CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.BossBGM), 0.5f);
        }
        else
        {
            gameState = GameState.Gameplay;
            Debug.LogWarning("Loaded an unregistered scene");
        }
    }
}
