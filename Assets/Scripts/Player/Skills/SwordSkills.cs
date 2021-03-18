using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkills : PlayerSkills
{
    public int CurrentCombo => currentCombo;

    // Skill 1 params
    private readonly float[] skill1PushSpeed = new float[3] { 50.0f, 175.0f, 50.0f };
    private readonly float[] skill1KnockBackAmplitudes = new float[3] { 2.0f, 4.0f, 2.0f };
    private const float skill1KnockUpAmplitude = 2.0f;
    private const float skill1PushDurationRatio = 0.1f;
    private const float skill1LockMovementRatio = 0.7f;
    private const float skill1AnticipationRatio = 0.3f;

    // Skill 2 params
    private const float swordSkill2LockMovementRatio = 1.0f;
    private const float skill2AnticipationRatio = 0.5f;

    // Skill 3 params
    private const float skill3DashAnticipationRatio = 1.0f;
    private const float skill3AttackAnticipationRatio = 0.35f;
    private const float skill3SpeedMultiplier = 3.5f;
    private const float skill3KnockAmplitude = 2.0f;
    private const float skill3CooldownResetRatio = 0.6f;

    private AttackHitbox swordPrimaryHitbox;

    // Combo stuff
    private const float resetComboRatio = 1.5f; // 1.5 times of attack anim length
    private int currentCombo = 0;
    private float lastAttackTime = 0.0f;

    public SwordSkills(
        Transform transform,
        AttackHitbox swordPrimaryHitbox
    ) : base(transform)
    {
        this.swordPrimaryHitbox = swordPrimaryHitbox;
    }

    // Sword auto attack combo
    public override void Skill1()
    {
        base.Skill1();
        float animLength = PlayerAnimation.Instance.GetAnimLength(0);
        // Process combo
        float currentTime = Time.time;
        if (currentTime - lastAttackTime <= resetComboRatio * animLength)
        {
            // Increase combo
            if (currentCombo < 2)
            {
                ++currentCombo;
            }
            else
            {
                currentCombo = 0;
            }
        }
        else
        {
            // Reset combo
            currentCombo = 0;
        }

        lastAttackTime = currentTime;
        // Debug.Log(currentCombo);

        // Primary attack = skillDamage[0]
        float damage = PlayerStats.Instance.BaseSkillDamage[0];

        // Lock player's movement and flip
        // Lock equals to animation clip length
        movement.LockMovementBySkill(animLength * skill1LockMovementRatio, false, true);
        movement.LockJumpBySkill(animLength * skill1LockMovementRatio);

        movement.MoveForwardBySkill(skill1PushSpeed[currentCombo], animLength * skill1PushDurationRatio, groundOnly: false);

        // Then attack
        float anticipationPeriod = animLength * skill1AnticipationRatio;
        CoroutineUtility.Instance.ExecDelay(() =>
            AttackWithHitbox(
                swordPrimaryHitbox,
                damage,
                knockUpAmplitude: skill1KnockUpAmplitude,
                knockBackAmplitude: skill1KnockBackAmplitudes[currentCombo],
                hitEffect: HitEffect.Slash
        ), anticipationPeriod);
    }

    // Sword wave
    public override void Skill2()
    {
        base.Skill2();

        // Skill 2 = skillDamage[1]
        float damage = PlayerStats.Instance.BaseSkillDamage[1];

        // Sword wave
        // Lock player's movement according to anim length
        float animLength = PlayerAnimation.Instance.GetAnimLength(1);
        movement.LockMovementBySkill(animLength * swordSkill2LockMovementRatio, true, true);

        // Spawn sword wave with delay
        CoroutineUtility.Instance.CreateCoroutine(SwordWave(damage, movement.ForwardVector, animLength * skill2AnticipationRatio));
    }

    // Dash -> Dash attack
    public override void Skill3()
    {
        base.Skill3();

        // Lock animation
        float animLength = PlayerAnimation.Instance.GetAnimLength(2);
        float lockMovementDuration = animLength * (1.0f + skill3DashAnticipationRatio);
        movement.LockMovementBySkill(lockMovementDuration, true, true);
        movement.LockJumpBySkill(lockMovementDuration);

        // Dash ahead
        CoroutineUtility.Instance.ExecDelay(() =>
        {
            movement.MoveForwardBySkill(
                PlayerStats.Instance.MovementSpeed * skill3SpeedMultiplier,
                animLength,
                groundOnly: false,
                forceMode: ForceMode2D.Impulse
            );

            DashAttack();
        }, skill3DashAnticipationRatio * animLength);
    }

    private void DashAttack()
    {
        PlayerAnimation.Instance.PlayDashAttackAnimation();

        // Lock animation
        float animLength = PlayerAnimation.Instance.GetAnimLength(1);
        movement.LockMovementBySkill(animLength, true, true);
        movement.LockJumpBySkill(animLength);

        // Skill 3 = skillDamage[2]
        float damage = PlayerStats.Instance.BaseSkillDamage[2];

        // Attack
        float anticipationPeriod = animLength * skill3AttackAnticipationRatio;
        CoroutineUtility.Instance.ExecDelay(() =>
        {
            float damageDealt = AttackWithHitbox(swordPrimaryHitbox, damage, knockUpAmplitude: skill3KnockAmplitude, hitEffect: HitEffect.Slash);
            Debug.Log(damageDealt);

            if (damageDealt > 0.0f) // If manage to hit something
            {
                // Reduce cooldown
                currentCooldown[2] = PlayerStats.Instance.SkillCooldown[2] * (1.0f - skill3CooldownResetRatio);
            }
        }, anticipationPeriod);
    }

    private IEnumerator SwordWave(float damage, Vector2 forwardVector, float delay)
    {
        yield return new WaitForSeconds(delay);
        AttackWithProjectile(
            ref ObjectManager.Instance.SwordWaves,
            damage, transform.position,
            forwardVector, knockAmplitude:
            2.0f,
            hitEffect: HitEffect.Slash
        );
    }
}
