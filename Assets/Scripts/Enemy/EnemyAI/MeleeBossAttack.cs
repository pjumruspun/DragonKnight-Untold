using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossAttack : EnemyAttack
{
    [Tooltip("GameObject name that contains AttackHitbox for this attack to work")]
    [SerializeField]
    private string attackHitboxName;
    [SerializeField]
    private float knockBackAmplitude;
    [SerializeField]
    private float knockUpAmplitude;
    [SerializeField]
    private float attackDelay; // Need to adjust to animation later
    private AttackHitbox attackHitbox;

    protected override void ProcessAttack()
    {

        CoroutineUtility.ExecDelay(() => Attack(), attackDelay);
    }

    protected override void Attack()
    {
        if (attackHitbox == null)
        {
            FindAttackHitboxByName();
        }

        MeleeAttack(attackHitbox, knockBackAmplitude, knockUpAmplitude);
    }

    private void FindAttackHitboxByName()
    {
        Transform attackHitboxTransform = enemy.transform.Find(attackHitboxName);
        if (attackHitboxTransform.TryGetComponent<AttackHitbox>(out AttackHitbox attackHitbox))
        {
            this.attackHitbox = attackHitbox;
        }
        else
        {
            throw new System.Exception($"{enemy.name}: Could not get AttackHitbox component from child object with name {attackHitboxName}");
        }
    }
}
