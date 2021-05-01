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
        if (enemy is RangedEnemy)
        {
            RangedAttack(((RangedEnemy)enemy).Projectile);
        }
        else if (enemy is MeleeEnemy)
        {
            MeleeAttack(((MeleeEnemy)enemy).EnemyAttackHitbox);
        }
    }

    protected virtual float MeleeAttack(AttackHitbox hitbox, float knockBackAmplitude = 0.0f, float knockUpAmplitude = 0.0f, float overrideDamage = 0.0f)
    {
        float totalDamage = 0.0f;
        HashSet<Collider2D> colliders = hitbox.HitColliders;
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                float damageDealt;
                if (overrideDamage > 0.0f)
                {
                    damageDealt = player.TakeDamage(overrideDamage);
                }
                else
                {
                    damageDealt = player.TakeDamage(enemy.AttackDamage);
                }

                totalDamage += damageDealt;
            }

            bool shouldKnock = (knockBackAmplitude > 0.0f || knockUpAmplitude > 0.0f) && totalDamage > 0.01f;
            if (shouldKnock && collider.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
            {
                Vector2 fx = enemy.ForwardVector * knockBackAmplitude;
                Vector2 fy = Vector2.up * knockUpAmplitude;
                movement.KnockBack(fx + fy);
            }
        }

        return totalDamage;
    }

    private void RangedAttack(ObjectPool projectilePool)
    {
        GameObject spawnedObject = projectilePool.SpawnObject(transform.position, Quaternion.identity);

        if (spawnedObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            Vector2 direction;
            if (enemy.IsFlyingEnemy)
            {
                direction = player.position - transform.position;
            }
            else
            {
                direction = enemy.GetForwardVector();
            }

            projectile.SetConfig(new ProjectileConfig(direction, enemy.AttackDamage));
        }
        else
        {
            throw new System.InvalidOperationException();
        }
    }
}
