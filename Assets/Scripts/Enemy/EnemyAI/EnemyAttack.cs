using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State for enemy to executing their attacks
/// </summary>
public class EnemyAttack : EnemyBehavior
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemy.CurrentCooldown = enemy.AttackCooldown;
        ProcessAttack();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ListenToChaseSignal();
    }

    protected virtual void ProcessAttack()
    {
        CoroutineUtility.ExecDelay(() => Attack(), enemy.AttackDelay);
    }

    protected virtual void Attack()
    {
        if (enemy.IsRanged)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack(enemy.EnemyAttackHitbox);
        }
    }

    protected virtual void MeleeAttack(AttackHitbox hitbox, float knockBackAmplitude = 0.0f, float knockUpAmplitude = 0.0f)
    {
        HashSet<Collider2D> colliders = hitbox.HitColliders;
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                player.TakeDamage(enemy.AttackDamage);
            }

            bool shouldKnock = knockBackAmplitude > 0.0f || knockUpAmplitude > 0.0f;

            if (shouldKnock && collider.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
            {
                Vector2 fx = enemy.ForwardVector * knockBackAmplitude;
                Vector2 fy = Vector2.up * knockUpAmplitude;
                movement.KnockBack(fx + fy);
            }
        }
    }

    private void RangedAttack()
    {
        GameObject spawnedObject = enemy.Projectile.SpawnObject(transform.position, Quaternion.identity);

        if (spawnedObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.SetDirection(enemy.GetForwardVector());
            projectile.SetDamage(enemy.AttackDamage, crit: false);
        }
        else
        {
            throw new System.InvalidOperationException();
        }
    }
}
