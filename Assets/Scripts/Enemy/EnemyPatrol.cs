using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyBehavior
{
    private float distanceToGround = 0.5f;
    private float patrolingSpeed = 1.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Patrol();
    }

    private void Patrol()
    {
        if (!enemy.ShouldChase)
        {
            // Patrol
            if (!IsGrounded())
            {
                // If is going to fall
                // turn around
                float x = transform.localScale.x;
                if (x > 0.01f)
                {
                    // If walking right, go left
                    rigidbody2D.velocity = new Vector2(-patrolingSpeed, rigidbody2D.velocity.y);
                }
                else if (x < -0.01f)
                {
                    // If walking left, go right
                    rigidbody2D.velocity = new Vector2(patrolingSpeed, rigidbody2D.velocity.y);
                }
            }
            else
            {
                float x = transform.localScale.x;
                if (x > 0.01f)
                {
                    rigidbody2D.velocity = new Vector2(patrolingSpeed, rigidbody2D.velocity.y);
                }
                else if (x < -0.01f)
                {
                    rigidbody2D.velocity = new Vector2(-patrolingSpeed, rigidbody2D.velocity.y);
                }
            }
        }
        else
        {
            // Switch state
            animator.SetTrigger("Chase");
        }
    }

    private bool IsGrounded()
    {
        Vector2 position = enemy.GroundDetector.position;
        Vector2 direction = Vector2.down;
        float distance = distanceToGround;
        Debug.DrawRay(position, direction * distance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, Layers.GroundLayer);
        return hit.collider != null;
    }
}
