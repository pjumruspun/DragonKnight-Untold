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
        maxHealth = ConfigContainer.Instance.GetPlayerConfig.MaxHealth;
        base.Start();
        collider2D = GetComponent<BoxCollider2D>();
        if (playerCollider != null)
        {
            collider2D = playerCollider;
        }

        EventPublisher.PlayerStatsChange += AdjustMaxHealth;
    }

    protected override void HandleHealthChange()
    {
        EventPublisher.TriggerPlayerHealthChange();
    }

    override protected void HandleDeath()
    {
        // Trigger player death
        EventPublisher.TriggerPlayerDead();
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerStatsChange -= AdjustMaxHealth;
    }

    private void AdjustMaxHealth(Stats stats)
    {
        float finalMaxHealth = PlayerStats.CalculateMaxHealth(ConfigContainer.Instance.GetPlayerConfig.MaxHealth, stats.vit);
        if (finalMaxHealth > maxHealth)
        {
            // Increase current health with the same amount of max health increased
            float maxHealthIncreased = finalMaxHealth - maxHealth;
            currentHealth += maxHealthIncreased;
        }

        maxHealth = finalMaxHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        EventPublisher.TriggerPlayerHealthChange();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(23.4f);
        }
    }
}
