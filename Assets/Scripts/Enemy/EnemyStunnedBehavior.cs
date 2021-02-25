using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunnedBehavior : EnemyBehavior
{
    private float cachedGravityScale;
    private float currentNotMoveYDuration;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemy.ShowStunStars(true);
        cachedGravityScale = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 1.0f;
        currentNotMoveYDuration = 0.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Stunned();
    }

    private void Stunned()
    {
        float velocityY = rigidbody2D.velocity.y;
        rigidbody2D.velocity = new Vector2(0.0f, velocityY);
        if (Mathf.Abs(velocityY) < 0.01f)
        {
            currentNotMoveYDuration += Time.deltaTime;
        }
        else
        {
            currentNotMoveYDuration = 0.0f;
        }

        if (currentNotMoveYDuration > enemy.SecondsBeforeGetUp)
        {
            // Can now get up
            animator.SetBool("Stunned", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        enemy.ShowStunStars(false);
        enemy.RestoreSuperArmor();
        rigidbody2D.gravityScale = cachedGravityScale;
    }
}
