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
    private float[] timeSinceLastSkillUsed = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
    private bool isDragonForm = false;
    private PlayerSkills playerSkills;


    public void ChangeClass(PlayerClass playerClass)
    {
        // Call this after player choose his/her class
        // Can call this at the camp after clearing a stage too?
        EventPublisher.TriggerPlayerChangeClass(playerClass);
    }

    public float GetCurrentSkillCooldown(int number, bool percentage = false)
    {
        // Skill 0 = Primary attack
        // Skill 3 = Ultimate
        float currentCooldown = playerSkills.GetCurrentCooldown(number, timeSinceLastSkillUsed[number], percentage);
        return currentCooldown;
    }

    private void Start()
    {
        // Initialize player skills
        playerSkills = new PlayerSkills(dragonPrimaryHitbox, swordPrimaryHitbox, arrowPrefab, swordWavePrefab);
        // Initialize player starting class, player will get to choose this later
        ChangeClass(PlayerClass.Sword);

        // This line means player is ready to attack right when this method is called
        for (int i = 0; i < timeSinceLastSkillUsed.Length; ++i)
        {
            timeSinceLastSkillUsed[i] = playerSkills.SkillCooldown[i];
        }

        // Subscribe
        EventPublisher.PlayerUseSkill += ActivateSkill;
        EventPublisher.PlayerShapeshift += Shapeshift;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerUseSkill -= ActivateSkill;
        EventPublisher.PlayerShapeshift -= Shapeshift;
        EventPublisher.PlayerChangeClass -= ProcessChangingClass;
    }

    private void Update()
    {
        ProcessDeltaTime();

        if (Input.GetKeyDown(KeyCode.T))
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
        for (int i = 0; i < timeSinceLastSkillUsed.Length; ++i)
        {
            // Loop just 4, shouldn't hurt
            timeSinceLastSkillUsed[i] += Time.deltaTime;
        }
    }

    private void ListenToAttackEvent()
    {
        if (InputManager.PrimaryAttack && IsSkillReady(0))
        {
            // Player attacks here
            EventPublisher.TriggerPlayerUseSkill(0);
        }
        else if (InputManager.Skill2 && IsSkillReady(1))
        {
            // Player skill 2
            EventPublisher.TriggerPlayerUseSkill(1);
        }
    }

    private bool IsSkillReady(int number)
    {
        float currentCooldown = GetCurrentSkillCooldown(number);
        bool readyToAttack = currentCooldown <= 0.01f;
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

    private void ActivateSkill(int skillNumber)
    {
        switch (skillNumber)
        {
            case 0:
                playerSkills.Skill1(transform.position, GetForwardVector());
                break;
            case 1:
                playerSkills.Skill2(transform.position, GetForwardVector());
                break;
            case 2:
                throw new System.NotImplementedException();
            case 3:
                throw new System.NotImplementedException();
            default:
                throw new System.InvalidOperationException();
        }

        timeSinceLastSkillUsed[skillNumber] = 0.0f;
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
