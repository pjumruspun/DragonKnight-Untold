using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Dragon Skills", menuName = "Roguelite/Skills/Dragon")]
public class DragonSkills : PlayerSkills
{
    public bool IsRushing => isRushing;

    // Skill 1 params
    private const float slashAnimLength = 0.3f;
    private const float skill1KnockUpAmplitude = 3.0f;
    private const float skill1KnockBackAmplitude = 2.0f;
    private const float skill1AnticipationRatio = 0.3f;
    private const float skill1LockMovementRatio = 0.6f;
    private const float skill1AnimStopRatio = 0.2f;
    private const float skill1ScreenShakeDuration = 0.3f;
    private const float skill1ScreenShakePower = 0.15f;

    // Skill 2 params
    private const float skill2Speed = 10.0f;
    private const float skill2FinalDamageFactor = 3.0f;
    private const float skill2KnockBackAmplitude = 10.0f;
    private const float skill2KnockUpAmplitude = 1.5f;
    private const float skill2FinalKnockUpAmplitude = 3.0f;
    private const float skill2ScreenShakeDuration = 0.3f;
    private const float skill2ScreenShakePower = 0.15f;
    private const float rushingDelay = 0.05f;
    private const float rushingDamageInterval = 0.20f;
    private bool isRushing = false;

    // Skill 3 params
    private const float skill3LockMovementRatio = 0.9f;
    private const float skill3SpeedMultiplier = 2.0f;
    private const float skill3KnockUpAmplitude = 3.5f;
    private const float skill3KnockBackAmplitude = 0.5f;
    private const float skill3TotalCooldownReduction = 0.6f;

    // Ultimate params
    private const float ultKnockUpAmplitude = 0.5f;
    private const float ultDelay = 0.05f;
    private const float ultInterval = 0.20f;
    private bool isUsingUltimate = false;

    private AttackHitbox dragonPrimaryHitbox;
    private AttackHitbox dragonVortexHitbox;
    private AttackHitbox fireBreathHitbox;
    private AttackHitbox dragonRushHitbox;
    private float[] dragonAttackDamage = new float[4];
    private float[] dragonAttackCooldown = new float[4];

    private float dragonSuperArmorAttack = 100.0f;
    private GameObject fireBreath;
    private GameObject clawSlash;
    private GameObject dragonDashEffect;
    private GameObject dragonHorizontalDashEffect;
    private Animator clawSlashAnim;
    private Animator dashAnim;
    private Coroutine rushCoroutine;
    private Coroutine fireBreathCoroutine;

