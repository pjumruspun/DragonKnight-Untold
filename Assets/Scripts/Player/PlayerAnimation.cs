using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Subscribe
        EventPublisher.PlayerPrimaryAttack += PlayPrimaryAttackAnimation;
        EventPublisher.PlayerJump += PlayJumpAnimation;
        EventPublisher.PlayerLand += PlayLandAnimation;
        EventPublisher.PlayerRun += PlayRunAnimation;
        EventPublisher.PlayerStop += PlayIdleAnimation;
        EventPublisher.PlayerShapeshift += PlayShapeshiftAnimation;
    }

    private void Update()
    {
        animator.SetBool("Midair", !PlayerMovement.Instance.IsGrounded());
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
    }

    private void PlayPrimaryAttackAnimation()
    {
        animator.SetTrigger("SwordAttack");
    }

    private void PlayJumpAnimation()
    {
        // Reset to make sure the landing animation
        // is performed smoothly
        animator.ResetTrigger("SwordLand");
        animator.SetTrigger("SwordJump");
    }

    private void PlayLandAnimation()
    {
        animator.SetTrigger("SwordLand");
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
            animator.SetBool("DragonForm", true);
            animator.SetTrigger("DragonUp");
        }
        else
        {
            // Player just transformed into a human
            animator.SetBool("DragonForm", false);
            animator.SetTrigger("DragonDown");
        }
    }
}