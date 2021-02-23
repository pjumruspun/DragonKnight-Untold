using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoSingleton<PlayerAbilities>
{
    public bool IsDragonForm => isDragonForm;

    [SerializeField]
    private PlayerConfig config;
    [SerializeField]
    private PlayerAttackHitbox swordPrimaryHitbox;
    [SerializeField]
    private float primaryAttackDamage;
    private float primaryAttackRate;
    private float timeSinceLastPrimaryAttack = 0.0f;
    private bool isDragonForm = false;

    private void Start()
    {
        SetPlayerAttributes();
        timeSinceLastPrimaryAttack = 1.0f / primaryAttackRate;

        // Subscribe
        EventPublisher.PlayerPrimaryAttack += PrimaryAttack;
        EventPublisher.PlayerShapeshift += Shapeshift;
    }

    private void SetPlayerAttributes()
    {
        PlayerConfig playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        WeaponConfig weaponConfig = playerConfig.SwordConfig; // This should change depends on weapon type
        primaryAttackRate = weaponConfig.primaryAttackRate;
        primaryAttackDamage = weaponConfig.primaryAttackDamage;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerPrimaryAttack -= PrimaryAttack;
        EventPublisher.PlayerShapeshift -= Shapeshift;
    }

    private void Update()
    {
        ProcessDeltaTime();

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
        bool readyToAttack = timeSinceLastPrimaryAttack >= 1.0f / primaryAttackRate;
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
        foreach (Collider2D enemyCollider in swordPrimaryHitbox.HitColliders)
        {
            GameObject enemyObject = enemyCollider.gameObject;
            if (enemyObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                // Damage the enemy here
                enemyHealth.TakeDamage(primaryAttackDamage);
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
}
