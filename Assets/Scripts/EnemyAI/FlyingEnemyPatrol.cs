using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyPatrol : EnemyBehavior
{
    private float patrolingSpeed = 1.0f;
    private float patrolingRadius = 4.0f;
    private Vector2 initialPosition;
    private Vector2 destination;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        initialPosition = transform.position;
        destination = GetNextDestination();
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
            // Will wander around initial position
            if (HasReachedDestination())
            {
                // Pick new destination
                destination = GetNextDestination();
            }
            else
            {
                // Should move according to the patroling direction
                Vector2 position = transform.position;
                Debug.DrawLine(position, destination, Color.blue);
                rigidbody2D.velocity = (destination - position).normalized * patrolingSpeed;
            }
        }
        else
        {
            // Chase
            animator.SetTrigger("Chase");
        }
    }

    private bool HasReachedDestination()
    {
        Vector2 position = transform.position;
        return Vector2.Distance(destination, position) < 0.01f;
    }

    private Vector2 GetNextDestination()
    {
        return initialPosition + GetRandomUnitVector() * patrolingRadius;
    }

    private Vector2 GetRandomUnitVector()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        return new Vector2(x, y).normalized;
    }
}
