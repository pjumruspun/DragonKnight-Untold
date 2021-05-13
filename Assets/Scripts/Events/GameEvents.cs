using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to EventPublisher
public class GameEvents
{
    public delegate void OnPause(bool pause);
    public delegate void OnCompleteLevel();
    public delegate void OnMoveToNextLevel();
    public delegate void OnRestartGame();
    public delegate void OnResetGame();
    public delegate void OnKeyAmountChange();
    public delegate void OnSoulChange(int amount);
    public delegate void OnPerkUpgrade(string perkName, int level);

    public static event OnPause Pause;
    public static event OnCompleteLevel CompleteLevel;
    public static event OnMoveToNextLevel MoveToNextLevel;
    public static event OnRestartGame RestartGame;
    public static event OnResetGame ResetGame;
    public static event OnKeyAmountChange KeyAmountChange;
    public static event OnSoulChange SoulChange;
    public static event OnPerkUpgrade PerkUpgrade;

    public static void TriggerPause(bool pause)
    {
        Pause?.Invoke(pause);
    }

    public static void TriggerCompleteLevel()
    {
        CompleteLevel?.Invoke();
    }

    public static void TriggerMoveToNextLevel()
    {
        MoveToNextLevel?.Invoke();
    }

    public static void TriggerRestartGame()
    {
        RestartGame?.Invoke();
    }

    public static void TriggerResetGame()
    {
        ResetGame?.Invoke();
    }

    public static void TriggerKeyAmountChange()
    {
        KeyAmountChange?.Invoke();
    }

    public static void TriggerSoulChange(int amount)
    {
        SoulChange?.Invoke(amount);
    }

    public static void TriggerPerkUpgrade(string perkName, int perkLevel)
    {
        PerkUpgrade?.Invoke(perkName, perkLevel);
    }
}
