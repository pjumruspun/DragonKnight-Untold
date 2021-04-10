using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archer Skills", menuName = "Roguelite/Skills/Archer")]
public class ArcherSkills : PlayerSkills
{
    private DashEffectSize chargedShotEffect;

    // Spread shot vars
    private bool hasFiredSpreadShot = false;

    // Auto attack params
    private float timeSinceLastShot = 0.0f;
    private const float flipLockParam = 1.1f;
    private const float skill1LockMovementRatio = 0.4f;

    // Arrow Rain params
    private const float arrowRainLockMovementTime = 0.5f;
    private Vector2 arrowRainForceVector = new Vector2(3.0f, 3.0f);

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

    public void Initialize(Transform transform, DashEffectSize chargedShotEffect)
    {
        base.Initialize(transform);
        shouldReleaseChargeShot = false;

        this.chargedShotEffect = chargedShotEffect;
        this.chargedShotEffect.gameObject.SetActive(false);

        EventPublisher.PlayerLand += ResetSpreadShot;
    }

    public override void Skill1()
    {
        if (movement.IsGrounded())
        {
            base.Skill1();
            NormalShot();
        }
        else
        {
            if (!hasFiredSpreadShot)
            {
                SpreadShot();
                hasFiredSpreadShot = true;
            }
        }
    }

    // Charged Shot
    public override void Skill2()
    {
        base.Skill2();

        shouldReleaseChargeShot = false;
        // Charge shot
        chargeShot = CoroutineUtility.Instance.CreateCoroutine(IncreaseChargeLevel());

        // Show charging effect
        chargedShotEffect.gameObject.SetActive(true);
        chargedShotEffect.SetSize(0, maxChargeLevel);

        // Reset Trigger
        PlayerAnimation.Instance.ResetChargeShotTrigger();

        // Lock movement
        movement.LockJumpBySkill(true);
        movement.LockFlipBySkill(true);
        movement.LockMovementBySkill(true, false);
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
        float damage = PlayerStats.Instance.BaseSkillDamage[0] * (1.5f + 0.75f * currentChargeLevel);
        float knockBackAmplitude = chargedShotKnockBackAmplitude * (0.5f + 0.1f * currentChargeLevel / maxChargeLevel);

        Projectile chargedShot = AttackWithProjectile(
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

        // Set projectile visual size
        DashEffectSize chargedShotArrowEffect = chargedShot.GetComponentInChildren<DashEffectSize>();
        if (chargedShotArrowEffect == null)
        {
            throw new System.NullReferenceException("Cannot find DashEffectSize under Projectile chargedShot's children");
        }

        chargedShotArrowEffect.SetSize(currentChargeLevel, maxChargeLevel);

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

            // Also mark stop casting skill
            isCastingSkill = false;
        }, animLength);

        // Stop animation
        PlayerAnimation.Instance.StopChargingShot();

        // Disable charge effect
        chargedShotEffect.gameObject.SetActive(false);

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

    private void NormalShot()
    {
        // Primary attack = skillDamage[0]
        float damage = PlayerStats.Instance.BaseSkillDamage[0];

        // Get cooldown
        float cooldown = PlayerStats.Instance.SkillCooldown[0];

        // Lock movement
        movement.LockMovementBySkill(cooldown * skill1LockMovementRatio, true, false, false);

        // Lock flip for a while
        movement.LockFlipBySkill(true);
        timeSinceLastShot = Time.time;

        // Lock skill casting by cooldown time
        UnlockCastingIn(cooldown);

        float gap = cooldown * flipLockParam;

        CoroutineUtility.ExecDelay(() =>
        {
            if (Time.time > timeSinceLastShot + gap)
            {
                movement.LockFlipBySkill(false);
            }
        }, gap);

        // Spawn arrow
        AttackWithProjectile(
            ref ObjectManager.Instance.Arrows,
            damage,
            transform.position,
            movement.ForwardVector,
            hitEffect: HitEffect.Slash,
            shouldFlinch: false
        );
    }

    private void SpreadShot()
    {
        // This is just like arrow rain but only fire one time
        float animLength = PlayerAnimation.Instance.GetAnimLength("Archer_ShootDown");
        movement.LockMovementBySkill(animLength * 0.5f, false, true);

        // Damage
        float damage = PlayerStats.Instance.BaseSkillDamage[0];

        // Spawn spread shot
        CoroutineUtility.Instance.CreateCoroutine(ArrowRain(damage, 0.3f, 1, 3));

        // Unlock casting skill
        UnlockCastingIn(animLength);
    }

    private void ResetSpreadShot()
    {
        hasFiredSpreadShot = false;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerLand -= ResetSpreadShot;
    }

    private void ArrowRain()
    {
        // Skill 3 = skillDamage[2]
        float damage = PlayerStats.Instance.BaseSkillDamage[2];

        // Arrow rain
        // Lock player's movement and flip
        movement.LockMovementBySkill(arrowRainLockMovementTime, false, true);

        // Add force by skills first
        Vector2 forceVector = arrowRainForceVector;
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
        CoroutineUtility.Instance.CreateCoroutine(ArrowRain(damage, 0.3f, 5, 3));
    }

    private IEnumerator IncreaseChargeLevel()
    {
        // Set charging effect to true
        while (true)
        {
            yield return new WaitForSeconds(chargeTimePerLevel);
            ++currentChargeLevel;

            // Increase size of charge effect
            chargedShotEffect.SetSize(currentChargeLevel, maxChargeLevel);

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

    private IEnumerator ArrowRain(float damage, float interval, int totalFireCount, int arrowsEachShot)
    {
        // Spawns 5x3 arrows
        for (int i = 0; i < totalFireCount; ++i)
        {
            for (int j = 0; j < arrowsEachShot; ++j)
            {
                AttackWithProjectile(ref ObjectManager.Instance.Arrows, damage, transform.position, movement.ForwardVector, -7.5f - j * 15.0f);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}

