using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunnedBehavior : EnemyBehavior
{
    [SerializeField]
    private bool shouldRecoverAfterTime = false;

    [SerializeField]
    private float timeToRecover = 0.0f;

    private bool isStunned = false;
    private float cachedGravityScale;
    private float currentNotMoveYDuration;
    private float velocityY;
    private float timeWhenStunned;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (!enemy.IsDead)
        {
            enemy.ShowStunStars(true);
        }

        currentNotMoveYDuration = 0.0f;

        // Make flying enemies fall down
        // Not 1.0f to let the player perform air combo easily
        cachedGravityScale = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0.5f;


        // Set current vx to zero
        velocityY = rigidbody2D.velocity.y;
        rigidbody2D.velocity = new Vector2(0.0f, velocityY);

        // Add force to simulate knock airborne
        rigidbody2D.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);

        // To check if the enemy is stunned
        isStunned = true;

        // Tell that enemy is currently in the air
        enemy.IsKnockedAirborne = true;

        // Mark the time that enemy has been stunned
        timeWhenStunned = Time.time;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        velocityY = rigidbody2D.velocity.y;
        Stunned();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        isStunned = false;
        enemy.ShowStunStars(false);
        enemy.RestoreSuperArmor();
        rigidbody2D.gravityScale = cachedGravityScale;
    }

    public void KnockedUp(float amplitude)
    {
        if (isStunned && Mathf.Abs(velocityY) > 0.01f && !enemy.IsDead)
        {
            // Debug.Log("It works!");
            // Disable vy
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
            // Then add force
            rigidbody2D.AddForce(amplitude * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void Stunned()
    {
        if (enemy.IsDead)
        {
            animator.SetBool("Stunned", false);
            animator.SetTrigger("Dead");
        }

        if (Mathf.Abs(velocityY) < 0.001f)
        {
            currentNotMoveYDuration += Time.deltaTime;
        }
        else
        {
            currentNotMoveYDuration = 0.0f;
        }

        if (currentNotMoveYDuration > 0.1f)
        {
            // On ground for sure
            enemy.IsKnockedAirborne = false;
        }

        if (ShouldGetUp())
        {
            // Can now get up
            animator.SetBool("Stunned", false);
        }
    }

    private bool ShouldGetUp()
    {
        bool naturalGetUp = currentNotMoveYDuration > enemy.SecondsBeforeGetUp;

        float currentStunnedDuration = Time.time - timeWhenStunned;
        bool getUpByTime = shouldRecoverAfterTime && currentStunnedDuration > timeToRecover;

        return naturalGetUp || getUpByTime;
    }
}
