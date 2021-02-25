using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoSingleton<PlayerAbilities>
{
    public bool IsDragonForm => isDragonForm;

    [SerializeField]
    private PlayerAttackHitbox swordPrimaryHitbox;
    [SerializeField]
    private PlayerAttackHitbox dragonPrimaryHitbox;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private float primaryAttackDamage;
    private float primaryAttackRate;
    private float dragonPrimaryAttackDamage = 56.2f;
    private float dragonPrimaryAttackRate = 3.0f;
    private float timeSinceLastPrimaryAttack = 0.0f;
    private bool isDragonForm = false;
    private PlayerClass playerClass;

    public void ChangeClass(PlayerClass playerClass)
    {
        // Call this after player choose his/her class
        // Can call this at the camp after clearing a stage too?
        EventPublisher.TriggerPlayerChangeClass(playerClass);
    }

    private void Start()
    {
        SetPlayerAttributes();
        timeSinceLastPrimaryAttack = 1.0f / primaryAttackRate;

        // Subscribe
        EventPublisher.PlayerPrimaryAttack += PrimaryAttack;
        EventPublisher.PlayerShapeshift += Shapeshift;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
    }

    private void SetPlayerAttributes()
    {
        PlayerConfig playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        ClassConfig classConfig = playerConfig.SwordConfig; // This should change depends on player class
        primaryAttackRate = classConfig.primaryAttackRate;
        primaryAttackDamage = classConfig.primaryAttackDamage;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerPrimaryAttack -= PrimaryAttack;
        EventPublisher.PlayerShapeshift -= Shapeshift;
    }

    private void Update()
    {
        ProcessDeltaTime();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerClass == PlayerClass.Sword)
            {
                ChangeClass(PlayerClass.Archer);
            }
            else
            {
                ChangeClass(PlayerClass.Sword);
            }

            Debug.Log(primaryAttackDamage);
        }

        // If the player is still alive
        if (!PlayerHealth.Instance.IsDead)
        {
            ListenToAttackEvent();
            ListenToShapeshiftEvent();
        }
    }

    private void ProcessDeltaTime()
    {
        timeSinceLastPrimaryAttack += Time.deltaTime;
    }

    private void ListenToAttackEvent()
    {
        float attackRate = IsDragonForm ? dragonPrimaryAttackRate : primaryAttackRate;
        bool readyToAttack = timeSinceLastPrimaryAttack >= 1.0f / attackRate;
        if (InputManager.PrimaryAttack && readyToAttack)
        {
            // Player attacks here
            EventPublisher.TriggerPlayerPrimaryAttack();
        }
    }

    private void ListenToShapeshiftEvent()
    {
        if (InputManager.Shapeshift)
        {
            // Player transforms here
            isDragonForm = !isDragonForm;
            EventPublisher.TriggerPlayerShapeshift();
        }
    }

    private void PrimaryAttack()
    {
        if (IsDragonForm)
        {
            // Dragon Primary Attack
            AttackWithHitbox(dragonPrimaryHitbox, dragonPrimaryAttackDamage);
        }
        else
        {
            // Player Primary Attack
            switch (playerClass)
            {
                case PlayerClass.Sword:
                    AttackWithHitbox(swordPrimaryHitbox, primaryAttackDamage);
                    break;
                case PlayerClass.Archer:
                    // Spawn arrow
                    GameObject spawnedObject = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                    if (spawnedObject.TryGetComponent<Projectile>(out Projectile arrow))
                    {
                        arrow.SetDirection(GetForwardVector());
                        arrow.SetDamage(primaryAttackDamage);
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

    public void AttackWithHitbox(PlayerAttackHitbox desiredHitbox, float attackDamage)
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

        timeSinceLastPrimaryAttack = 0.0f;
    }

    private void Shapeshift()
    {
        // Debug.Log($"Dragon form: {isDragonForm}");
    }

    private void ProcessChangingClass(PlayerClass playerClass)
    {
        // Player should not be in dragon form when changing class, just in case
        if (IsDragonForm)
        {
            // Player dragon down
            isDragonForm = !isDragonForm;
            EventPublisher.TriggerPlayerShapeshift();
        }

        // Now change the class for real
        this.playerClass = playerClass;
        PlayerConfig playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        switch (playerClass)
        {
            case PlayerClass.Sword:
                ClassConfig swordConfig = playerConfig.SwordConfig;
                primaryAttackDamage = swordConfig.primaryAttackDamage;
                primaryAttackRate = swordConfig.primaryAttackRate;
                // Secondary here too
                break;
            case PlayerClass.Archer:
                ClassConfig archerConfig = playerConfig.ArcherConfig;
                primaryAttackDamage = archerConfig.primaryAttackDamage;
                primaryAttackRate = archerConfig.primaryAttackRate;
                // Secondary here too
                break;
            default:
                Debug.LogAssertion($"Invalid playerClass: {playerClass}");
                break;
        }
    }

    private Vector2 GetForwardVector()
    {
        switch (PlayerMovement.Instance.TurnDirection)
        {
            case PlayerMovement.MovementState.Right:
                return transform.right;
            case PlayerMovement.MovementState.Left:
                return -transform.right;
            case PlayerMovement.MovementState.Idle:
                throw new System.InvalidOperationException();
            default:
                throw new System.NotImplementedException();
        }
    }
}
