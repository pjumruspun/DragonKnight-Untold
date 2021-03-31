using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to EventPublisher
public class GameEvents
{
    public delegate void OnPause(bool pause);
    public delegate void OnCompleteLevel();
    public delegate void OnMoveToNextLevel();

    public static event OnPause Pause;
    public static event OnCompleteLevel CompleteLevel;
    public static event OnMoveToNextLevel MoveToNextLevel;

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
        Debug.Log("triggering...");
        MoveToNextLevel?.Invoke();
    }
}
