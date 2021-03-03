using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public bool IsExpired => isExpired;
    private float duration = 0.0f;
    private bool isExpired;

    public Buff(float duration)
    {
        this.duration = duration;
        isExpired = false;
        Start();
    }

    public virtual void Start()
    {
        Debug.Log("Start!");
    }

    public virtual void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0.0f)
        {
            End();
        }
    }

    public virtual void End()
    {
        isExpired = true;
        // Debug.Log("End!");
    }
}
