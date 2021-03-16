using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkills : PlayerSkills
{
    public int CurrentCombo => currentCombo;

    private readonly float[] skill1PushSpeed = new float[3] { 50.0f, 175.0f, 50.0f };
    private readonly float[] knockAmplitude = new float[3] { 2.0f, 4.0f, 2.0f };
    private const float skill1PushDurationRatio = 0.1f;
    private const float skill1LockMovementRatio = 0.7f;
    private const float skill1AnticipationRatio = 0.3f;
    private float swordSkill2LockMovementRatio = 1.0f;
    private float skill2AnticipationRatio = 0.5f;
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

    public override void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        base.Skill1(currentPlayerPosition, forwardVector);
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
        CoroutineUtility.Instance.CreateCoroutine(
            AttackWithHitbox(
                swordPrimaryHitbox,
                damage,
                knockUpAmplitude: 2.0f,
                knockBackAmplitude: knockAmplitude[currentCombo],
                delay: animLength * skill1AnticipationRatio)
        );
    }

    public override void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        base.Skill2(currentPlayerPosition, forwardVector);

        // Skill 2 = skillDamage[1]
        float damage = PlayerStats.Instance.BaseSkillDamage[1];

        // Sword wave
        // Lock player's movement according to anim length
        float animLength = PlayerAnimation.Instance.GetAnimLength(1);
        movement.LockMovementBySkill(animLength * swordSkill2LockMovementRatio, true, true);

        // Spawn sword wave with delay
        CoroutineUtility.Instance.CreateCoroutine(SwordWave(damage, forwardVector, animLength * skill2AnticipationRatio));
    }

    // public override float GetCurrentCooldown(int skillNumber, float timeSinceLastExecuted, bool percentage = false)
    // {
    //     return base.GetCurrentCooldown(skillNumber, timeSinceLastExecuted, percentage);
    // }

    private IEnumerator SwordWave(float damage, Vector2 forwardVector, float delay)
    {
        yield return new WaitForSeconds(delay);
        AttackWithProjectile(ref ObjectManager.Instance.SwordWaves, damage, transform.position, forwardVector, knockAmplitude: 2.0f);
    }
}
