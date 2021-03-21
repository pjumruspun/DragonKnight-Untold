using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper
{
    public static void TimeStop(float duration)
    {
        Time.timeScale = 0.0f;
        CoroutineUtility.ExecDelay(() => Time.timeScale = 1.0f, duration, realTime: true);
    }

    public static void StopAnimator(Animator animator, float duration)
    {
        animator.speed = 0.0f;
        CoroutineUtility.ExecDelay(() => animator.speed = 1.0f, duration);
    }
}
