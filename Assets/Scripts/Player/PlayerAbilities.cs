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
        // Debug.Log("Primary attack");
        PlayerAttackHitbox desiredHitbox = IsDragonForm ? dragonPrimaryHitbox : swordPrimaryHitbox; // Sword hitbox will need to change
        float attackDamage = IsDragonForm ? dragonPrimaryAttackDamage : primaryAttackDamage;

        foreach (Collider2D enemyCollider in desiredHitbox.HitColliders)
        {
            GameObject enemyObject = enemyCollider.gameObject;
            if (enemyObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Damage the enemy here
                enemy.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogAssertion($"gameObject {enemyCollider.gameObject.name} does not have EnemyHealth attached to.");
            }
        }

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
                ClassConfig archerConfig = playerConfig.SwordConfig;
                primaryAttackDamage = archerConfig.primaryAttackDamage;
                primaryAttackRate = archerConfig.primaryAttackRate;
                // Secondary here too
                break;
            default:
                Debug.LogAssertion($"Invalid playerClass: {playerClass}");
                break;
        }
    }
}
