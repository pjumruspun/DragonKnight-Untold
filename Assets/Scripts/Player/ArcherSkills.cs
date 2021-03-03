using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkills : PlayerSkills
{
    private ObjectPool arrows;

    public ArcherSkills(
        Transform transform,
        ref PlayerStats stats,
        GameObject arrowPrefab
    ) : base(transform, ref stats)
    {
        arrows = new ObjectPool(arrowPrefab, 20);
    }

    public override void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        base.Skill1(currentPlayerPosition, forwardVector);

        // Primary attack = skillDamage[0]
        float damage = stats.BaseSkillDamage[0];
        // Spawn arrow
        AttackWithProjectile(ref arrows, damage, currentPlayerPosition, forwardVector);
    }

    public override void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        base.Skill2(currentPlayerPosition, forwardVector);

        // Skill 2 = skillDamage[1]
        float damage = stats.BaseSkillDamage[1];

        // Arrow rain
        // Lock player's movement and flip
        movement.LockMovementBySkill(configs.ArcherSkill2LockMovementTime, false, true);

        // Add force by skills first
        Vector2 forceVector = configs.ArcherSkill2ForceVector;
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
        int arrowCount = 3; // configs.ArcherSkill2ArrowCount;
        float interval = 0.3f; // configs.ArcherSkill2Interval;
        CoroutineUtility.Instance.CreateCoroutine(ArrowRain(arrowCount, damage, forwardVector, interval));
    }

    private IEnumerator ArrowRain(int count, float damage, Vector2 forwardVector, float interval)
    {
        // Spawns 5x3 arrows
        for (int i = 0; i < count; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                AttackWithProjectile(ref arrows, damage, transform.position, forwardVector, -7.5f - j * 15.0f);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}

