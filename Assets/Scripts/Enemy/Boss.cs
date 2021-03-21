using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private static readonly float timeFreezeOnDead = 0.5f;
    private static readonly float screenShakeDurOnDead = 0.4f;
    private static readonly float screenShakePowOnDead = 0.2f;

    protected override void EnableEnemy()
    {
        base.EnableEnemy();

        // We don't need floating hp and super armor bars
        DisableFloatingBars();

        // Instead we will use separated UI
        BossEvents.TriggerBossSpawn(this);
    }

    protected override void HandleHealthChange()
    {
        base.HandleHealthChange();
        BossEvents.TriggerBossHpChange(this);
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        BossEvents.TriggerBossDead(this);

        // Stop time for a little, and shake the screen
        TimeStopper.TimeStop(timeFreezeOnDead);
        ScreenShake.Instance.StartShaking(screenShakeDurOnDead, screenShakePowOnDead);
    }

    protected override void Flinch()
    {
        // Boss cannot flinch from attacks
    }

    private void DisableFloatingBars()
    {
        hpBar.gameObject.SetActive(false);
        superArmorBar.gameObject.SetActive(false);
    }
}
