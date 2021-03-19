using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void StartGame()
    {
        LevelChanger.LoadScene(Scenes.Camp);
    }

    public void ResumeGame()
    {
        GameEvents.TriggerPause(false);
    }

    public void MainMenu()
    {
        LevelChanger.LoadScene(Scenes.MainMenu);
    }

    public void ShowOptions()
    {
        Debug.Log("Showing options!");
    }
}
