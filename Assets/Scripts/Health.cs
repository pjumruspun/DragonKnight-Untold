using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Health : MonoBehaviour, IHealth
{
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    protected float maxHealth = 100;
    protected float currentHealth;
    protected Collider2D collider2D;

    public float TakeDamage(float damage)
    {
        if (!IsDead)
        {
            // Returns health after taking damage
            currentHealth -= damage;
            if (currentHealth < 0.0f)
            {
                currentHealth = 0.0f;
                HandleDeath();
            }

            HandleHealthChange();
        }

        return currentHealth;
    }

    protected abstract void HandleHealthChange();

    protected abstract void HandleDeath();

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
}
