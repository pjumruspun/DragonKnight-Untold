using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public static PlayerAbilities Instance { get; private set; }
    public bool IsDragonForm => isDragonForm;

    [SerializeField]
    private float primaryAttackRate = 2.0f;
    private float timeSinceLastPrimaryAttack = 0.0f;
    private bool isDragonForm = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

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
        ListenToAttackEvent();
        ListenToShapeshiftEvent();
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
            EventPublisher.TriggerPlayerPrimaryAttack();
        }
    }

    private void ListenToShapeshiftEvent()
    {
        if (InputManager.Shapeshift)
        {
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
        Debug.Log($"Dragon form: {isDragonForm}");
    }
}
