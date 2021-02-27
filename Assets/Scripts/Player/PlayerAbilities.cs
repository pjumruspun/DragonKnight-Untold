using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoSingleton<PlayerAbilities>
{
    public bool IsDragonForm => isDragonForm;
    public PlayerClass CurrentClass => currentClass;

    [SerializeField]
    private PlayerAttackHitbox swordPrimaryHitbox;
    [SerializeField]
    private PlayerAttackHitbox dragonPrimaryHitbox;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject swordWavePrefab;
    [SerializeField]
    private GameObject fireBreath;
    private float[] timeSinceLastSkillUsed = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
    private bool isDragonForm = false;
    private SwordSkills swordSkills;
    private ArcherSkills archerSkills;
    private DragonSkills dragonSkills;
    private PlayerSkills current;
    private PlayerClass currentClass;


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
        float currentCooldown = current.GetCurrentCooldown(number, timeSinceLastSkillUsed[number], percentage);
        return currentCooldown;
    }

    private void Start()
    {
        // Subscribe
        EventPublisher.PlayerUseSkill += ActivateSkill;
        EventPublisher.PlayerShapeshift += Shapeshift;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
        EventPublisher.StopFireBreath += StopFireBreath;

        // Initialize player skills
        swordSkills = new SwordSkills(transform, swordPrimaryHitbox, swordWavePrefab);
        archerSkills = new ArcherSkills(transform, arrowPrefab);

        // Initialize player starting class, player will get to choose this later
        ChangeClass(PlayerClass.Sword);

        // This line means player is ready to attack right when this method is called
        for (int i = 0; i < timeSinceLastSkillUsed.Length; ++i)
        {
            timeSinceLastSkillUsed[i] = current.SkillCooldown[i];
        }


    }

    private void OnDestroy()
    {
        EventPublisher.PlayerUseSkill -= ActivateSkill;
        EventPublisher.PlayerShapeshift -= Shapeshift;
        EventPublisher.PlayerChangeClass -= ProcessChangingClass;
        EventPublisher.StopFireBreath -= StopFireBreath;
    }

    private void Update()
    {
        ProcessDeltaTime();

        // Debugging
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentClass == PlayerClass.Sword)
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
        else if (InputManager.Skill2Release && IsDragonForm)
        {
            // Stop fire breath
            EventPublisher.TriggerStopFireBreath();
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
            EventPublisher.TriggerPlayerShapeshift(isDragonForm);
        }
    }

    private void ActivateSkill(int skillNumber)
    {
        switch (skillNumber)
        {
            case 0:
                current.Skill1(transform.position, GetForwardVector());
                break;
            case 1:
                current.Skill2(transform.position, GetForwardVector());
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

    private void StopFireBreath()
    {
        DragonSkills dragon = (DragonSkills)current;
        dragon.Skill2Release();
    }

    private void Shapeshift(bool isDragon)
    {
        if (isDragon)
        {
            current = dragonSkills;
        }
        else
        {
            // Stop breathing fire first as we need current to be
            // instance of DragonSkill
            EventPublisher.TriggerStopFireBreath();

            // Change current skill sets back
            switch (currentClass)
            {
                case PlayerClass.Sword:
                    current = swordSkills;
                    break;
                case PlayerClass.Archer:
                    current = archerSkills;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }

    private void ProcessChangingClass(PlayerClass playerClass)
    {
        // Change class label
        currentClass = playerClass;

        // Change current skill sets
        switch (playerClass)
        {
            case PlayerClass.Sword:
                current = swordSkills;
                break;
            case PlayerClass.Archer:
                current = archerSkills;
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }

        // Assign dragon skills
        dragonSkills = new DragonSkills(transform, dragonPrimaryHitbox, current.PStats, fireBreath);

        // Player should not be in dragon form when changing class, just in case
        if (IsDragonForm)
        {
            // Player dragon down
            isDragonForm = !isDragonForm;
            EventPublisher.TriggerPlayerShapeshift(isDragonForm);
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
