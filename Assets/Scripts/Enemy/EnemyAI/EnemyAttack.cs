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
        CoroutineUtility.Instance.StartCoroutine(AttackWithDelay(enemy.AttackDelay));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ListenToChaseSignal();
    }

    private IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemy.IsRanged)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        AttackHitbox hitbox = enemy.EnemyAttackHitbox;
        HashSet<Collider2D> colliders = hitbox.HitColliders;
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                player.TakeDamage(enemy.AttackDamage);
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
