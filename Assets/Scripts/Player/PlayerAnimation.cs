using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoSingleton<PlayerAnimation>
{
    // Dragon animators
    [SerializeField]
    private RuntimeAnimatorController nightController;

    // Human animators
    [SerializeField]
    private RuntimeAnimatorController archerController;
    [SerializeField]
    private RuntimeAnimatorController swordController;

    // Base
    private Animator animator;

    // Cache config
    private PlayerConfig playerConfig;

    // Map string to each class/dragon
    private Dictionary<int, string[]> nSkillToAnimationName = new Dictionary<int, string[]>
    {
        { 0, new string[]{ "Sword_Attack1", "Archer_Attack1", "Night_Claw"}},
        { 1, new string[]{ "Sword_Attack3", "?", "?"}}
    };

    // Animation clip length according to skill number
    // Already consider player's state like dragon/class
    public float GetAnimLength(int skillNumber)
    {
        name = GetAnimName(skillNumber);
        AnimationClip[] anims = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < anims.Length; ++i)
        {
            if (anims[i].name == name)
            {
                // Debug.Log($"{anims[i].name}: {anims[i].length}");
                return anims[i].length;
            }
        }

        throw new System.IndexOutOfRangeException();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = swordController as RuntimeAnimatorController;
        playerConfig = ConfigContainer.Instance.GetPlayerConfig;

        // Subscribe
        EventPublisher.PlayerUseSkill += PlaySkillAnimation;
        EventPublisher.PlayerJump += PlayJumpAnimation;
        EventPublisher.PlayerLand += PlayLandAnimation;
        EventPublisher.PlayerRun += PlayRunAnimation;
        EventPublisher.PlayerStop += PlayIdleAnimation;
        EventPublisher.PlayerShapeshift += PlayShapeshiftAnimation;
        EventPublisher.PlayerDead += PlayDeadAnimation;
        EventPublisher.PlayerChangeClass += ChangeHumanAnimator;
        EventPublisher.StopFireBreath += PlayStopFireBreathAnimation;
    }

    private void Update()
    {
        animator.SetBool("Midair", !PlayerMovement.Instance.IsGrounded());
    }

    private void OnDestroy()
    {
        // Unsubscribe
        EventPublisher.PlayerUseSkill -= PlaySkillAnimation;
        EventPublisher.PlayerJump -= PlayJumpAnimation;
        EventPublisher.PlayerLand -= PlayLandAnimation;
        EventPublisher.PlayerRun -= PlayRunAnimation;
        EventPublisher.PlayerStop -= PlayIdleAnimation;
        EventPublisher.PlayerShapeshift -= PlayShapeshiftAnimation;
        EventPublisher.PlayerDead -= PlayDeadAnimation;
        EventPublisher.PlayerChangeClass -= ChangeHumanAnimator;
        EventPublisher.StopFireBreath -= PlayStopFireBreathAnimation;
    }

    private void PlaySkillAnimation(int skillNumber)
    {
        // Need to accelerate the animation
        switch (skillNumber)
        {
            case 0:
                animator.SetTrigger("PrimaryAttack");
                break;
            case 1:
                if (DragonGauge.Instance.IsDragonForm)
                {
                    // Fire breath
                    animator.SetBool("Fire", true);
                }
                else
                {
                    animator.SetTrigger("Skill2");
                }
                break;
            case 2:
                throw new System.NotImplementedException();
            case 3:
                throw new System.NotImplementedException();
            default:
                throw new System.InvalidOperationException();
        }
    }

    private void PlayJumpAnimation()
    {
        // Reset to make sure the landing animation
        // is performed smoothly
        // animator.ResetTrigger("SwordLand");
        animator.SetTrigger("Jump");
    }

    private void PlayLandAnimation()
    {
        // animator.SetTrigger("SwordLand");
    }

    private void PlayRunAnimation()
    {
        // Need to accelerate the animation
        animator.SetBool("Running", true);
    }

    private void PlayIdleAnimation()
    {
        animator.SetBool("Running", false);
    }

    private void PlayShapeshiftAnimation(bool isDragon)
    {
        if (isDragon)
        {
            // Player just transformed into a dragon
            // animator.SetBool("DragonForm", true);
            animator.runtimeAnimatorController = nightController as RuntimeAnimatorController;
        }
        else
        {
            // Player just transformed into a human
            // animator.SetBool("DragonForm", false);
            animator.runtimeAnimatorController = swordController as RuntimeAnimatorController;
        }
    }

    private void PlayDeadAnimation()
    {
        animator.SetBool("Dead", true);
    }

    private void PlayStopFireBreathAnimation()
    {
        animator.SetBool("Fire", false);
    }

    private void ChangeHumanAnimator(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Sword:
                animator.runtimeAnimatorController = swordController as RuntimeAnimatorController;
                break;
            case PlayerClass.Archer:
                animator.runtimeAnimatorController = archerController as RuntimeAnimatorController;
                break;
            default:
                Debug.LogAssertion($"Invalid playerClass: {playerClass}");
                break;
        }
    }

    private string GetAnimName(int skillNumber)
    {
        return nSkillToAnimationName[skillNumber][GetIndex()];
    }

    private int GetIndex()
    {
        if (DragonGauge.Instance.IsDragonForm)
        {
            // Map to night's animation
            return 2;
        }
        else
        {
            switch (PlayerAbilities.Instance.CurrentClass)
            {
                case PlayerClass.Sword:
                    return 0;
                case PlayerClass.Archer:
                    return 1;
                default:
                    throw new System.IndexOutOfRangeException();
            }
        }
    }
}