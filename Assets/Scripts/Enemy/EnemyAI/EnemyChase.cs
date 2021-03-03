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
            // else if (enemy.IsRanged)
            // {
            //     // In between, only for ranged enemies
            //     // Too close when on the left, or right side
            //     bool tooCloseLeft = transform.position.x > player.position.x - enemy.AttackRange + spaceOffset;
            //     bool tooCloseRight = transform.position.x < player.position.x + enemy.AttackRange - spaceOffset;
            //     if (tooCloseLeft)
            //     {
            //         // Should walk left until reached preferable attack range
            //         GoLeft();
            //     }
            //     else if (tooCloseRight)
            //     {
            //         // Should walk right until reached preferable attack range
            //         // Go right
            //         GoRight();
            //     }
            // }
        }
        else
        {
            // Switch state
            animator.SetTrigger("Patrol");
        }

        ListenToAttackSignal();
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
