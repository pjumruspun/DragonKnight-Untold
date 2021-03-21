using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State after enemy attacked, and skill is on cooldown, and repositioning is not needed
/// </summary>
public class EnemyAttackCooldown : EnemyBehavior
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ListenToAttackSignal();
        ListenToChaseSignal();
        TurnTowardPlayer();
    }

    private void TurnTowardPlayer()
    {
        bool playerIsOnLeft = transform.position.x > player.position.x;
        if (playerIsOnLeft)
        {
            enemy.Flip(MovementState.Left);
        }
        else
        {
            enemy.Flip(MovementState.Right);
        }
    }
}
