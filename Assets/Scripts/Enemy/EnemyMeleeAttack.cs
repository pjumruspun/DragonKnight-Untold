using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyBehavior
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemy.CurrentCooldown = enemy.AttackCooldown;
        CoroutineUtility.Instance.StartCoroutine(AttackWithDelay(enemy.AttackDelay));
    }

    private IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack();
    }

    private void Attack()
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
}
