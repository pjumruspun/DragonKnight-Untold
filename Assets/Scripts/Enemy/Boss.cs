using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    [Header("Soul Reward")]
    [SerializeField]
    private int soulGivenOnDeath = 1200 * StageManager.currentWorld;
    private static readonly float timeFreezeOnDead = 0.0f;
    private static readonly float screenShakeDurOnDead = 0.4f;
    private static readonly float screenShakePowOnDead = 0.2f;

    public override void TakeDamage(
        float damage, bool crit,
        float superArmorDamage = 0.0f,
        float knockUpAmplitude = 0.0f,
        float knockBackAmplitude = 0.0f,
        bool shouldFlinch = false
    )
    {
        // Instead, boss takes SA damage equals to damage received
        base.TakeDamage(damage, crit, damage, knockUpAmplitude, knockBackAmplitude);
    }

    protected override void KnockedBack(float amplitude)
    {
        // Boss doesn't get knocked
    }

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

    protected override void HandleSuperArmorUIChange()
    {
        BossEvents.TriggerBossSAChange(this);
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        BossEvents.TriggerBossDead(this);

        ShowEffectsAfterDead();

        // Complete level in boss level
        GameEvents.TriggerCompleteLevel();
    }

    protected override void PlayerSoulGain()
    {
        // Experimental value
        SoulStatic.soul += soulGivenOnDeath;
        GameEvents.TriggerSoulChange(soulGivenOnDeath);
    }

    protected override void Flinch()
    {
        // Boss cannot flinch from attacks
    }

    private void ShowEffectsAfterDead()
    {
        // Stop time for a little, and shake the screen
        TimeStopper.TimeStop(timeFreezeOnDead);
        CoroutineUtility.ExecDelay(() =>
            ScreenShake.Instance.StartShaking(screenShakeDurOnDead, screenShakePowOnDead),
            timeFreezeOnDead,
            true
        );
    }

    private void DisableFloatingBars()
    {
        hpBar.gameObject.SetActive(false);
        superArmorBar.gameObject.SetActive(false);
    }
}
