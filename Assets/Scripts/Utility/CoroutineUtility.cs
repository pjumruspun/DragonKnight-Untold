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

    public Coroutine ExecDelay(System.Action func, float delay)
    {
        return StartCoroutine(ExecDelayPrivate(func, delay));
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

    private IEnumerator ExecDelayPrivate(System.Action func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func?.Invoke();
    }

    private IEnumerator ExecAtEndFramePrivate(System.Action func)
    {
        yield return new WaitForEndOfFrame();
        func?.Invoke();
    }
}
