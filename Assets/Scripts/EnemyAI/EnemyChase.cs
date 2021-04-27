using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBehavior
{
    private const float spaceOffset = 0.5f;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Chase();
        ListenToAttackSignal();
    }

    private void Chase()
    {
        if (enemy.ShouldChase)
        {
            bool inAttackRangeLeft = transform.position.x < player.position.x - enemy.AttackRange;
            bool inAttackRangeRight = transform.position.x > player.position.x + enemy.AttackRange;
            if (inAttackRangeLeft)
            {
                // Enemy is on the left side of the player
                // Go right
                GoRight();
            }
            else if (inAttackRangeRight)
            {
                // Enemy is on the right side of the player
                // Go left
                GoLeft();
            }
        }
        else
        {
            // Switch state
            animator.SetTrigger("Patrol");
        }
    }

    private void GoLeft()
    {
        rigidbody2D.velocity = new Vector2(-cachedActualSpeed, rigidbody2D.velocity.y);
    }

    private void GoRight()
    {
        rigidbody2D.velocity = new Vector2(cachedActualSpeed, rigidbody2D.velocity.y);
    }
}
