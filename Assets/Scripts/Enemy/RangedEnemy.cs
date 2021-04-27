using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy, IRanged
{
    public ObjectPool Projectile => projectilePool;

    [Header("Ranged")]
    [SerializeField]
    protected GameObject projectilePrefab;
    protected ObjectPool projectilePool;

    protected override void EnableEnemy()
    {
        base.EnableEnemy();
        if (projectilePrefab != null)
        {
            projectilePool = new ObjectPool(projectilePrefab, 1);
        }
        else
        {
            throw new System.NullReferenceException("Projectile prefab not assigned");
        }
    }
}
