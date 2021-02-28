using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Stats
{
    // For easy referencing
    public int atk;
    public int agi;
    public int vit;
    public int tal;
    public int luk;

    public Stats(int atk, int agi, int vit, int tal, int luk)
    {
        this.atk = atk;
        this.agi = agi;
        this.vit = vit;
        this.tal = tal;
        this.luk = luk;
    }
}

public class PlayerStats
{
    public float[] SkillCooldown => CalculateSkillCooldown();
    public IReadOnlyList<float> BaseSkillDamage => baseSkillDamage;

    // Visible stats
    private Stats stats;

    // Hidden stats
    private float critDamage = 1.5f;
    private float healthRegen = 1.0f;
    private float[] baseSkillCooldown = new float[4];
    private float[] baseSkillDamage = new float[4];
    private float attackSpeed = 1.0f; // Only affects skill 1, auto attack

    private PlayerStats() { }

    // For creating stats based on class
    public static PlayerStats Create(PlayerClass playerClass)
    {
        PlayerConfig playerConfig = ConfigContainer.Instance.GetPlayerConfig;
        ClassConfig config;
        switch (playerClass)
        {
            case PlayerClass.Sword:
                config = playerConfig.SwordConfig;
                break;
            case PlayerClass.Archer:
                config = playerConfig.ArcherConfig;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        float[] cooldowns = new float[4];
        float[] skillDamage = new float[4];
        config.skillCooldown.CopyTo(cooldowns, 0);
        config.skillDamage.CopyTo(skillDamage, 0);
        Stats stats = new Stats(config.atk, config.agi, config.vit, config.tal, config.luk);

        PlayerStats playerStats = new PlayerStats();

        playerStats.baseSkillCooldown = cooldowns;
        playerStats.baseSkillDamage = skillDamage;
        playerStats.stats = stats;

        return playerStats;
    }

    // Assign but not create
    public void AssignStats(Stats stats)
    {
        this.stats = stats;

        // Notify stats changed
        EventPublisher.TriggerPlayerStatsChange(stats);

        // Health might update
        EventPublisher.TriggerPlayerHealthChange();
    }

    public void AssignSkillCooldown(float[] cooldown)
    {
        for (int i = 0; i < baseSkillCooldown.Length; ++i)
        {
            baseSkillCooldown[i] = cooldown[i];
        }
    }

    // Static version because PlayerMovement needs it
    public static float CalculateMovementSpeed(float baseMoveSpeed, int inputAgi)
    {
        // Speed +3% per agi
        return (1 + inputAgi * 0.03f) * baseMoveSpeed;
    }

    // Static version because PlayerHealth needs it
    public static float CalculateMaxHealth(float baseHealth, int inputVit)
    {
        // Health +5% per vit
        return (1 + inputVit * 0.05f) * baseHealth;
    }

    public void CalculateDamage(float baseDamage, out float finalDamage, out bool crit, bool canCrit = true)
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);

        // Damage +3% for each atk
        float damage = baseDamage * (1 + 0.03f * stats.atk);

        // Handle crit
        if (random < CalculateCritChance() && canCrit)
        {
            // Crit
            finalDamage = damage * critDamage;
            crit = true;
        }
        else
        {
            // Didn't crit
            finalDamage = damage;
            crit = false;
        }
    }

    public override string ToString()
    {
        return $"ATK: {stats.atk}\t AGI: {stats.agi}\t VIT: {stats.vit}\t TAL: {stats.tal}\t LUK: {stats.luk}";
    }

    private float CalculateCritChance()
    {
        // Simple 2% crit chance per luk for now
        return stats.luk * 0.02f;
    }

    // This is very expensive as it's currently calculating every frame
    // This could be cached into a variable and only calculate when the cooldown has changed
    // Can do it later when the game is lagging
    private float[] CalculateSkillCooldown()
    {
        // If player ever has cooldown reduction items

        float[] results = new float[4];
        for (int i = 0; i < 4; ++i)
        {
            results[i] = baseSkillCooldown[i];
        }

        // Only skill 1 is affected by attack speed
        results[0] /= attackSpeed;
        return baseSkillCooldown;
    }
}
