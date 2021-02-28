using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public bool IsExpired;
    private float duration = 0.0f;
    private bool isExpired;

    public Buff(float duration)
    {
        this.duration = duration;
        isExpired = false;
        OnStart();
    }

    public virtual void OnStart()
    {
        Debug.Log("Start!");
    }

    public virtual void OnUpdate()
    {
        duration -= Time.deltaTime;
        if (duration < 0.0f)
        {
            OnEnd();
        }
    }

    public virtual void OnEnd()
    {
        isExpired = true;
        Debug.Log("End!");
    }
}
