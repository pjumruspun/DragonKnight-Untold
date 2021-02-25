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
    private GameObject arrowPrefab;
    private PlayerConfig playerConfig;
    private PlayerAttackHitbox dragonPrimaryHitbox;
    private PlayerAttackHitbox swordPrimaryHitbox;
    private float[] dragonAttackDamage;
    private float[] dragonAttackCooldown;
    private float[] skillDamage;
    private float[] skillCooldown;
    private PlayerClass playerClass;
    private ObjectPool arrows;
    private ObjectPool swordWaves;

    public PlayerSkills(PlayerAttackHitbox dragonPrimaryHitbox, PlayerAttackHitbox swordPrimaryHitbox, GameObject arrowPrefab, GameObject swordWavePrefab)
    {
        playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        this.dragonPrimaryHitbox = dragonPrimaryHitbox;
        this.swordPrimaryHitbox = swordPrimaryHitbox;
        this.arrowPrefab = arrowPrefab;

        // Object pooling
        arrows = new ObjectPool(arrowPrefab, 20);
        swordWaves = new ObjectPool(swordWavePrefab, 20);

        // Subscribe
        EventPublisher.PlayerChangeClass += AdjustStats;
    }

    public void Dispose()
    {
        // Destructor, call on garbage collects
        EventPublisher.PlayerChangeClass -= AdjustStats;
    }

    public void PrimaryAttack(Vector3 currentPlayerPosition, Vector2 forwardVector) // Player position for arrow spawning position
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
                    GameObject spawnedObject = arrows.SpawnObject(currentPlayerPosition, Quaternion.identity);
                    if (spawnedObject.TryGetComponent<Projectile>(out Projectile arrow))
                    {
                        Debug.Log(forwardVector);
                        arrow.SetDirection(forwardVector);
                        arrow.SetDamage(damage);
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                    break;
                default:
                    throw new System.NotImplementedException();

            }
        }
    }

    public float GetCurrentCooldown(int skillNumber, float timeSinceLastExecuted)
    {
        if (skillNumber < 1 || skillNumber > 4)
        {
            throw new System.InvalidOperationException($"Error skillNumber {skillNumber} is not in between 1 and 4");
        }
        else
        {
            ++skillNumber; // to match with array
            float cooldown = PlayerAbilities.Instance.IsDragonForm ? dragonAttackCooldown[skillNumber] : skillCooldown[skillNumber];
            float current = cooldown - timeSinceLastExecuted;
            return current < 0.0f ? 0.0f : current;
        }
    }

    private void AttackWithHitbox(PlayerAttackHitbox desiredHitbox, float attackDamage)
    {
        HashSet<Collider2D> collidersToRemove = new HashSet<Collider2D>();
        // Debug.Log(desiredHitbox.HitColliders.Count);
        Debug.Log($"Before: {desiredHitbox.HitColliders.Count}");
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
                    enemy.TakeDamage(attackDamage);
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

        Debug.Log($"After iterated: {desiredHitbox.HitColliders.Count}");
        // Remove the unrelated colliders
        foreach (Collider2D unrelatedCollider in collidersToRemove)
        {
            desiredHitbox.HitColliders.Remove(unrelatedCollider);
        }

        Debug.Log($"After remove: {desiredHitbox.HitColliders.Count}");
    }

    private void AdjustStats(PlayerClass playerClass)
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
                // Secondary here too
                break;
            case PlayerClass.Archer:
                ClassConfig archerConfig = playerConfig.ArcherConfig;
                skillDamage = archerConfig.skillDamage;
                skillCooldown = archerConfig.skillCooldown;
                // Secondary here too
                break;
            default:
                Debug.LogAssertion($"Invalid playerClass: {playerClass}");
                break;
        }
    }
}
