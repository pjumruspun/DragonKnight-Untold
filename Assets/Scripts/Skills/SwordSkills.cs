using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sword Skills", menuName = "Roguelite/Skills/Sword")]
public class SwordSkills : PlayerSkills
{
    public int CurrentCombo => currentCombo;
    // Skill canceling params
    private readonly float[] skillCastingRatio = new float[4] { 0.7f, 0.8f, 0.8f, 0.9f };

    // Skill 1 params
    private readonly float[] skill1PushSpeed = new float[3] { 50.0f, 50.0f, 150.0f };
    private readonly float[] skill1KnockBackAmplitudes = new float[3] { 2.0f, 2.0f, 4.0f };
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
    private const float skill3SlashKnockUpAmplitude = 2.0f;
    private const float skill3CooldownResetRatio = 0.6f;
    private const float skill3DashKnockUpAmplitude = 2.5f;
    private const float skill3DashKnockBackAmplitude = 4.5f;
    private const float dashDamageRatio = 0.5f;
    private const float slashDamageRatio = 0.5f;

    // Ultimate skill variables
    private bool hasCounterAttacked = false;
    private bool counterSuccessful = false;
    private InvulBuff currentInvulBuff = null;

    // Parry skill params
    private const float parryAttackLockMovementRatio = 0.7f;
    private const float parryInvulDuration = 1.0f;

    // Counter params
    private readonly float[] counterAttackTimeRatio = new float[3] { 0.1f, 0.3f, 0.7f };
    private readonly float[] counterAttackDamageRatio = new float[3] { 0.25f, 0.25f, 0.5f };
    private readonly float[] counterKnockUpAmplitude = new float[3] { 1.5f, 2.0f, 2.0f };
    private const float counterTimeStopRatio = 0.2f;
    private const float counterScreenShakeDuration = 0.3f;
    private const float counterScreenShakePower = 0.15f;
    private const float counterSuperArmorAttack = 100.0f;

    // Hitboxes
    private AttackHitbox swordPrimaryHitbox;

    // Effects
    private GameObject dashEffect;

    // Combo stuff
    private const float resetComboRatio = 1.5f; // 1.5 times of attack anim length
    private int currentCombo = 0;
    private float lastAttackTime = 0.0f;

    public void Initialize(Transform transform, AttackHitbox swordPrimaryHitbox, GameObject dashEffect)
    {
        base.Initialize(transform);

        this.swordPrimaryHitbox = swordPrimaryHitbox;

        this.dashEffect = dashEffect;
        this.dashEffect.SetActive(false);
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
        movement.LockMovementBySkill(animLength * skill1LockMovementRatio, true, true);
        movement.LockJumpBySkill(animLength * skill1LockMovementRatio);

        movement.MoveForwardBySkill(skill1PushSpeed[currentCombo], animLength * skill1PushDurationRatio, groundOnly: false);

        // Then attack
        float anticipationPeriod = animLength * skill1AnticipationRatio;
        CoroutineUtility.ExecDelay(() =>
        {
            SoundManager.Play(SFXName.Sword1);

            float damageDealt = AttackWithHitbox(
                swordPrimaryHitbox,
                damage,
                knockUpAmplitude: skill1KnockUpAmplitude,
                knockBackAmplitude: skill1KnockBackAmplitudes[currentCombo],
                hitEffect: HitEffect.Slash,
                sfx: SFXName.SwordHit
            );

        }, anticipationPeriod);

        // Unlock casting skills
        UnlockCastingIn(animLength * skillCastingRatio[0]);
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

        // Unlock casting skill
        UnlockCastingIn(animLength * skillCastingRatio[1]);
    }

    // Dash dealing damage -> Dash slash
    public override void Skill3()
    {
        base.Skill3();

        // Lock animation
        float animLength = PlayerAnimation.Instance.GetAnimLength(2);
        float lockMovementDuration = animLength * (1.0f + skill3DashAnticipationRatio);
        movement.LockMovementBySkill(lockMovementDuration, true, true);
        movement.LockJumpBySkill(lockMovementDuration);

        // Dash ahead
        CoroutineUtility.ExecDelay(() =>
        {
            movement.MoveForwardBySkill(
                PlayerStats.Instance.MovementSpeed * skill3SpeedMultiplier,
                animLength,
                groundOnly: false,
                forceMode: ForceMode2D.Impulse
            );

            // Skill 3 = skillDamage[2]
            float damage = PlayerStats.Instance.BaseSkillDamage[2] * dashDamageRatio;

            // Dash effect on
            this.dashEffect.SetActive(true);
            // Dash effect off
            CoroutineUtility.ExecDelay(() => this.dashEffect.SetActive(false), animLength);

            // Attack
            int totalHits = 3;
            for (int i = 0; i < totalHits; ++i)
            {
                CoroutineUtility.ExecDelay(() =>
                {
                    // Dash Attack
                    // First time knock up
                    float totalDamage = AttackWithHitbox(
                            swordPrimaryHitbox,
                            damage / totalHits,
                            knockUpAmplitude: skill3DashKnockUpAmplitude,
                            knockBackAmplitude: skill3DashKnockBackAmplitude,
                            hitEffect: HitEffect.Slash
                    );
                }, i * (animLength / totalHits));
            }

            // DashSlash();
        }, skill3DashAnticipationRatio * animLength);

        // Unlock casting
        UnlockCastingIn(animLength * skillCastingRatio[2]);
    }

