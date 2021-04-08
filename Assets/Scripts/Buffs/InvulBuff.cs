using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulBuff : Buff
{
    public InvulBuff(float duration) : base(duration)
    {
        OnStart += MakePlayerInvul;
        Callback += MakePlayerNotInvul;

        Start();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void End()
    {
        base.End();
    }

    private void MakePlayerInvul()
    {
        PlayerHealth.Instance.SetInvul(true);
    }

    private void MakePlayerNotInvul()
    {
        PlayerHealth.Instance.SetInvul(false);
    }
}
