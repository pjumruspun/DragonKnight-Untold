using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy, IMelee
{
    public AttackHitbox EnemyAttackHitbox => attackHitbox;

    [Header("Melee")]
    [SerializeField]
    private AttackHitbox attackHitbox;
}
