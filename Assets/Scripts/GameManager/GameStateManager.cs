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
        BossEvents.BossDead += PlayVictoryTheme;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        BossEvents.BossDead -= PlayVictoryTheme;
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
            CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.CampBGM), 1.0f);
        }
        else if (scene.name.Split('_')[0] == "Level")
        {
            gameState = GameState.Gameplay;
            GameEvents.TriggerPause(false);
            CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.StageBGM), 1.0f);
        }
        else if (scene.name.Split('_')[0] == "Boss")
        {
            gameState = GameState.Gameplay;
            GameEvents.TriggerPause(false);
            CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.BossBGM), 1.0f);
        }
        else
        {
            gameState = GameState.Gameplay;
            Debug.LogWarning("Loaded an unregistered scene");
        }
    }

    private void PlayVictoryTheme(Boss boss)
    {
        SoundManager.Stop(SFXName.BossBGM);
        CoroutineUtility.ExecDelay(() => SoundManager.Play(SFXName.VictoryBGM), 0.3f);
    }
}
