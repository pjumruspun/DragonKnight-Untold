using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Visible stats
    Stats stats;

    // Hidden stats
    private float critDamage = 1.5f;
    private float healthRegen = 1.0f;

    public PlayerStats()
    {
        stats = new Stats(0, 0, 0, 0, 0);
    }

    // Assign but not create
    public void AssignStats(Stats stats)
    {
        this.stats = stats;
    }

    // Static version because PlayerHealth needs it
    public static float CalculateMaxHealth(float baseHealth, int inputVit)
    {
        // Health +5% per vit
        return (1 + inputVit * 0.05f) * baseHealth;
    }

    public void CalculateDamage(float baseDamage, out float finalDamage, out bool crit, bool canCrit = true)
    {
        float random = Random.Range(0.0f, 1.0f);

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
}
