using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Health : MonoBehaviour
{
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0.01f;

    protected float maxHealth = 100;
    protected float currentHealth;
    protected Collider2D collider2D;

    public virtual void TakeDamage(float damage)
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
    }

    protected abstract void HandleHealthChange();

    protected abstract void HandleDeath();

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        HandleHealthChange();
    }
}
