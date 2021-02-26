using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragonSkills : PlayerSkills
{
    private PlayerAttackHitbox dragonPrimaryHitbox;
    private float[] dragonAttackDamage = new float[4];
    private float[] dragonAttackCooldown = new float[4];
    // for testing
    private float dragonSuperArmorAttack = 100.0f;

    public DragonSkills(
        Transform transform,
        PlayerAttackHitbox dragonPrimaryHitbox,
        PlayerStats stats
    ) : base(transform)
    {
        this.dragonPrimaryHitbox = dragonPrimaryHitbox;
        this.stats = stats; // Player class stats

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
        AttackWithHitbox(dragonPrimaryHitbox, playerConfig.NightDragonConfig.dragonAttackDamage[0], dragonSuperArmorAttack);
    }

    public override void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        // Skill 2 = dragonAttackDamage[1]
        float damage = dragonAttackDamage[1];

        // Dragon Skill 2
        Debug.Log("Still not implemented");
    }
}