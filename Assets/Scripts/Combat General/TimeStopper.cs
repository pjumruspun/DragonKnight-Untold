using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper
{
    public static void TimeStop(float duration)
    {
        // Time.timeScale = 0.0f;
        // CoroutineUtility.Instance.ExecDelay(() => Time.timeScale = 1.0f, duration, realTime: true);
        Animator playerAnim = PlayerAnimation.Instance.GetAnimator;
        playerAnim.speed = 0.0f;
        CoroutineUtility.ExecDelay(() => playerAnim.speed = 1.0f, duration);
    }

    public static void StopAnimator(Animator animator, float duration)
    {
        animator.speed = 0.0f;
        CoroutineUtility.ExecDelay(() => animator.speed = 1.0f, duration);
    }
}
