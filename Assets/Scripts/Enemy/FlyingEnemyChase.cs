using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyChase : EnemyBehavior
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
            Vector2 chaseDirection = (player.position - transform.position).normalized;
            Vector2 chaseVector = chaseDirection * cachedActualSpeed;
            rigidbody2D.velocity = chaseVector;
        }
        else
        {
            // Switch state
            animator.SetTrigger("Patrol");
        }
    }
}
