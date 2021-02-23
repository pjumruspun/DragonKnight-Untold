using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoSingleton<PlayerAbilities>
{
    public bool IsDragonForm => isDragonForm;

    [SerializeField]
    private float primaryAttackRate = 2.0f;
    private float timeSinceLastPrimaryAttack = 0.0f;
    private bool isDragonForm = false;

    private void Start()
    {
        timeSinceLastPrimaryAttack = 1.0f / primaryAttackRate;

        // Subscribe
        EventPublisher.PlayerPrimaryAttack += PrimaryAttack;
        EventPublisher.PlayerShapeshift += Shapeshift;
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
        timeSinceLastPrimaryAttack = 0.0f;
    }

    private void Shapeshift()
    {
        // Debug.Log($"Dragon form: {isDragonForm}");
    }
}
