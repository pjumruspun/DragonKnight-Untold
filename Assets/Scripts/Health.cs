using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Health : MonoBehaviour, IHealth
{
    public int MaxHealth => maxHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth -= value;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            // Trigger player death here
        }
    }

    private int maxHealth = 100;
    private int currentHealth;
    private BoxCollider2D collider2D;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }
}
