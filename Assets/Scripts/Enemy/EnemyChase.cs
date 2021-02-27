using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBehavior
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Chase();
    }

    private void Chase()
    {
        if (enemy.ShouldChase)
        {
            if (transform.position.x < player.position.x)
            {
                // Enemy is on the left side of the player
                // Go right
                rigidbody2D.velocity = new Vector2(cachedActualSpeed, rigidbody2D.velocity.y);
            }
            else if (transform.position.x > player.position.x)
            {
                // Enemy is on the right side of the player
                // Go left
                rigidbody2D.velocity = new Vector2(-cachedActualSpeed, rigidbody2D.velocity.y);
            }
        }
        else
        {
            // Switch state
            animator.SetTrigger("Patrol");
        }
    }
}
