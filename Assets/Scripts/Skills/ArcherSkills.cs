using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archer Skills", menuName = "Roguelite/Skills/Archer")]
public class ArcherSkills : PlayerSkills
{
    private const float archerSkill2LockMovementTime = 0.5f;
    private Vector2 archerSkill2ForceVector = new Vector2(3.0f, 3.0f);

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
        int arrowCount = 3; // configs.ArcherSkill2ArrowCount;
        float interval = 0.3f; // configs.ArcherSkill2Interval;
        CoroutineUtility.Instance.CreateCoroutine(ArrowRain(arrowCount, damage, movement.ForwardVector, interval));
    }

    private IEnumerator ArrowRain(int count, float damage, Vector2 forwardVector, float interval)
    {
        // Spawns 5x3 arrows
        for (int i = 0; i < count; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                AttackWithProjectile(ref ObjectManager.Instance.Arrows, damage, transform.position, forwardVector, -7.5f - j * 15.0f);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}

