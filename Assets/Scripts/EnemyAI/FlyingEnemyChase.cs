using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

#pragma warning disable 0108
#pragma warning disable 0414
public class FlyingEnemyChase : EnemyBehavior
{
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 3.0f;
    private bool reached;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        seeker = animator.GetComponent<Seeker>();
        lookForPlayer = UpdatePath;
        base.OnStateEnter(animator, stateInfo, layerIndex);
        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ProcessingPath();
        ListenToAttackSignal();
    }

    protected override void ListenToAttackSignal()
    {
        bool readyToAttack = enemy.CurrentCooldown < 0.01f;
        if (DistanceToPlayer <= enemy.AttackRange && readyToAttack)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, player.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    private void Chase()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody2D.position).normalized;
        Vector2 moveVector = direction * cachedActualSpeed * 4.0f;
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            // Reach
            ++currentWaypoint;
        }

        rigidbody2D.AddForce(moveVector);
    }

    private void ProcessingPath()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reached = true;
            return;
        }
        else
        {
            reached = false;
        }

        Chase();
    }

    // private void Chase()
    // {
    //     if (enemy.ShouldChase)
    //     {
    //         Vector2 chaseDirection = (player.position - transform.position).normalized;
    //         Vector2 chaseVector = chaseDirection * cachedActualSpeed;
    //         rigidbody2D.velocity = chaseVector;
    //     }
    //     else
    //     {
    //         // Switch state
    //         animator.SetTrigger("Patrol");
    //     }
    // }
}
