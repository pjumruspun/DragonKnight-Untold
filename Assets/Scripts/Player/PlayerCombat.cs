using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoSingleton<PlayerCombat>
{
    public bool IsRushing => DragonGauge.Instance.IsDragonForm && (CurrentSkills() as DragonSkills).IsRushing;
    public bool IsCastingSkill => CurrentSkills().IsCastingSkill;
    public int SwordCombo =>
        PlayerClassStatic.currentClass == PlayerClass.Sword ? ((SwordSkills)humanSkills).CurrentCombo : throw new System.InvalidOperationException();

    [Header("Hitboxes")]
    [SerializeField]
    private AttackHitbox swordPrimaryHitbox;
    [SerializeField]
    private AttackHitbox dragonPrimaryHitbox;
    [SerializeField]
    private AttackHitbox dragonVortexHitbox;
    [SerializeField]
    private AttackHitbox dragonRushHitbox;
    [SerializeField]
    private AttackHitbox airShotHitZone;

    [Header("Skill Effects")]
    [SerializeField]
    private GameObject dashEffect;
    [SerializeField]
    private GameObject fireBreath;
    [SerializeField]
    private GameObject clawSlash;
    [SerializeField]
    private GameObject dragonDashEffect;
    [SerializeField]
    private GameObject dragonHorizontalDashEffect;
    [SerializeField]
    private DashEffectSize chargedShotEffect;

    private DragonSkills dragonSkills => SkillsRepository.Dragon;
    private PlayerSkills humanSkills;

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
        float currentCooldownPercentage = CurrentSkills() == null ? 0.0f : CurrentSkills().CurrentCooldownPercentage(skillNumber);
        return currentCooldownPercentage;
    }

    private void Start()
    {
        // Subscribe
        EventPublisher.PlayerUseSkill += ActivateSkill;
        EventPublisher.PlayerChangeClass += ProcessChangingClass;
        EventPublisher.PlayerShapeshift += StopFireOnDragonDown;
        EventPublisher.StopFireBreath += StopFireBreath;
        EventPublisher.StopRush += StopRush;
        EventPublisher.StopChargeShot += StopChargeShot;

        // Initialize player starting class, player will get to choose this later
        InitializeSkills();
        ChangeClass(PlayerClassStatic.currentClass);
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerUseSkill -= ActivateSkill;
        EventPublisher.PlayerChangeClass -= ProcessChangingClass;
        EventPublisher.PlayerShapeshift -= StopFireOnDragonDown;
        EventPublisher.StopFireBreath -= StopFireBreath;
        EventPublisher.StopRush -= StopRush;
        EventPublisher.StopChargeShot -= StopChargeShot;
    }

    private void Update()
    {
        ProcessSkillCooldown();
        BuffManager.UpdateBuffManager();

        // Debugging only
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (PlayerClassStatic.currentClass == PlayerClass.Sword)
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
        else if ( // If player releases charge shot
            InputManager.Skill2Release &&
            PlayerClassStatic.currentClass == PlayerClass.Archer
            && !DragonGauge.Instance.IsDragonForm
        )
        {
            // Release
            (CurrentSkills() as ArcherSkills).NotifySkill2ToRelease();
        }
        else if ( // If player releases rush
            InputManager.Skill2Release &&
            DragonGauge.Instance.IsDragonForm
        )
        {
            EventPublisher.TriggerStopRush();
        }
        else if (InputManager.Skill3 && IsSkillReady(2))
        {
            if (PlayerClassStatic.currentClass == PlayerClass.Archer && !DragonGauge.Instance.IsDragonForm)
            {
                if (PlayerMovement.Instance.IsGrounded())
                {
                    // For archer, skill 2 (Air shot) can be activated only when on ground
                    EventPublisher.TriggerPlayerUseSkill(2);
                }
            }
            else
            {
                EventPublisher.TriggerPlayerUseSkill(2);
            }
        }
        else if (InputManager.UltimateSkill && IsSkillReady(3))
        {
            if (PlayerClassStatic.currentClass == PlayerClass.Sword && !DragonGauge.Instance.IsDragonForm)
            {
                // For sword, ultimate can be activated only when on ground
                if (PlayerMovement.Instance.IsGrounded())
                {
                    EventPublisher.TriggerPlayerUseSkill(3);
                }
            }
            else
            {
                EventPublisher.TriggerPlayerUseSkill(3);
            }
        }
        else if (InputManager.UltimateRelease && DragonGauge.Instance.IsDragonForm)
        {
            // Stop fire breath
            EventPublisher.TriggerStopFireBreath();
        }
    }

    private bool IsSkillReady(int skillNumber)
    {
        float currentCooldown = CurrentSkills().GetCurrentCooldown()[skillNumber];
        bool readyToAttack = currentCooldown <= 0.01f && !CurrentSkills().IsCastingSkill;
        // Debug.Log($"{skillNumber}, {currentCooldown}");
        return readyToAttack;
    }

    private void ActivateSkill(int skillNumber)
    {
        // if (PlayerClassStatic.currentClass == PlayerClass.Archer && !DragonGauge.Instance.IsDragonForm)
        // {
        //     if (!((ArcherSkills)CurrentSkills()).HasFiredSpreadShot)
        //     {
        //         // Only consume if hasn't fired spread shot
        //         PerkEffects.BerserkConsumeHealth();
        //     }
        // }
        // else
        // {
        //     // Always consumed if other classes
        //     PerkEffects.BerserkConsumeHealth();
        // }

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
                CurrentSkills().UltimateSkill();
                break;
            default:
                throw new System.InvalidOperationException();
        }
    }

    private void StopFireOnDragonDown(bool isDragon)
    {
        if (!isDragon)
        {
            // Stop fire breath if dragon down
            dragonSkills.UltimateRelease();
        }
    }

    private void StopFireBreath()
    {
        dragonSkills.UltimateRelease();
    }

    private void StopRush()
    {
        dragonSkills.RushRelease();
    }

    private void StopChargeShot()
    {
        if (PlayerClassStatic.currentClass == PlayerClass.Archer)
        {
            (CurrentSkills() as ArcherSkills).Skill2Release();
        }
        else
        {
            throw new System.InvalidOperationException("Attempt to call StopChargeShot when player is not an archer.");
        }
    }

    private void ProcessChangingClass(PlayerClass playerClass)
    {
        // Don't forget to transfer items and perks here once it's done

        // Change class label
        PlayerClassStatic.currentClass = playerClass;

        // Change current skill sets
        humanSkills = GetPlayerSkill(playerClass);

        // Assign dragon skills
        // dragonSkills = new DragonSkills(transform, dragonPrimaryHitbox, fireBreath, clawSlash);
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

    private PlayerSkills GetPlayerSkill(PlayerClass playerClass)
    {
        // Create a new instance of PlayerSkill depends on the given class
        switch (playerClass)
        {
            case PlayerClass.Sword:
                // Send this.stats pointer in
                return SkillsRepository.Sword;
            case PlayerClass.Archer:
                // Send this.stats pointer in
                return SkillsRepository.Archer;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    private void InitializeSkills()
    {
        SkillsRepository.Sword.Initialize(transform, swordPrimaryHitbox, dashEffect);
        SkillsRepository.Archer.Initialize(transform, chargedShotEffect, airShotHitZone);
        SkillsRepository.Dragon.Initialize(
            transform, dragonPrimaryHitbox, dragonVortexHitbox, dragonRushHitbox, fireBreath, clawSlash, dragonDashEffect, dragonHorizontalDashEffect
        );
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
