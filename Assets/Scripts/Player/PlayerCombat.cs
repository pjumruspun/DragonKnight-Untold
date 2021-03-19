using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoSingleton<PlayerCombat>
{
    public int SwordCombo =>
        currentClass == PlayerClass.Sword ? ((SwordSkills)humanSkills).CurrentCombo : throw new System.InvalidOperationException();
    public PlayerClass CurrentClass => currentClass;

    [Header("Hitboxes")]
    [SerializeField]
    private AttackHitbox swordPrimaryHitbox;
    [SerializeField]
    private AttackHitbox dragonPrimaryHitbox;

    [Header("Skill Effects")]
    [SerializeField]
    private GameObject fireBreath;
    [SerializeField]
    private GameObject clawSlash;

    private DragonSkills dragonSkills;
    private PlayerSkills humanSkills;
    private PlayerClass currentClass;
    private BuffManager buffManager;

    public void ChangeClass(PlayerClass playerClass)
    {
        // Call this after player choose his/her class
        // Can call this at the camp after clearing a stage too?
        EventPublisher.TriggerPlayerChangeClass(playerClass);
    }

    public float[] GetCurrentCooldown()
    {
        return CurrentSkills().GetCurrentCooldown();
    }

    public float CurrentCooldownPercentage(int skillNumber)
    {
        return CurrentSkills().CurrentCooldownPercentage(skillNumber);
    }

    private void Start()
    {
        // Subscribe
        EventPublisher.PlayerUseSkill += ActivateSkill;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
        EventPublisher.PlayerShapeshift += StopFireOnDragonDown;
        EventPublisher.StopFireBreath += StopFireBreath;

        // Initialize player starting class, player will get to choose this later
        ChangeClass(PlayerClass.Sword);

        // Initialize buff manager
        buffManager = new BuffManager();
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerUseSkill -= ActivateSkill;
        EventPublisher.PlayerChangeClass -= ProcessChangingClass;
        EventPublisher.PlayerShapeshift -= StopFireOnDragonDown;
        EventPublisher.StopFireBreath -= StopFireBreath;
    }

    private void Update()
    {
        ProcessSkillCooldown();
        buffManager.UpdateBuffManager();

        // Debugging only
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

        // Debugging only
        if (Input.GetKeyDown(KeyCode.G))
        {
            buffManager.AddSwordUltBuff();
        }

        // If the player is still alive
        if (!PlayerHealth.Instance.IsDead)
        {
            ListenToAttackEvent();
        }
    }

    private void ProcessSkillCooldown()
    {
        // No matter which form the player is in
        // Both should have their cooldown refresh simultaneously
        dragonSkills.ProcessSkillCooldown();
        humanSkills.ProcessSkillCooldown();
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
        else if (InputManager.Skill2Release && DragonGauge.Instance.IsDragonForm)
        {
            // Stop fire breath
            EventPublisher.TriggerStopFireBreath();
        }
        else if (InputManager.Skill3 && IsSkillReady(2))
        {
            EventPublisher.TriggerPlayerUseSkill(2);
        }
    }

    private bool IsSkillReady(int skillNumber)
    {
        float currentCooldown = CurrentSkills().GetCurrentCooldown()[skillNumber];
        bool readyToAttack = currentCooldown <= 0.01f;
        // Debug.Log($"{skillNumber}, {currentCooldown}");
        return readyToAttack;
    }

    private void ActivateSkill(int skillNumber)
    {
        switch (skillNumber)
        {
            case 0:
                // Debug.Log("test");
                CurrentSkills().Skill1();
                break;
            case 1:
                CurrentSkills().Skill2();
                break;
            case 2:
                CurrentSkills().Skill3();
                break;
            case 3:
                throw new System.NotImplementedException();
            default:
                throw new System.InvalidOperationException();
        }
    }

    private void StopFireOnDragonDown(bool isDragon)
    {
        if (!isDragon)
        {
            // Stop fire breath if dragon down
            dragonSkills.Skill2Release();
        }
    }

    private void StopFireBreath()
    {
        dragonSkills.Skill2Release();
    }

    private void ProcessChangingClass(PlayerClass playerClass)
    {
        // Don't forget to transfer items and perks here once it's done

        // Change class label
        currentClass = playerClass;

        // Change current skill sets
        humanSkills = CreatePlayerSkill(playerClass);

        // Assign dragon skills
        dragonSkills = new DragonSkills(transform, dragonPrimaryHitbox, fireBreath, clawSlash);
    }

    private Vector2 GetForwardVector()
    {
        switch (PlayerMovement.Instance.TurnDirection)
        {
            case MovementState.Right:
                return transform.right;
            case MovementState.Left:
                return -transform.right;
            case MovementState.Idle:
                throw new System.InvalidOperationException();
            default:
                throw new System.NotImplementedException();
        }
    }

    private PlayerSkills CreatePlayerSkill(PlayerClass playerClass)
    {
        // Create a new instance of PlayerSkill depends on the given class
        switch (playerClass)
        {
            case PlayerClass.Sword:
                // Send this.stats pointer in
                return new SwordSkills(transform, swordPrimaryHitbox);
            case PlayerClass.Archer:
                // Send this.stats pointer in
                return new ArcherSkills(transform);
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    private PlayerSkills CurrentSkills()
    {
        // returns an appropriate skill sets
        // whether player is in human or dragon form
        if (DragonGauge.Instance.IsDragonForm)
        {
            return dragonSkills;
        }
        else
        {
            return humanSkills;
        }
    }
}
