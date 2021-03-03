using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkills
{
    // Cache
    protected Transform transform;
    protected PlayerConfig playerConfig;
    protected AdditionalSkillConfigs configs;
    protected PlayerMovement movement;
    protected float[] currentCooldown;
    protected PlayerStats stats;

    /// <summary>
    /// Returns current cooldown of every skill.
    /// currentCooldown[i] has the range of [0, stats.SkillCooldown[i]].
    /// </summary>
    /// <returns></returns>
    public virtual float[] GetCurrentCooldown()
    {
        // Need to be a method because it needs to be overriden
        return currentCooldown;
    }

    /// <summary>
    /// Returns current cooldown percentage of given skillNumber.
    /// </summary>
    /// <param name="skillNumber"></param>
    /// <returns></returns>
    public virtual float CurrentCooldownPercentage(int skillNumber)
    {
        return currentCooldown[skillNumber] / stats.SkillCooldown[skillNumber];
    }

    public PlayerSkills(Transform transform, ref PlayerStats stats)
    {
        // Cache
        playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        configs = playerConfig.AdditionalConfigs;
        movement = PlayerMovement.Instance;
        this.transform = transform;
        this.stats = stats;
        currentCooldown = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
    }

    /// <summary>
    /// Call this in void Update() as it updates the cooldown of each skill
    /// </summary>
    public void ProcessSkillCooldown()
    {
        // Debug.Log($"{currentCooldown[0]} {currentCooldown[1]} {currentCooldown[2]} {currentCooldown[3]}");
        for (int i = 0; i < 4; ++i)
        {
            if (currentCooldown[i] > 0.0f)
            {
                currentCooldown[i] -= Time.deltaTime;
            }
            else
            {
                currentCooldown[i] = 0.0f;
            }
        }
    }

    /// <summary>
    /// Execute skill 1 (auto attack)
    /// </summary>
    /// <param name="currentPlayerPosition"></param>
    /// <param name="forwardVector"></param>
    public virtual void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        currentCooldown[0] = stats.SkillCooldown[0];
    }

    /// <summary>
    /// Execute skill 2
    /// </summary>
    /// <param name="currentPlayerPosition"></param>
    /// <param name="forwardVector"></param>
    public virtual void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector)
    {
        currentCooldown[1] = stats.SkillCooldown[1];
    }

    /// <summary>
    /// Spawn a projectile which is created by "objectPool". Make sure that the projectile script has
    /// target marked as "Enemy".
    /// </summary>
    /// <param name="objectPool">an ObjectPool that has projectile prefab in it.</param>
    /// <param name="damage">How much damage does this projectile do.</param>
    /// <param name="spawnPosition">Position that the projectile should spawn at.</param>
    /// <param name="forwardVector">Player's forward vector ((-1,0) or (1,0))</param>
    /// <param name="rotationZ">Angle compared to forward vector.</param>
    /// <param name="knockAmplitude">If the enemy is knocked, how much force in y-axis will the enemy gets hit by when this projectile hits.</param>
    protected void AttackWithProjectile(
        ref ObjectPool objectPool,
        float damage, Vector3 spawnPosition,
        Vector2 forwardVector,
        float rotationZ = 0.0f,
        float knockAmplitude = 0.0f
    )
    {
        GameObject spawnedObject = objectPool.SpawnObject(spawnPosition, Quaternion.identity);

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

    /// <summary>
    /// Melee attack, attack all enemies within the sent hitbox.
    /// </summary>
    /// <param name="desiredHitbox">Hitbox that enemies will be hit if they are in.</param>
    /// <param name="attackDamage">How much damage does this attack do.</param>
    /// <param name="superArmorDamage">If this does any super armor damage.</param>
    /// <param name="knockAmplitude">If the enemy is knocked, how much force in y-axis will the enemy gets hit by when this attack hits.</param>
    protected void AttackWithHitbox(
        AttackHitbox desiredHitbox,
        float attackDamage,
        float superArmorDamage = 0.0f,
        float knockAmplitude = 0.0f
    )
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