    private void DashSlash()
    {
        PlayerAnimation.Instance.PlayDashAttackAnimation();

        // Lock animation
        float animLength = PlayerAnimation.Instance.GetAnimLength(1);
        movement.LockMovementBySkill(animLength, true, true);
        movement.LockJumpBySkill(animLength);

        // Skill 3 = skillDamage[2]
        float damage = PlayerStats.Instance.BaseSkillDamage[2] * slashDamageRatio;

        // Attack
        float anticipationPeriod = animLength * skill3AttackAnticipationRatio;
        CoroutineUtility.ExecDelay(() =>
        {
            float damageDealt = AttackWithHitbox(swordPrimaryHitbox, damage, knockUpAmplitude: skill3SlashKnockUpAmplitude, hitEffect: HitEffect.Slash);
            if (damageDealt > 0.0f) // If manage to hit something
            {
                // Reduce cooldown
                currentCooldown[2] = PlayerStats.Instance.SkillCooldown[2] * (1.0f - skill3CooldownResetRatio);
            }
        }, anticipationPeriod);
    }

    public override void UltimateSkill()
    {
        base.UltimateSkill();

        // Ensure that animation is playing correctly
        PlayerAnimation.Instance.ResetParryTrigger();

        movement.LockMovementBySkill(parryInvulDuration, true, true);
        movement.LockJumpBySkill(parryInvulDuration);

        // Give blocking buff
        InvulBuff invulBuff = new InvulBuff(parryInvulDuration);

        // When the buff ends, try to counter attack
        invulBuff.OnUpdate += TryCounterAttack;
        invulBuff.Callback += ResetCounterAttackVar; // Unlock casting in here
        currentInvulBuff = invulBuff;
        BuffManager.AddBuff(invulBuff);
    }

    private void TryCounterAttack()
    {
        if (!hasCounterAttacked && PlayerHealth.Instance.HasSuccessfullyBlocked)
        {
            CounterAttack();
        }
    }

    private void CounterAttack()
    {
        // Lock casting again
        isCastingSkill = true;

        // Notify that player has already counter attacked
        hasCounterAttacked = true;
        counterSuccessful = true;
        Debug.Log("counter successful");

        // Play animation
        PlayerAnimation.Instance.PlayCounterAnimation();

        PlayerHealth.Instance.SetHasBlocked(false);

        float animLength = PlayerAnimation.Instance.GetAnimLength("Sword_Counter");

        // Three fold attack
        ThreeFoldAttack(animLength);

        // Delayed unlock movement depends on anim length
        CoroutineUtility.ExecDelay(() =>
        {
            movement.ForceUnlockJump();
            movement.ForceUnlockMovement();
        }, animLength * 1.0f);

        // Upon successful counter, extends invul window by anim length
        currentInvulBuff.SetDuration(currentInvulBuff.DurationLeft + animLength * 1.0f);

        // Unlock casting
        UnlockCastingIn(animLength * skillCastingRatio[3]);
        CoroutineUtility.ExecDelay(() => counterSuccessful = false, animLength * skillCastingRatio[3]);
    }

    private void ThreeFoldAttack(float animLength)
    {
        float damage = PlayerStats.Instance.BaseSkillDamage[3];

        // First
        CoroutineUtility.ExecDelay(() =>
            AttackWithHitbox(
                swordPrimaryHitbox,
                damage * counterAttackDamageRatio[0],
                knockUpAmplitude: counterKnockUpAmplitude[0],
                hitEffect: HitEffect.Slash
            ), counterAttackTimeRatio[0] * animLength
        );

        // Second
        CoroutineUtility.ExecDelay(() =>
            AttackWithHitbox(
                swordPrimaryHitbox,
                damage * counterAttackDamageRatio[1],
                knockUpAmplitude: counterKnockUpAmplitude[1],
                hitEffect: HitEffect.Slash
            ), counterAttackTimeRatio[1] * animLength
        );

        // Third, also screen shake and destroy enemy's super armor
        CoroutineUtility.ExecDelay(() =>
        {
            float totalDamage = AttackWithHitbox(
                swordPrimaryHitbox,
                damage * counterAttackDamageRatio[2],
                knockUpAmplitude: counterKnockUpAmplitude[2],
                hitEffect: HitEffect.Slash,
                superArmorDamage: counterSuperArmorAttack
            );

            if (totalDamage > 0.0f)
            {
                // Time stop
                float timeStopDuration = animLength * counterTimeStopRatio;
                // Stop player's animator
                TimeStopper.StopAnimator(PlayerAnimation.Instance.GetAnimator, timeStopDuration);

                // Screen shaking
                ScreenShake.Instance.StartShaking(counterScreenShakeDuration, counterScreenShakePower);
            }
        }, counterAttackTimeRatio[2] * animLength);
    }

    private void ResetCounterAttackVar()
    {
        if (!counterSuccessful)
        {
            isCastingSkill = false;
        }

        Debug.Log("Uncast");

        hasCounterAttacked = false;
        PlayerHealth.Instance.SetHasBlocked(false);
        PlayerAnimation.Instance.StopParrying();
        currentInvulBuff = null;
    }

    private IEnumerator SwordWave(float damage, Vector2 forwardVector, float delay)
    {
        yield return new WaitForSeconds(delay);
        AttackWithProjectile(
            ref ObjectManager.Instance.SwordWaves,
            damage, transform.position,
            forwardVector, knockUpAmplitude:
            2.0f,
            hitEffect: HitEffect.Slash
        );
    }
}
