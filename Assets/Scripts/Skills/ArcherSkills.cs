using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archer Skills", menuName = "Roguelite/Skills/Archer")]
public class ArcherSkills : PlayerSkills
{
    private const float archerSkill2LockMovementTime = 0.5f;
    private Vector2 archerSkill2ForceVector = new Vector2(3.0f, 3.0f);

    // Arrow Rain params
    private const int arrowsEachShot = 5;
    private const int totalFireRounds = 3;
    private const float fireArrowsInterval = 0.3f;

    // Charge shot variables
    private Coroutine chargeShot;
    private int currentChargeLevel = 0;

    // Charge shot params
    private const int maxChargeLevel = 5;
    private const float chargeTimePerLevel = 0.5f;
    private const float skill2PlayerKnockBackAmplitude = 10.0f;
    private const float chargedShotKnockBackAmplitude = 5.0f;
    private const float chargedShotKnockUpAmplitude = 4.0f;
    private bool shouldReleaseChargeShot;
    private const float chargedShotHitInterval = 0.15f;

    public override void Initialize(Transform transform)
    {
        base.Initialize(transform);
        shouldReleaseChargeShot = false;
    }

    public override void Skill1()
    {
        base.Skill1();

        // Primary attack = skillDamage[0]
        float damage = PlayerStats.Instance.BaseSkillDamage[0];
        // Spawn arrow
        AttackWithProjectile(ref ObjectManager.Instance.Arrows, damage, transform.position, movement.ForwardVector);
    }

    public override void Skill2()
    {
        base.Skill2();

        if (movement.IsGrounded())
        {
            Skill2GroundVariant();
        }
        else
        {
            Skill2AirVariant();
        }
    }

    public void NotifySkill2ToRelease()
    {
        if (currentChargeLevel >= 1)
        {
            EventPublisher.TriggerStopChargeShot();
        }
        else
        {
            shouldReleaseChargeShot = true;
        }
    }

    public void Skill2Release()
    {
        // Spawn arrow
        float damage = PlayerStats.Instance.BaseSkillDamage[0] * currentChargeLevel;
        float knockBackAmplitude = chargedShotKnockBackAmplitude * currentChargeLevel / maxChargeLevel;

        AttackWithProjectile(
            ref ObjectManager.Instance.ChargedArrows,
            damage,
            transform.position,
            movement.ForwardVector,
            knockBackAmplitude: knockBackAmplitude,
            knockUpAmplitude: chargedShotKnockUpAmplitude,
            hitEffect: HitEffect.Slash,
            shouldHitContinuously: true,
            hitInterval: chargedShotHitInterval
        );

        // Knock back
        float animLength = PlayerAnimation.Instance.GetAnimLength("Archer_Charge_End");
        Vector2 moveVector = -movement.ForwardVector;
        movement.ForceMove(
            moveVector,
            skill2PlayerKnockBackAmplitude,
            animLength,
            groundOnly: false,
            forceMode: ForceMode2D.Impulse
        );

        // Release movement lock after done knocking back player
        CoroutineUtility.ExecDelay(() =>
        {
            movement.LockJumpBySkill(false);
            movement.LockFlipBySkill(false);
            movement.LockMovementBySkill(false);
        }, animLength);

        // Stop animation
        PlayerAnimation.Instance.StopChargingShot();

        // Kill coroutine
        if (chargeShot != null)
        {
            CoroutineUtility.Instance.KillCoroutine(chargeShot);
        }

        // Reset charge level
        currentChargeLevel = 0;

        // Reset variable
        shouldReleaseChargeShot = false;
    }

    private void Skill2AirVariant()
    {
        // Skill 2 = skillDamage[1]
        float damage = PlayerStats.Instance.BaseSkillDamage[1];

        // Arrow rain
        // Lock player's movement and flip
        movement.LockMovementBySkill(archerSkill2LockMovementTime, false, true);

        // Add force by skills first
        Vector2 forceVector = archerSkill2ForceVector;
        switch (movement.TurnDirection)
        {
            case MovementState.Right:
                // Go up left
                movement.AddForceBySkill(new Vector2(-Mathf.Abs(forceVector.x), Mathf.Abs(forceVector.y)));
                break;
            case MovementState.Left:
                // Go up right
                movement.AddForceBySkill(new Vector2(Mathf.Abs(forceVector.x), Mathf.Abs(forceVector.y)));
                break;
        }

        // Then spawn arrow rains
        CoroutineUtility.Instance.CreateCoroutine(ArrowRain(damage, movement.ForwardVector, fireArrowsInterval));
    }

    private void Skill2GroundVariant()
    {
        shouldReleaseChargeShot = false;
        // Charge shot
        chargeShot = CoroutineUtility.Instance.CreateCoroutine(IncreaseChargeLevel());

        // Reset Trigger
        PlayerAnimation.Instance.ResetChargeShotTrigger();

        // Lock movement
        movement.LockJumpBySkill(true);
        movement.LockFlipBySkill(true);
        movement.LockMovementBySkill(true);
    }

    private IEnumerator IncreaseChargeLevel()
    {
        // Set charging effect to true
        while (true)
        {
            yield return new WaitForSeconds(chargeTimePerLevel);
            ++currentChargeLevel;
            if (currentChargeLevel == maxChargeLevel)
            {
                EventPublisher.TriggerStopChargeShot();
            }

            if (shouldReleaseChargeShot && currentChargeLevel == 1)
            {
                EventPublisher.TriggerStopChargeShot();
            }
        }
    }

    private IEnumerator ArrowRain(float damage, Vector2 forwardVector, float interval)
    {
        // Spawns 5x3 arrows
        for (int i = 0; i < totalFireRounds; ++i)
        {
            for (int j = 0; j < arrowsEachShot; ++j)
            {
                AttackWithProjectile(ref ObjectManager.Instance.Arrows, damage, transform.position, forwardVector, -7.5f - j * 15.0f);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}

