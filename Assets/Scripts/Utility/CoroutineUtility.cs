using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  For calling Coroutines in classes that doesn't have one
/// Such as StateMachineBehavior classes
/// </summary>
public class CoroutineUtility : MonoSingleton<CoroutineUtility>
{
    public Coroutine CreateCoroutine(IEnumerator function)
    {
        return StartCoroutine(function);
    }

    public static Coroutine ExecDelay(System.Action func, float delay, bool realTime = false)
    {
        return Instance.StartCoroutine(ExecDelayPrivate(func, delay, realTime));
    }

    public Coroutine ExecAtEndFrame(System.Action func)
    {
        return StartCoroutine(ExecAtEndFramePrivate(func));
    }

    public void KillCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }

    public IEnumerator HideAfterSeconds(GameObject objectToHide, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        objectToHide.SetActive(false);
    }

    private static IEnumerator ExecDelayPrivate(System.Action func, float delay, bool realTime)
    {
        if (realTime)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        func?.Invoke();
    }

    private IEnumerator ExecAtEndFramePrivate(System.Action func)
    {
        yield return new WaitForEndOfFrame();
        func?.Invoke();
    }
}
