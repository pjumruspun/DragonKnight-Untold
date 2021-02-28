using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkills : PlayerSkills
{
    private PlayerAttackHitbox swordPrimaryHitbox;
    private ObjectPool swordWaves;

    public SwordSkills(
        Transform transform,
        PlayerAttackHitbox swordPrimaryHitbox,
        GameObject swordWavePrefab
    ) : base(transform)
    {
        this.swordWaves = new ObjectPool(swordWavePrefab, 20);
        this.swordPrimaryHitbox = swordPrimaryHitbox;

        // Init sword stats
        this.stats = PlayerStats.Create(PlayerClass.Sword);
    }

    public override void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        base.Skill1(currentPlayerPosition, forwardVector);

        // Primary attack = skillDamage[0]
        float damage = stats.BaseSkillDamage[0];

        // Lock player's movement and flip
        // Lock equals to animation clip length
        float animLength = PlayerAnimation.Instance.GetAnimLength(0);
        movement.LockMovementBySkill(animLength, true, true);

        // Then attack
        AttackWithHitbox(swordPrimaryHitbox, damage, knockAmplitude: 1.5f);
    }

    public override void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        base.Skill2(currentPlayerPosition, forwardVector);

        // Skill 2 = skillDamage[1]
        float damage = stats.BaseSkillDamage[1];

        // Sword wave
        // Lock player's movement
        movement.LockMovementBySkill(configs.SwordSkill2LockMovementTime, true, true);

        // Spawn sword wave with delay
        CoroutineUtility.Instance.CreateCoroutine(SwordWave(damage, forwardVector, configs.SwordSkill2DelayTime));
    }

    // public override float GetCurrentCooldown(int skillNumber, float timeSinceLastExecuted, bool percentage = false)
    // {
    //     return base.GetCurrentCooldown(skillNumber, timeSinceLastExecuted, percentage);
    // }

    private IEnumerator SwordWave(float damage, Vector2 forwardVector, float delay)
    {
        yield return new WaitForSeconds(delay);
        AttackWithProjectile(ref swordWaves, damage, transform.position, forwardVector, knockAmplitude: 2.0f);
    }
}
