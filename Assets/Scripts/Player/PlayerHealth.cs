using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    // Can't multiple inherit so we need to handmade a singleton here
    public static PlayerHealth Instance { get; private set; }

    [SerializeField]
    private BoxCollider2D playerCollider;

    override protected void Start()
    {
        base.Start();
        collider2D = GetComponent<BoxCollider2D>();
        if (playerCollider != null)
        {
            collider2D = playerCollider;
        }
    }

    protected override void HandleHealthChange()
    {
        base.HandleHealthChange();
        EventPublisher.TriggerPlayerHealthChange();
    }

    override protected void HandleDeath()
    {
        base.HandleDeath();
        // Trigger player death
        EventPublisher.TriggerPlayerDead();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            float health = TakeDamage(24.0f);
        }
    }
}
