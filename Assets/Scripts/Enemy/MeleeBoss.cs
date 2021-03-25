using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBoss : Boss, IMelee
{
    public AttackHitbox EnemyAttackHitbox => attackHitbox;

    [Header("Melee")]
    [SerializeField]
    private AttackHitbox attackHitbox;
}
