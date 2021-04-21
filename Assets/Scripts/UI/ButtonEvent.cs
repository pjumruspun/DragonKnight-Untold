using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void StartGame()
    {
        LevelChanger.LoadSceneInstant(Scenes.Camp);
    }

    public void ResumeGame()
    {
        GameEvents.TriggerPause(false);
    }

    public void RestartGame()
    {
        GameEvents.TriggerRestartGame();
    }

    public void ResetTestScene()
    {
        DragonGaugeStatic.dragonEnergy = 100.0f;
        LevelChanger.LoadSceneInstant(Scenes.TestScene);
    }

    public void MainMenu()
    {
        GameEvents.TriggerResetGame();
        LevelChanger.LoadSceneInstant(Scenes.MainMenu);
    }

    public void ShowOptions()
    {
        Debug.Log("Showing options!");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
