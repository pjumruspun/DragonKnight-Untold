using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkills : System.IDisposable
{
    public IReadOnlyList<float> SkillCooldown => stats.SkillCooldown;
    public PlayerStats PStats => stats;
    protected Transform transform;
    protected PlayerConfig playerConfig;
    protected AdditionalSkillConfigs configs;
    protected PlayerMovement movement;
    protected PlayerStats stats;


    public PlayerSkills(Transform transform)
    {
        // Cache
        playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        configs = playerConfig.AdditionalConfigs;
        movement = PlayerMovement.Instance;
        this.transform = transform;

        // Subscribe
        // EventPublisher.PlayerChangeClass += AdjustClassAttributes;
    }

    public void Dispose()
    {
        // Destructor, call on garbage collects
        // EventPublisher.PlayerChangeClass -= AdjustClassAttributes;
    }

    public abstract void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector);

    public abstract void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector);

    public virtual float GetCurrentCooldown(int skillNumber, float timeSinceLastExecuted, bool percentage = false)
    {
        if (skillNumber < 0 || skillNumber > 3)
        {
            throw new System.InvalidOperationException($"Error skillNumber {skillNumber} is not in between 0 and 3");
        }
        else
        {
            float cooldown = stats.SkillCooldown[skillNumber];
            float current = cooldown - timeSinceLastExecuted;
            if (percentage)
            {
                // Normalized with cooldown
                current = current / cooldown;
            }

            return current < 0.0f ? 0.0f : current;
        }
    }

    protected void AttackWithProjectile(ref ObjectPool objectPool, float damage, Vector3 currentPlayerPosition, Vector2 forwardVector, float rotationZ = 0.0f, float knockAmplitude = 0.0f)
    {
        GameObject spawnedObject = objectPool.SpawnObject(currentPlayerPosition, Quaternion.identity);

        if (forwardVector.x < -0.01f)
        {
            // If left, flip then rotate
            forwardVector = Quaternion.Euler(0.0f, 0.0f, -rotationZ) * forwardVector;
        }
        else if (forwardVector.x > 0.01f)
        {
            // Don't flip
            forwardVector = Quaternion.Euler(0.0f, 0.0f, rotationZ) * forwardVector;
        }

        if (spawnedObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.SetDirection(forwardVector);
            stats.CalculateDamage(damage, out float finalDamage, out bool crit);
            projectile.SetDamage(finalDamage, crit);
            projectile.SetKnockValue(knockAmplitude);
        }
        else
        {
            throw new System.InvalidOperationException();
        }
    }

    protected void AttackWithHitbox(PlayerAttackHitbox desiredHitbox, float attackDamage, float superArmorDamage = 0.0f, float knockAmplitude = 0.0f)
    {
        HashSet<Collider2D> collidersToRemove = new HashSet<Collider2D>();
        foreach (Collider2D enemyCollider in desiredHitbox.HitColliders)
        {
            bool objectIsActive = enemyCollider.gameObject.activeInHierarchy;
            bool isReallyAnEnemy = enemyCollider.gameObject.layer == Layers.enemyLayerIndex;
            if (objectIsActive && isReallyAnEnemy)
            {
                GameObject enemyObject = enemyCollider.gameObject;
                if (enemyObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // Damage the enemy here
                    stats.CalculateDamage(attackDamage, out float finalDamage, out bool crit);
                    enemy.TakeDamage(finalDamage, crit, superArmorDamage, knockAmplitude);
                }
                else
                {
                    // The enemy is dead, clear it from set
                    Debug.LogAssertion($"gameObject {enemyCollider.gameObject.name} does not have EnemyHealth attached to.");
                }
            }
            else
            {
                // Keep track of colliders to remove
                collidersToRemove.Add(enemyCollider);
            }
        }

        // Remove the unrelated colliders
        foreach (Collider2D unrelatedCollider in collidersToRemove)
        {
            desiredHitbox.HitColliders.Remove(unrelatedCollider);
        }
    }
}
