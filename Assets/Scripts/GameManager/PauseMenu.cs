using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoSingleton<PauseMenu>
{
    [SerializeField]
    private GameObject pauseMenu;
    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        ShowPauseMenu(false);
        GameEvents.Pause += ProcessPause;
    }

    private void OnDestroy()
    {
        GameEvents.Pause -= ProcessPause;
    }

    private void Update()
    {
        ListenToPauseSignal();
    }

    private void ListenToPauseSignal()
    {
        if (InputManager.Pause && GameStateManager.State == GameState.Gameplay)
        {
            GameEvents.TriggerPause(!isPaused);
        }
    }

    private void ProcessPause(bool pause)
    {
        // Change isPause inside this function
        isPaused = pause;

        if (pause)
        {
            // ZA WARUDOOO
            Time.timeScale = 0.0f;
        }
        else
        {
            // Soshite toki wa ugaki dasu
            Time.timeScale = 1.0f;
        }

        ShowPauseMenu(pause);

    }

    private void ShowPauseMenu(bool show)
    {
        pauseMenu.SetActive(show);
    }
}
