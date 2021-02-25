using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
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

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = swordController as RuntimeAnimatorController;

        // Subscribe
        EventPublisher.PlayerUseSkill += PlaySkillAnimation;
        EventPublisher.PlayerJump += PlayJumpAnimation;
        EventPublisher.PlayerLand += PlayLandAnimation;
        EventPublisher.PlayerRun += PlayRunAnimation;
        EventPublisher.PlayerStop += PlayIdleAnimation;
        EventPublisher.PlayerShapeshift += PlayShapeshiftAnimation;
        EventPublisher.PlayerDead += PlayDeadAnimation;
        EventPublisher.PlayerChangeClass += ChangeHumanAnimator;
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
                animator.SetTrigger("Skill2");
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

    private void PlayShapeshiftAnimation()
    {
        if (PlayerAbilities.Instance.IsDragonForm)
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
}