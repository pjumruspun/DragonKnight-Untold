using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will handle both HP bar update and health update
public class EnemyHealth : Health
{
    [SerializeField]
    private float enemyMaxHealth = 150.0f;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        // Show floating damage number

        HandleHealthChange();
    }

    protected override void Start()
    {
        maxHealth = enemyMaxHealth;
        base.Start();
    }

    protected override void HandleHealthChange()
    {
        // Update HP Bar
    }

    protected override void HandleDeath()
    {
        // Destroy object in x seconds?
    }
}
