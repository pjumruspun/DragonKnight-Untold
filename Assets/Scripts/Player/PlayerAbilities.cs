using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoSingleton<PlayerAbilities>
{
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
    private DragonSkills dragonSkills;
    private PlayerSkills humanSkills;
    private PlayerClass currentClass;
    private List<Buff> buffs;


    public void ChangeClass(PlayerClass playerClass)
    {
        // Call this after player choose his/her class
        // Can call this at the camp after clearing a stage too?
        EventPublisher.TriggerPlayerChangeClass(playerClass);
    }

    public float CurrentCooldownPercentage(int skillNumber)
    {
        return CurrentSkills().CurrentCooldownPercentage(skillNumber);
    }

    private void Start()
    {
        // Subscribe
        EventPublisher.PlayerUseSkill += ActivateSkill;
        EventPublisher.PlayerShapeshift += ToggleSkillSet;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
        EventPublisher.StopFireBreath += StopFireBreath;

        // Initialize player starting class, player will get to choose this later
        ChangeClass(PlayerClass.Sword);
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerUseSkill -= ActivateSkill;
        EventPublisher.PlayerShapeshift -= ToggleSkillSet;
        EventPublisher.PlayerChangeClass -= ProcessChangingClass;
        EventPublisher.StopFireBreath -= StopFireBreath;
    }

    private void Update()
    {
        ProcessSkillCooldown();

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

        // Debugging
        if (Input.GetKeyDown(KeyCode.G))
        {

        }

        // If the player is still alive
        if (!PlayerHealth.Instance.IsDead)
        {
            ListenToAttackEvent();
        }
    }

    private void ProcessSkillCooldown()
    {
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
    }

    private bool IsSkillReady(int skillNumber)
    {
        float currentCooldown = CurrentSkills().CurrentCooldown()[skillNumber];
        bool readyToAttack = currentCooldown <= 0.01f;
        Debug.Log($"{skillNumber}, {currentCooldown}");
        return readyToAttack;
    }

    private void ActivateSkill(int skillNumber)
    {
        switch (skillNumber)
        {
            case 0:
                Debug.Log("test");
                CurrentSkills().Skill1(transform.position, GetForwardVector());
                break;
            case 1:
                CurrentSkills().Skill2(transform.position, GetForwardVector());
                break;
            case 2:
                throw new System.NotImplementedException();
            case 3:
                throw new System.NotImplementedException();
            default:
                throw new System.InvalidOperationException();
        }
    }

    private void StopFireBreath()
    {
        DragonSkills dragon = (DragonSkills)CurrentSkills();
        dragon.Skill2Release();
    }

    private void ToggleSkillSet(bool isDragon)
    {
        // if (isDragon)
        // {
        //     current = dragonSkills;
        // }
        // else
        // {
        //     // Stop breathing fire first as we need current to be
        //     // instance of DragonSkill
        //     EventPublisher.TriggerStopFireBreath();

        //     // Change current skill sets back
        //     current = humanSkills;
        // }
    }

    private void ProcessChangingClass(PlayerClass playerClass)
    {
        // Change class label
        currentClass = playerClass;

        // Change current skill sets
        humanSkills = CreatePlayerSkill(playerClass);

        // Assign dragon skills
        dragonSkills = new DragonSkills(transform, dragonPrimaryHitbox, humanSkills.PStats, fireBreath);
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

    private void AddBuff(Buff buff)
    {
        // TODO
    }

    private PlayerSkills CreatePlayerSkill(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Sword:
                return new SwordSkills(transform, swordPrimaryHitbox, swordWavePrefab);
            case PlayerClass.Archer:
                return new ArcherSkills(transform, arrowPrefab);
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    private PlayerSkills CurrentSkills()
    {
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
