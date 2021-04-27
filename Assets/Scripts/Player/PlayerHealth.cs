using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    // Can't multiple inherit so we need to handmade a singleton here
    public static PlayerHealth Instance { get; private set; }
    public bool HasSuccessfullyBlocked => hasSuccessfullyBlocked;
    [SerializeField]
    private BoxCollider2D playerCollider;

    // Buff stuff
    private bool isInvulnerable = false;
    private bool hasSuccessfullyBlocked = false;

    public void SetInvul(bool invul)
    {
        isInvulnerable = invul;
    }

    public void SetHasBlocked(bool hasBlocked)
    {
        hasSuccessfullyBlocked = hasBlocked;
    }

    public override float TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            hasSuccessfullyBlocked = true;
            // Spawn block effect
            FloatingTextSpawner.Spawn("Blocked!", transform.position);
            return 0.0f;
        }
        else
        {
            base.TakeDamage(damage);
            EventPublisher.TriggerPlayerTakeDamage();
            return damage;
        }
    }

    public float Heal(float healAmount)
    {
        if (!IsDead)
        {
            FloatingTextSpawner.Spawn($"+{healAmount}", transform.position);
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            EventPublisher.TriggerPlayerHealthChange();
        }

        return healAmount;
    }

    override protected void Start()
    {
        maxHealth = PlayerStats.Instance.MaxHealth;
        base.Start();

        // Scene transition
        if (PlayerHealthStatic.currentHealth > 0.0f)
        {
            currentHealth = PlayerHealthStatic.currentHealth;
        }

        Invoke("UpdateMaxHealth", Time.deltaTime);
        collider2D = GetComponent<BoxCollider2D>();
        if (playerCollider != null)
        {
            collider2D = playerCollider;
        }

        EventPublisher.PlayerStatsChange += UpdateMaxHealth;
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
        EventPublisher.PlayerStatsChange -= UpdateMaxHealth;

        // Fetch data if there is scene transition
        PlayerHealthStatic.currentHealth = currentHealth;
    }

    private void UpdateMaxHealth()
    {
        float finalMaxHealth = PlayerStats.Instance.MaxHealth;
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

    private void OnRestartGame()
    {
        // Set to -1.0f so that PlayerHealth doesn't import health from PlayerHealthStatic
        PlayerHealthStatic.currentHealth = -1.0f;
    }
}
