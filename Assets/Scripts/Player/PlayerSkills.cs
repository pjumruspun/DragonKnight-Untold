using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : System.IDisposable
{
    public float[] SkillDamage => skillDamage; // For debugging purpose
    public float[] SkillCooldown => skillCooldown;
    public float[] DragonAttackDamage => dragonAttackDamage;
    public float[] DragonAttackCooldown => dragonAttackCooldown;
    public PlayerClass Class => playerClass;
    private Transform transform;
    private GameObject arrowPrefab;
    private PlayerConfig playerConfig;
    private AdditionalSkillConfigs configs;
    private PlayerAttackHitbox dragonPrimaryHitbox;
    private PlayerAttackHitbox swordPrimaryHitbox;
    private float[] dragonAttackDamage;
    private float[] dragonAttackCooldown;
    private float[] skillDamage;
    private float[] skillCooldown;
    private PlayerClass playerClass;
    private ObjectPool arrows;
    private ObjectPool swordWaves;
    private PlayerMovement movement;
    private PlayerStats stats;

    public PlayerSkills(Transform transform, PlayerAttackHitbox dragonPrimaryHitbox, PlayerAttackHitbox swordPrimaryHitbox, GameObject arrowPrefab, GameObject swordWavePrefab)
    {
        // Cache
        playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        configs = playerConfig.AdditionalConfigs;
        movement = PlayerMovement.Instance;

        // Init player stats
        stats = new PlayerStats();

        this.transform = transform;
        this.dragonPrimaryHitbox = dragonPrimaryHitbox;
        this.swordPrimaryHitbox = swordPrimaryHitbox;
        this.arrowPrefab = arrowPrefab;

        // Object pooling
        arrows = new ObjectPool(arrowPrefab, 20);
        swordWaves = new ObjectPool(swordWavePrefab, 20);

        // Subscribe
        EventPublisher.PlayerChangeClass += AdjustClassAttributes;
    }

    public void Dispose()
    {
        // Destructor, call on garbage collects
        EventPublisher.PlayerChangeClass -= AdjustClassAttributes;
    }

    public void Skill1(Vector3 currentPlayerPosition, Vector2 forwardVector) // Player position for arrow spawning position
    {
        // Primary attack = skillDamage[0]
        float damage = skillDamage[0];

        if (PlayerAbilities.Instance.IsDragonForm)
        {
            // Dragon Primary Attack
            // Night dragon is just a place holder for now
            AttackWithHitbox(dragonPrimaryHitbox, playerConfig.NightDragonConfig.dragonAttackDamage[0]);
        }
        else
        {
            // Player Primary Attack
            switch (playerClass)
            {
                case PlayerClass.Sword:
                    AttackWithHitbox(swordPrimaryHitbox, damage);
                    break;
                case PlayerClass.Archer:
                    // Spawn arrow
                    AttackWithProjectile(ref arrows, damage, currentPlayerPosition, forwardVector);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }

    public void Skill2(Vector3 currentPlayerPosition, Vector2 forwardVector) // Player position for arrow spawning position
    {
        // Skill 2 = skillDamage[1]
        float damage = skillDamage[1];

        if (PlayerAbilities.Instance.IsDragonForm)
        {
            // Dragon Skill 2
            Debug.Log("Still not implemented");
        }
        else
        {
            // Player Primary Attack
            switch (playerClass)
            {
                case PlayerClass.Sword:
                    // Sword wave
                    // Lock player's movement
                    movement.LockMovementBySkill(configs.SwordSkill2LockMovementTime, true);

                    // Lock player's flip
                    movement.LockFlipBySkill(configs.SwordSkill2LockMovementTime);

                    // Spawn sword wave with delay
                    CoroutineUtility.Instance.CreateCoroutine(SwordWave(damage, forwardVector, configs.SwordSkill2DelayTime));
                    break;
                case PlayerClass.Archer:
                    // Arrow rain
                    // Lock player's movement
                    movement.LockMovementBySkill(configs.ArcherSkill2LockMovementTime);

                    // Lock player's flip
                    movement.LockFlipBySkill(configs.ArcherSkill2LockMovementTime);

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
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }

    public float GetCurrentCooldown(int skillNumber, float timeSinceLastExecuted, bool percentage = false)
    {
        if (skillNumber < 0 || skillNumber > 3)
        {
            throw new System.InvalidOperationException($"Error skillNumber {skillNumber} is not in between 0 and 3");
        }
        else
        {
            float cooldown = PlayerAbilities.Instance.IsDragonForm ? dragonAttackCooldown[skillNumber] : skillCooldown[skillNumber];
            float current = cooldown - timeSinceLastExecuted;
            if (percentage)
            {
                // Normalized with cooldown
                current = current / cooldown;
            }

            return current < 0.0f ? 0.0f : current;
        }
    }

    private void AttackWithProjectile(ref ObjectPool objectPool, float damage, Vector3 currentPlayerPosition, Vector2 forwardVector, float rotationZ = 0.0f)
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

        if (spawnedObject.TryGetComponent<Projectile>(out Projectile arrow))
        {
            arrow.SetDirection(forwardVector);
            stats.CalculateDamage(damage, out float finalDamage, out bool crit);
            arrow.SetDamage(finalDamage, crit);
        }
        else
        {
            throw new System.InvalidOperationException();
        }
    }

    private void AttackWithHitbox(PlayerAttackHitbox desiredHitbox, float attackDamage)
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
                    enemy.TakeDamage(finalDamage, crit);
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

    private void AdjustClassAttributes(PlayerClass playerClass)
    {
        // This should be put in Adjust dragon stats function once it's implemented
        dragonAttackDamage = this.playerConfig.NightDragonConfig.dragonAttackDamage;
        dragonAttackCooldown = this.playerConfig.NightDragonConfig.dragonAttackCooldown;

        // Now change the class
        this.playerClass = playerClass;
        switch (playerClass)
        {
            case PlayerClass.Sword:
                ClassConfig swordConfig = playerConfig.SwordConfig;
                skillDamage = swordConfig.skillDamage;
                skillCooldown = swordConfig.skillCooldown;
                AdjustStats(new Stats(swordConfig.atk, swordConfig.agi, swordConfig.vit, swordConfig.tal, swordConfig.luk));
                // Secondary here too
                break;
            case PlayerClass.Archer:
                ClassConfig archerConfig = playerConfig.ArcherConfig;
                skillDamage = archerConfig.skillDamage;
                skillCooldown = archerConfig.skillCooldown;
                AdjustStats(new Stats(archerConfig.atk, archerConfig.agi, archerConfig.vit, archerConfig.tal, archerConfig.luk));
                // Secondary here too
                break;
            default:
                Debug.LogAssertion($"Invalid playerClass: {playerClass}");
                break;
        }
    }

    private void AdjustStats(Stats stats)
    {
        this.stats.AssignStats(stats);
        // Notify stats changed
        EventPublisher.TriggerPlayerStatsChange(stats);
        // Health might update
        EventPublisher.TriggerPlayerHealthChange();
    }

    private IEnumerator SwordWave(float damage, Vector2 forwardVector, float delay)
    {
        yield return new WaitForSeconds(delay);
        AttackWithProjectile(ref swordWaves, damage, transform.position, forwardVector);
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
