using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerChase : EnemyChase
{
    [Header("Attack Probability")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float punchingProbability;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float stompingProbability;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float jumpStompProbability;

    [Header("Attack Range")]
    [SerializeField]
    private float punchingRange = 2.0f;
    [SerializeField]
    private float stompingRange = 2.0f;
    [SerializeField]
    private float jumpStompRange = 6.0f;

    [Header("Threshold to Start Jump Stomp")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float jumpStompEnableThreshold = 0.66f;

    [Header("Jumping Parameters")]
    [SerializeField]
    private float maxJumpXPower = 5.0f;
    [SerializeField]
    private float jumpDelayTime = 0.3f;

    [Header("Threshold to gain immense action speed")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float gainSpeedThreshold = 0.33f;

    [Header("Action speed to gain after threshold")]
    [SerializeField]
    private float actionSpeedToGain = 1.3f;

    private const float jumpYPower = 3.0f;

    private bool jumpStompEnabled => (enemy.CurrentHealth / enemy.MaxHealth) <= jumpStompEnableThreshold;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ListenToSpeedGainSignal();
    }

    protected override void ListenToAttackSignal()
    {
        bool readyToAttack = enemy.CurrentCooldown < 0.01f;
        if (readyToAttack)
        {
            float softmaxAmplitude = punchingProbability + stompingProbability;
            if (jumpStompEnabled)
            {
                softmaxAmplitude += jumpStompProbability;
            }

            float random = Random.Range(0.0f, softmaxAmplitude);

            if (random <= punchingProbability && DistanceToPlayerX <= punchingRange)
            {
                animator.SetTrigger("Attack");
            }
            else if (random <= punchingProbability + stompingProbability && DistanceToPlayerX <= stompingRange)
            {
                animator.SetTrigger("Stomp");
            }
            else if (jumpStompEnabled && random <= softmaxAmplitude && DistanceToPlayerX <= jumpStompRange)
            {
                CoroutineUtility.ExecDelay(() => JumpBeforeAttack(), jumpDelayTime / enemy.ActionSpeed);
                animator.SetTrigger("JumpStomp");
            }
        }
    }

    private void JumpBeforeAttack()
    {
        float x = enemy.ForwardVector.x * Mathf.Min(maxJumpXPower, DistanceToPlayerX);
        rigidbody2D.AddForce(new Vector2(x, jumpYPower), ForceMode2D.Impulse);
    }

    private void ListenToSpeedGainSignal()
    {
        float enemyHpPercentage = enemy.CurrentHealth / enemy.MaxHealth;
        if (enemyHpPercentage <= gainSpeedThreshold)
        {
            enemy.ActionSpeed = actionSpeedToGain;
            Debug.Log(animator.speed);
            Debug.Log(enemy.AttackCooldown);
        }
    }
}