    public void Initialize(
        Transform transform,
        AttackHitbox dragonPrimaryHitbox,
        AttackHitbox dragonVortexHitbox,
        AttackHitbox dragonRushHitbox,
        GameObject fireBreath,
        GameObject clawSlash,
        GameObject dragonDashEffect,
        GameObject dragonHorizontalDashEffect
    )
    {
        base.Initialize(transform);

        this.dragonPrimaryHitbox = dragonPrimaryHitbox;
        this.dragonVortexHitbox = dragonVortexHitbox;
        this.dragonRushHitbox = dragonRushHitbox;

        this.fireBreath = fireBreath;
        this.fireBreathHitbox = fireBreath.GetComponent<AttackHitbox>();
        this.fireBreath.SetActive(false);

        this.clawSlash = clawSlash;
        this.clawSlash.SetActive(false);

        this.dragonDashEffect = dragonDashEffect;
        this.dragonDashEffect.SetActive(false);

        this.dragonHorizontalDashEffect = dragonHorizontalDashEffect;
        this.dragonHorizontalDashEffect.SetActive(false);

        if (this.clawSlash.TryGetComponent<Animator>(out Animator animator))
        {
            this.clawSlashAnim = animator;
        }
        else
        {
            throw new System.NullReferenceException("Cannot get Animator component from clawSlash GameObject");
        }

        if (this.dragonDashEffect.TryGetComponent<Animator>(out Animator dashAnim))
        {
            this.dashAnim = dashAnim;
        }
        else
        {
            throw new System.NullReferenceException("Cannot get Animator component from dragonDashEffect GameObject");
        }

        // Initialize base skill damage and cooldown
        dragonAttackCooldown = SkillsRepository.Dragon.GetBaseSkillCooldowns.Cast<float>().ToArray();
        dragonAttackDamage = SkillsRepository.Dragon.GetBaseSkillDamage.Cast<float>().ToArray();
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
        isCastingSkill = true;

        // Primary attack = dragonAttackDamage[0]
        float damage = dragonAttackDamage[0];

        // Dragon Primary Attack
        // Night dragon is just a place holder for now
        float animLength = PlayerAnimation.Instance.GetAnimLength(0);

        UnlockCastingIn(animLength * 0.8f);

        // Lock movement
        movement.LockMovementBySkill(skill1LockMovementRatio * animLength, true, true);
        movement.LockJumpBySkill(skill1LockMovementRatio * animLength);

        // Actual attack damage applied
        float attackDelay = skill1AnticipationRatio * animLength;

        CoroutineUtility.ExecDelay(() =>
        {
            // Attack
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
                // Time stop
                float timeStopDuration = animLength * skill1AnimStopRatio;
                // Stop player's animator
                TimeStopper.StopAnimator(PlayerAnimation.Instance.GetAnimator, timeStopDuration);

                // Stop claw slash's animator
                TimeStopper.StopAnimator(clawSlashAnim, timeStopDuration);

                // Screen shaking
                ScreenShake.Instance.StartShaking(skill1ScreenShakeDuration, skill1ScreenShakePower);
            }
        }, attackDelay);

        // Claw slash effect
        // On
        CoroutineUtility.ExecDelay(() =>
        {
            this.clawSlash.SetActive(true);
        }, attackDelay / 2);

