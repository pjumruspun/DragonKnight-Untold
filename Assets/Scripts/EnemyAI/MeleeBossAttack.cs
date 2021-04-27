using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossAttack : EnemyAttack
{
    [Header("GameObject which contains AttackHitbox")]
    [Tooltip("GameObject name that contains AttackHitbox for this attack to work")]
    [SerializeField]
    private string attackHitboxName;

    [Header("Damage")]
    [SerializeField]
    [Tooltip("How much damage does this attack do?")]
    private float damage;

    [Header("Knock Value of this attack")]
    [SerializeField]
    private float knockBackAmplitude;
    [SerializeField]
    private float knockUpAmplitude;

    [Header("Time delay for attack to really land")]
    [SerializeField]
    private float attackDelay; // Need to adjust to animation later

    [Header("Special effect when this attack lands")]
    [Tooltip("Time temporary stop if this attack hits the player")]
    [SerializeField]
    private float timeStopDuration = 0.0f;
    [SerializeField]
    private float screenShakeDuration = 0.3f;
    [SerializeField]
    private float screenShakePower = 0.15f;
    private float calculatedDamage => damage * Difficulty.EnemyAttackScalingFactor;

    private AttackHitbox attackHitbox;

    protected override void ProcessAttack()
    {
        CoroutineUtility.ExecDelay(() => Attack(), attackDelay / enemy.ActionSpeed);
    }

    protected override void Attack()
    {
        if (attackHitbox == null)
        {
            FindAttackHitboxByName();
        }

        float damageDealt = MeleeAttack(attackHitbox, knockBackAmplitude, knockUpAmplitude, calculatedDamage);
        if (damageDealt > 0.0f)
        {
            // Hit player successfully
            ProcessTimeStop();
            ProcessScreenShake();
        }
    }

    private void FindAttackHitboxByName()
    {
        Transform attackHitboxTransform = enemy.transform.Find(attackHitboxName);
        if (attackHitboxTransform.TryGetComponent<AttackHitbox>(out AttackHitbox attackHitbox))
        {
            this.attackHitbox = attackHitbox;
        }
        else
        {
            throw new System.Exception($"{enemy.name}: Could not get AttackHitbox component from child object with name {attackHitboxName}");
        }
    }

    private void ProcessTimeStop()
    {
        bool shouldStopTime = timeStopDuration > 0.0f;
        if (shouldStopTime)
        {
            TimeStopper.TimeStop(timeStopDuration);
        }
    }

    private void ProcessScreenShake()
    {
        bool shouldScreenShake = screenShakeDuration > 0.0f && screenShakePower > 0.0f;
        if (shouldScreenShake)
        {
            ScreenShake.Instance.StartShaking(screenShakeDuration, screenShakePower);
        }
    }
}
