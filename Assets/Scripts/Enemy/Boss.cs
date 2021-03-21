using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
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
