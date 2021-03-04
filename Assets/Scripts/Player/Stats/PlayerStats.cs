using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    public float MovementSpeed => (1.0f + (agi.GetValue * 0.03f)) * baseMovementSpeed;
    public float MaxHealth => (1.0f + (vit.GetValue * 0.05f)) * basePlayerMaxHealth;
    public float[] SkillCooldown => CalculateSkillCooldown();
    public IReadOnlyList<float> BaseSkillDamage => baseSkillDamage;

    // Visible stats
    [SerializeField]
    private Stats<int> atk;
    [SerializeField]
    private Stats<int> agi;
    [SerializeField]
    private Stats<int> vit;
    [SerializeField]
    private Stats<int> tal;
    [SerializeField]
    private Stats<int> luk;

    // Other stats
    private float critDamage = 1.5f;
    private float basePlayerMaxHealth = 200;
    private float baseMovementSpeed = 3.0f;
    private float healthRegen = 1.0f;
    private float[] baseSkillCooldown = new float[4]
    {
        0.5f,
        3.0f,
        1.0f,
        1.0f
    };
    private float[] baseSkillDamage = new float[4]
    {
        20.0f,
        30.0f,
        20.0f,
        20.0f
    };
    private float attackSpeed = 1.0f; // Only affects skill 1, auto attack

    public void CalculateDamage(float baseDamage, out float finalDamage, out bool crit, bool canCrit = true)
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);

        // Damage +3% for each atk
        float damage = baseDamage * (1 + 0.03f * atk.GetValue);

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

    /// <summary>
    /// Adapter method, for assigning the whole stats for player
    /// from the parameter "stats"
    /// </summary>
    /// <param name="stats"></param>
    public void AssignStatsFromItems(ItemStats stats)
    {
        atk.Additive = stats.AtkAddModifier;
        atk.Multiplicative = stats.AtkMultModifier;

        agi.Additive = stats.AgiAddModifier;
        agi.Multiplicative = stats.AgiMultModifier;

        vit.Additive = stats.VitAddModifier;
        vit.Multiplicative = stats.VitMultModifier;

        tal.Additive = stats.TalAddModifier;
        tal.Multiplicative = stats.TalMultModifier;

        luk.Additive = stats.LukAddModifier;
        luk.Multiplicative = stats.LukMultModifier;

        EventPublisher.TriggerPlayerStatsChange();
    }

    public override string ToString()
    {
        return $"ATK: {atk.GetValue}\t AGI: {agi.GetValue}\t VIT: {vit.GetValue}\t TAL: {tal.GetValue}\t LUK: {luk.GetValue}";
    }

    protected override void Awake()
    {
        base.Awake();

        // Set stats, should random
        atk = new Stats<int>(30);
        agi = new Stats<int>(30);
        vit = new Stats<int>(30);
        tal = new Stats<int>(30);
        luk = new Stats<int>(30);
    }

    private float CalculateCritChance()
    {
        // Simple 2% crit chance per luk for now
        return luk.GetValue * 0.02f;
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
