using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff // IDisposable to free memory?
{
    public bool IsExpired => isExpired;
    public float DurationRatio => duration / initDuration;
    public float DurationLeft => duration;
    public event System.Action OnStart;
    public event System.Action OnUpdate;
    public event System.Action Callback;
    private float initDuration;
    private float duration = 0.0f;
    private bool isExpired;

    public Buff(float duration)
    {
        this.initDuration = duration;
        this.duration = duration;
        isExpired = false;
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public virtual void Start()
    {
        OnStart?.Invoke();
    }

    public virtual void Update()
    {
        OnUpdate?.Invoke();
        duration -= Time.deltaTime;
        if (duration < 0.0f)
        {
            End();
        }
    }

    public virtual void End()
    {
        isExpired = true;
        Callback?.Invoke();

        // Then unsub all
        Unsubscribe(OnStart);
        Unsubscribe(OnUpdate);
        Unsubscribe(Callback);

        // Free memory
    }

    private void Unsubscribe(System.Action action)
    {
        if (action != null)
        {
            foreach (var d in action.GetInvocationList())
            {
                action -= (d as System.Action);
            }
        }
    }
}
