using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper
{
    public static void TimeStop(float duration)
    {
        Time.timeScale = 0.0f;
        CoroutineUtility.Instance.ExecDelay(() => Time.timeScale = 1.0f, duration, realTime: true);
    }
}
