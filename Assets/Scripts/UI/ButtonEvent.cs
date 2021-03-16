using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void StartGame()
    {
        GameStateManager.Instance.StartGame();
    }

    public void ResumeGame()
    {
        GameEvents.TriggerPause(false);
    }

    public void MainMenu()
    {
        GameStateManager.Instance.LoadMainMenu();
    }

    public void ShowOptions()
    {
        Debug.Log("Showing options!");
    }
}
