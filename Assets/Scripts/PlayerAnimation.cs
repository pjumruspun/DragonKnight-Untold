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
        EventPublisher.PlayerJump += PlayJumpAnimation;
        EventPublisher.PlayerLand += PlayLandAnimation;
        EventPublisher.PlayerRun += PlayRunAnimation;
        EventPublisher.PlayerStop += PlayIdleAnimation;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        EventPublisher.PlayerJump -= PlayJumpAnimation;
        EventPublisher.PlayerLand -= PlayLandAnimation;
        EventPublisher.PlayerRun -= PlayRunAnimation;
        EventPublisher.PlayerStop -= PlayIdleAnimation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAnimation("SwordAttack");
        }
    }

    private void PlayAnimation(string param)
    {
        Debug.Log($"Playing animation {param}");
        animator.SetTrigger(param);
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
        animator.ResetTrigger("SwordIdle");
        animator.SetTrigger("SwordRun");
    }

    private void PlayIdleAnimation()
    {
        animator.ResetTrigger("SwordRun");
        animator.SetTrigger("SwordIdle");
    }
}