using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragonSkills : PlayerSkills
{
    private PlayerAttackHitbox dragonPrimaryHitbox;
    private PlayerAttackHitbox fireBreathHitbox;
    private float[] dragonAttackDamage = new float[4];
    private float[] dragonAttackCooldown = new float[4];
    // for testing
    private float dragonSuperArmorAttack = 50.0f;
    private GameObject fireBreath;
    private Coroutine fireBreathCoroutine;

    public DragonSkills(
        Transform transform,
        PlayerAttackHitbox dragonPrimaryHitbox,
        PlayerStats stats,
        GameObject fireBreath
    ) : base(transform)
    {
        this.dragonPrimaryHitbox = dragonPrimaryHitbox;
        this.stats = stats; // Player class stats
        this.fireBreath = fireBreath;
        this.fireBreathHitbox = fireBreath.GetComponent<PlayerAttackHitbox>();

        this.fireBreath.SetActive(false);

        DragonConfig config = ConfigContainer.Instance.GetPlayerConfig.NightDragonConfig;
        config.dragonAttackDamage.CopyTo(this.dragonAttackDamage, 0);
        config.dragonAttackCooldown.CopyTo(this.dragonAttackCooldown, 0);
    }

    public override float GetCurrentCooldown(int skillNumber, float timeSinceLastExecuted, bool percentage = false)
    {
        if (skillNumber < 0 || skillNumber > 3)
        {
            throw new System.InvalidOperationException($"Error skillNumber {skillNumber} is not in between 0 and 3");
        }
        else
        {
            float cooldown = dragonAttackCooldown[skillNumber];
            float current = cooldown - timeSinceLastExecuted;
            if (percentage)
            {
                // Normalized with cooldown
                current = current / cooldown;
            }

            return current < 0.0f ? 0.0f : current;
        }
    }

    public override void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        // Primary attack = dragonAttackDamage[0]
        float damage = dragonAttackDamage[0];

        // Dragon Primary Attack
        // Night dragon is just a place holder for now
        AttackWithHitbox(dragonPrimaryHitbox, playerConfig.NightDragonConfig.dragonAttackDamage[0], dragonSuperArmorAttack, knockAmplitude: 3.0f);
    }

    public override void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        // Skill 2 = dragonAttackDamage[1]
        float damage = dragonAttackDamage[1];

        // Dragon Skill 2
        fireBreathCoroutine = CoroutineUtility.Instance.CreateCoroutine(DelayedFireBreath(0.1f, 0.33f));
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

        Debug.Log("Fire breath stop");
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