using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkills : PlayerSkills
{
    private ObjectPool arrows;

    public ArcherSkills(
        Transform transform,
        GameObject arrowPrefab
    ) : base(transform)
    {
        arrows = new ObjectPool(arrowPrefab, 20);
        this.stats = PlayerStats.Create(PlayerClass.Archer);
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
            case PlayerMovement.MovementState.Right:
                // Go up left
                movement.AddForceBySkill(new Vector2(-Mathf.Abs(forceVector.x), Mathf.Abs(forceVector.y)));
                break;
            case PlayerMovement.MovementState.Left:
                // Go up right
                movement.AddForceBySkill(new Vector2(Mathf.Abs(forceVector.x), Mathf.Abs(forceVector.y)));
                break;
        }

        // Then spawn arrow rains
        int arrowCount = configs.ArcherSkill2ArrowCount;
        float interval = configs.ArcherSkill2Interval;
        CoroutineUtility.Instance.CreateCoroutine(ArrowRain(arrowCount, damage, forwardVector, interval));
    }

    private IEnumerator ArrowRain(int count, float damage, Vector2 forwardVector, float interval)
    {
        for (int i = 0; i < count; ++i)
        {
            AttackWithProjectile(ref arrows, damage, transform.position, forwardVector, Random.Range(-20.0f, -50.0f));
            yield return new WaitForSeconds(interval);
        }
    }
}

