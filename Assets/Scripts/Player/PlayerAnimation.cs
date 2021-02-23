using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private RuntimeAnimatorController dragonAnimator;

    [SerializeField]
    private RuntimeAnimatorController humanAnimator;

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = humanAnimator as RuntimeAnimatorController;

        // Subscribe
        EventPublisher.PlayerPrimaryAttack += PlayPrimaryAttackAnimation;
        EventPublisher.PlayerJump += PlayJumpAnimation;
        EventPublisher.PlayerLand += PlayLandAnimation;
        EventPublisher.PlayerRun += PlayRunAnimation;
        EventPublisher.PlayerStop += PlayIdleAnimation;
        EventPublisher.PlayerShapeshift += PlayShapeshiftAnimation;
        EventPublisher.PlayerDead += PlayDeadAnimation;
    }

    private void Update()
    {
        animator.SetBool("Midair", !PlayerMovement.Instance.IsGrounded());
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.runtimeAnimatorController = dragonAnimator as RuntimeAnimatorController;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe
        EventPublisher.PlayerPrimaryAttack -= PlayPrimaryAttackAnimation;
        EventPublisher.PlayerJump -= PlayJumpAnimation;
        EventPublisher.PlayerLand -= PlayLandAnimation;
        EventPublisher.PlayerRun -= PlayRunAnimation;
        EventPublisher.PlayerStop -= PlayIdleAnimation;
        EventPublisher.PlayerShapeshift -= PlayShapeshiftAnimation;
        EventPublisher.PlayerDead -= PlayDeadAnimation;
    }

    private void PlayPrimaryAttackAnimation()
    {
        animator.SetTrigger("PrimaryAttack");
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
            animator.runtimeAnimatorController = dragonAnimator as RuntimeAnimatorController;
        }
        else
        {
            // Player just transformed into a human
            // animator.SetBool("DragonForm", false);
            animator.runtimeAnimatorController = humanAnimator as RuntimeAnimatorController;
        }
    }

    private void PlayDeadAnimation()
    {
        animator.SetTrigger("Dead");
    }
}