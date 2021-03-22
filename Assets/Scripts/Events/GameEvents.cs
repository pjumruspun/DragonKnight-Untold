using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to EventPublisher
public class GameEvents
{
    public delegate void OnPause(bool pause);

    public static event OnPause Pause;

    public static void TriggerPause(bool pause)
    {
        Pause?.Invoke(pause);
    }
}
