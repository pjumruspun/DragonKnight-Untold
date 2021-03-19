using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerChase : EnemyChase
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float punchingProbability;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float stompingProbability;

    protected override void ListenToAttackSignal()
    {
        bool readyToAttack = enemy.CurrentCooldown < 0.01f;
        if (DistanceToPlayerX <= enemy.AttackRange && readyToAttack)
        {
            float random = Random.Range(0.0f, punchingProbability + stompingProbability);
            if (random <= punchingProbability)
            {
                animator.SetTrigger("Attack");
            }
            else if (random <= punchingProbability + stompingProbability)
            {
                animator.SetTrigger("Stomp");
            }
            else
            {
                Debug.LogWarning($"{enemy.name} is stuck in ChasingState");
            }
        }
    }
}