        // Off
        CoroutineUtility.ExecDelay(() =>
        {
            this.clawSlash.SetActive(false);
        }, attackDelay + slashAnimLength);
    }

    public override void Skill2()
    {
        currentCooldown[1] = dragonAttackCooldown[1];
        float damage = dragonAttackDamage[1];

        isRushing = true;
        isCastingSkill = true;

        rushCoroutine = CoroutineUtility.Instance.StartCoroutine(Rush(damage, rushingDelay, rushingDamageInterval));

        movement.LockJumpBySkill(true);
        movement.LockFlipBySkill(true);
        movement.LockMovementBySkill(true);
    }

    public void RushRelease()
    {
        if (!isRushing)
        {
            return;
        }

        float damage = dragonAttackDamage[1] * skill2FinalDamageFactor;

        // Extra final damage
        float totalDamageDealt = AttackWithHitbox(
            dragonRushHitbox,
            damage,
            superArmorDamage: 100,
            knockUpAmplitude: skill2FinalKnockUpAmplitude,
            hitEffect: HitEffect.Slash
        );

        // Deplete cooldown by 40% if hit something
        if (totalDamageDealt > 0.0f)
        {
            const float cooldownReductionRatio = 0.3f;
            currentCooldown[1] *= 1.0f - cooldownReductionRatio;
        }

        dragonHorizontalDashEffect.SetActive(false);

        if (rushCoroutine != null)
        {
            CoroutineUtility.Instance.StopCoroutine(rushCoroutine);
            rushCoroutine = null;
        }

        isRushing = false;
        isCastingSkill = false;

        movement.LockJumpBySkill(false);
        movement.LockFlipBySkill(false);
        movement.LockMovementBySkill(false);
    }

    public void UltimateRelease()
    {
        if (!isUsingUltimate)
        {
            return;
        }

        fireBreath.SetActive(false);
        CoroutineUtility.Instance.KillCoroutine(fireBreathCoroutine);

        isCastingSkill = false;
        isUsingUltimate = false;

        movement.LockJumpBySkill(false);
        movement.LockFlipBySkill(false);
        movement.LockMovementBySkill(false);
    }

    public override void Skill3()
    {
        currentCooldown[2] = dragonAttackCooldown[2];

        isCastingSkill = true;

        float damage = dragonAttackDamage[2];
        float animLength = PlayerAnimation.Instance.GetAnimLength(2);
        float lockMovementDuration = animLength * skill3LockMovementRatio;

        UnlockCastingIn(animLength * 1.0f);

        movement.LockMovementBySkill(lockMovementDuration, true, true);
        movement.LockJumpBySkill(lockMovementDuration);

        // Dash upward
        Vector2 moveVector = Vector2.up + movement.ForwardVector * 0.2f;
        movement.ForceMove(
            moveVector,
            PlayerStats.Instance.MovementSpeed * skill3SpeedMultiplier,
            animLength,
            groundOnly: false,
            forceMode: ForceMode2D.Impulse
        );

        // Dash effect
        dragonDashEffect.SetActive(true);
        CoroutineUtility.ExecDelay(() => dragonDashEffect.SetActive(false), animLength);

        // Attack
        int totalHits = 3;
        for (int i = 0; i < totalHits; ++i)
        {
            float superArmorDamage;
            if (i == 0)
            {
                superArmorDamage = 100;
            }
            else
            {
                superArmorDamage = 0;
            }

            CoroutineUtility.ExecDelay(() =>
            {
                // Attack
                // First time knock up
                float totalDamage = AttackWithHitbox(
                    dragonPrimaryHitbox,
                    damage / totalHits,
                    superArmorDamage,
                    knockUpAmplitude: skill3KnockUpAmplitude,
                    knockBackAmplitude: skill3KnockBackAmplitude,
                    hitEffect: HitEffect.Slash
                );

                // If hit, reduce cooldown
                if (totalDamage > 0.01f)
                {
                    currentCooldown[2] -= (dragonAttackCooldown[2] * skill3TotalCooldownReduction / totalHits);
                }

            }, i * (animLength / totalHits));
        }
    }

    public override void UltimateSkill()
    {
        Debug.Log("Dragon ult");
        currentCooldown[3] = dragonAttackCooldown[3];

        isCastingSkill = true;

        // Ult = dragonAttackDamage[3]
        float damage = dragonAttackDamage[3];

        fireBreathCoroutine = CoroutineUtility.Instance.CreateCoroutine(DelayedFireBreath(damage, ultDelay, ultInterval));
        movement.LockJumpBySkill(true);
        movement.LockFlipBySkill(true);
        movement.LockMovementBySkill(true);
    }

    private IEnumerator Rush(float damage, float delay, float interval)
    {
        yield return new WaitForSeconds(delay);
        dragonHorizontalDashEffect.SetActive(true);
        DragonGaugeStatic.dragonEnergy -= 5.0f;
        if (DragonGaugeStatic.dragonEnergy <= 0.0f)
        {
            EventPublisher.TriggerStopRush();
        }

        while (true)
        {
            isRushing = true;
            DragonGaugeStatic.dragonEnergy -= 1.0f;
            if (DragonGaugeStatic.dragonEnergy <= 0.0f)
            {
                EventPublisher.TriggerStopRush();
            }

            movement.MoveForwardBySkill(skill2Speed, interval, groundOnly: false, forceMode: ForceMode2D.Impulse);
            AttackWithHitbox(
                dragonRushHitbox,
                damage,
                knockUpAmplitude: skill2KnockUpAmplitude,
                knockBackAmplitude: skill2KnockBackAmplitude,
                hitEffect: HitEffect.Slash
            );

            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator DelayedFireBreath(float damage, float delay, float interval)
    {
        yield return new WaitForSeconds(delay);
        fireBreath.SetActive(true);

        DragonGaugeStatic.dragonEnergy -= 5.0f;
        if (DragonGaugeStatic.dragonEnergy <= 0.0f)
        {
            EventPublisher.TriggerStopRush();
        }

        while (true)
        {
            DragonGaugeStatic.dragonEnergy -= 1.0f;
            isUsingUltimate = true;
            yield return new WaitForSeconds(interval);
            AttackWithHitbox(fireBreathHitbox, damage, knockUpAmplitude: ultKnockUpAmplitude);
        }
    }
}