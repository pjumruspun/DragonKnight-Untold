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

    public void KillCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }

    public IEnumerator HideAfterSeconds(GameObject objectToHide, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        objectToHide.SetActive(false);
    }
}
