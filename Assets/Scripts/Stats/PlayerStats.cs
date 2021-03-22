using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    public float AttackSpeed => attackSpeed.GetValue;
    public float MovementSpeed => (1.0f + (Mathf.Max(agi.GetValue, minAgiPossible) * 0.03f)) * baseMovementSpeed.GetValue;
    public float MaxHealth => (1.0f + (Mathf.Max(vit.GetValue, minVitPossible) * 0.05f)) * basePlayerMaxHealth;
    public float[] SkillCooldown => CalculateSkillCooldown();
    public IReadOnlyList<float> BaseSkillDamage => baseSkillDamage;

    [Header("Main Stats")]
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

    [Header("Additional Stats")]
    [SerializeField]
    private Stats<float> critDamage = new Stats<float>(1.5f);
    [SerializeField]
    private Stats<float> baseMovementSpeed = new Stats<float>(3.0f);
    [SerializeField]
    private Stats<float> healthRegen = new Stats<float>(1.0f);
    [SerializeField]
    private Stats<float> attackSpeed = new Stats<float>(1.0f); // Only affects skill 1, auto attack
    [SerializeField]
    private Stats<float> cooldownReduction = new Stats<float>(1.0f); // Only affects skill 2, 3, and 4
    private float basePlayerMaxHealth = 200;
    private float[] baseSkillCooldown = new float[4];
    private float[] baseSkillDamage = new float[4];

    public void CalculateDamage(float baseDamage, out float finalDamage, out bool crit, bool canCrit = true)
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);

        // Damage +3% for each atk
        float damage = baseDamage * (1 + 0.03f * atk.GetValue);

        // Handle crit
        if (random < CalculateCritChance() && canCrit)
        {
            // Crit
            finalDamage = damage * critDamage.GetValue;
            crit = true;
        }
        else
        {
            // Didn't crit
            finalDamage = damage;
            crit = false;
        }
    }

    private const int minAgiPossible = -30;
    private const int minVitPossible = 0;

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

        critDamage.Additive = stats.CritDamageAddModifier;
        critDamage.Multiplicative = stats.CritDamageMultModifier;

        baseMovementSpeed.Additive = stats.MovementSpeedAddModifier;
        baseMovementSpeed.Multiplicative = stats.MovementSpeedMultModifier;

        healthRegen.Additive = stats.HealthRegenAddModifier;
        healthRegen.Multiplicative = stats.HealthRegenMultModifier;

        attackSpeed.Additive = stats.AttackSpeedAddModifier;
        attackSpeed.Multiplicative = stats.AttackSpeedMultModifier;

        cooldownReduction.Additive = stats.CooldownReductionAddModifier;
        cooldownReduction.Multiplicative = stats.CooldownReductionMultModifier;

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
        atk = new Stats<int>(10);
        agi = new Stats<int>(10);
        vit = new Stats<int>(10);
        tal = new Stats<int>(10);
        luk = new Stats<int>(10);

        // Events
        EventPublisher.PlayerChangeClass += AdjustSkillParams;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerChangeClass -= AdjustSkillParams;
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
        results[0] /= attackSpeed.GetValue;
        return results;
    }

    private void AdjustSkillParams(PlayerClass playerClass)
    {
        PlayerSkills skills;
        switch (playerClass)
        {
            case PlayerClass.Sword:
                skills = SkillsRepository.Sword;
                break;
            case PlayerClass.Archer:
                skills = SkillsRepository.Archer;
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }

        baseSkillCooldown = skills.GetBaseSkillCooldowns.Cast<float>().ToArray();
        baseSkillDamage = skills.GetBaseSkillDamage.Cast<float>().ToArray();
    }
}
