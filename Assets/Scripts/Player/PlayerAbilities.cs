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
    private GameObject swordWavePrefab;
    private float timeSinceLastPrimaryAttack = 0.0f;
    private bool isDragonForm = false;
    private PlayerSkills playerSkills;


    public void ChangeClass(PlayerClass playerClass)
    {
        // Call this after player choose his/her class
        // Can call this at the camp after clearing a stage too?
        EventPublisher.TriggerPlayerChangeClass(playerClass);
    }

    private void Start()
    {
        // Initialize player skills
        playerSkills = new PlayerSkills(dragonPrimaryHitbox, swordPrimaryHitbox, arrowPrefab, swordWavePrefab);
        // Initialize player starting class, player will get to choose this later
        ChangeClass(PlayerClass.Sword);

        // This line means player is ready to attack right when this method is called
        timeSinceLastPrimaryAttack = playerSkills.SkillCooldown[0];

        // Subscribe
        EventPublisher.PlayerPrimaryAttack += PrimaryAttack;
        EventPublisher.PlayerShapeshift += Shapeshift;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerPrimaryAttack -= PrimaryAttack;
        EventPublisher.PlayerShapeshift -= Shapeshift;
        EventPublisher.PlayerChangeClass -= ProcessChangingClass;
    }

    private void Update()
    {
        ProcessDeltaTime();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerSkills.Class == PlayerClass.Sword)
            {
                ChangeClass(PlayerClass.Archer);
            }
            else
            {
                ChangeClass(PlayerClass.Sword);
            }

            Debug.Log(playerSkills.SkillDamage);
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
        if (InputManager.PrimaryAttack && IsSkillReady(1))
        {
            // Player attacks here
            EventPublisher.TriggerPlayerPrimaryAttack();
        }
    }

    private bool IsSkillReady(int number)
    {
        // Skill 1 = Primary attack
        // Skill 4 = Ultimate
        float currentCooldown = playerSkills.GetCurrentCooldown(number, timeSinceLastPrimaryAttack);
        bool readyToAttack = timeSinceLastPrimaryAttack >= currentCooldown;
        return readyToAttack;
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
        playerSkills.PrimaryAttack(transform.position, GetForwardVector());
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
