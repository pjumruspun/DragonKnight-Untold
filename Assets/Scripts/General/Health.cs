using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Health : MonoBehaviour
{
    public float HealthPercentage => currentHealth / maxHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0.01f;

    protected float maxHealth = 100;
    protected float currentHealth;
#pragma warning disable 0108
    protected Collider2D collider2D;

    public virtual float TakeDamage(float damage)
    {
        if (!IsDead)
        {
            // Returns health after taking damage
            currentHealth -= damage;

            // If this is the last hit
            if (IsDead)
            {
                currentHealth = 0.0f;
                HandleDeath();
            }

            HandleHealthChange();
        }

        return damage;
    }

    protected abstract void HandleHealthChange();

    protected abstract void HandleDeath();

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        HandleHealthChange();
    }
}
