using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragonSkills : PlayerSkills
{
    private const float slashAnimLength = 0.3f;
    private const float skill1KnockUpAmplitude = 3.0f;
    private const float skill1KnockBackAmplitude = 2.0f;
    private const float skill1AnticipationRatio = 0.3f;
    private const float skill1LockMovementRatio = 0.6f;
    private const float skill1AnimStopRatio = 0.2f;
    private AttackHitbox dragonPrimaryHitbox;
    private AttackHitbox fireBreathHitbox;
    private float[] dragonAttackDamage = new float[4]
    {
        30.0f,
        30.0f,
        30.0f,
        30.0f
    };
    private float[] dragonAttackCooldown = new float[4]
    {
        0.7f,
        1.0f,
        1.0f,
        1.0f
    };

    private float dragonSuperArmorAttack = 100.0f;
    private GameObject fireBreath;
    private GameObject clawSlash;
    private Animator clawSlashAnim;
    private Coroutine fireBreathCoroutine;

    public DragonSkills(
        Transform transform,
        AttackHitbox dragonPrimaryHitbox,
        GameObject fireBreath,
        GameObject clawSlash
    ) : base(transform)
    {
        this.dragonPrimaryHitbox = dragonPrimaryHitbox;
        this.fireBreath = fireBreath;
        this.fireBreathHitbox = fireBreath.GetComponent<AttackHitbox>();
        this.fireBreath.SetActive(false);

        this.clawSlash = clawSlash;
        this.clawSlash.SetActive(false);

        if (this.clawSlash.TryGetComponent<Animator>(out Animator animator))
        {
            this.clawSlashAnim = animator;
        }
        else
        {
            throw new System.NullReferenceException("Cannot get Animator component from clawSlash GameObject");
        }
    }

    public override float[] GetCurrentCooldown()
    {
        return currentCooldown;
    }

    public override float CurrentCooldownPercentage(int skillNumber)
    {
        return currentCooldown[skillNumber] / dragonAttackCooldown[skillNumber];
    }

    public override void Skill1()
    {
        currentCooldown[0] = dragonAttackCooldown[0];

        // Primary attack = dragonAttackDamage[0]
        float damage = dragonAttackDamage[0];

        // Dragon Primary Attack
        // Night dragon is just a place holder for now
        float animLength = PlayerAnimation.Instance.GetAnimLength(0);

        // Lock movement
        movement.LockMovementBySkill(skill1LockMovementRatio * animLength, true, true);
        movement.LockJumpBySkill(skill1LockMovementRatio * animLength);

        // Actual attack damage applied
        float attackDelay = skill1AnticipationRatio * animLength;

        CoroutineUtility.Instance.ExecDelay(() =>
        {
            float totalDamage = AttackWithHitbox(
                dragonPrimaryHitbox,
                damage,
                dragonSuperArmorAttack,
                knockUpAmplitude: skill1KnockUpAmplitude,
                knockBackAmplitude: skill1KnockBackAmplitude,
                hitEffect: HitEffect.Slash
            );

            if (totalDamage > 0.0f)
            {
                float timeStopDuration = animLength * skill1AnimStopRatio;
                // Stop player's animator
                TimeStopper.StopAnimator(PlayerAnimation.Instance.GetAnimator, timeStopDuration);

                // Stop claw slash's animator
                TimeStopper.StopAnimator(clawSlashAnim, timeStopDuration);
            }

        }, attackDelay);

        // Claw slash effect
        // On
        CoroutineUtility.Instance.ExecDelay(() =>
        {
            this.clawSlash.SetActive(true);
        }, attackDelay / 2);

        // Off
        CoroutineUtility.Instance.ExecDelay(() =>
        {
            this.clawSlash.SetActive(false);
        }, attackDelay + slashAnimLength);
    }

    public override void Skill2()
    {
        currentCooldown[1] = dragonAttackCooldown[1];

        // Skill 2 = dragonAttackDamage[1]
        float damage = dragonAttackDamage[1];

        // Dragon Skill 2
        fireBreathCoroutine = CoroutineUtility.Instance.CreateCoroutine(DelayedFireBreath(0.05f, 0.33f));
        Debug.Log("Fire breath start");
        movement.LockJumpBySkill(true);
        movement.LockFlipBySkill(true);
        movement.LockMovementBySkill(true);
    }

    public void Skill2Release()
    {
        fireBreath.SetActive(false);
        if (fireBreathCoroutine != null)
        {
            CoroutineUtility.Instance.KillCoroutine(fireBreathCoroutine);
        }

        // Debug.Log("Fire breath stop");
        movement.LockJumpBySkill(false);
        movement.LockFlipBySkill(false);
        movement.LockMovementBySkill(false);
    }

    private IEnumerator DelayedFireBreath(float delay, float interval)
    {
        yield return new WaitForSeconds(delay);
        fireBreath.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(interval);
            AttackWithHitbox(fireBreathHitbox, 10.0f, 0.0f, 1.25f);
        }
    }
}