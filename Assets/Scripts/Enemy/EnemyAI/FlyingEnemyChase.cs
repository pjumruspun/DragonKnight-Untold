using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, player.position, OnPathComplete);
        }

        // Debug.Log("Updated path");
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
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        Vector2 moveVector = direction * cachedActualSpeed;
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            // Reach
            ++currentWaypoint;
        }

        rigidbody2D.velocity = moveVector;
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
