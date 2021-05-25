using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStats : MonoSingleton<PlayerStats>
{
    public int ATK => atk.GetValue;
    public int AGI => agi.GetValue;
    public int VIT => vit.GetValue;
    public int TAL => tal.GetValue;
    public int LUK => luk.GetValue;
    public float CritDamage => critDamage.GetValue;
    public float HealthRegen => healthRegen.GetValue;
    public float CooldownReduction => cooldownReduction.GetValue;
    public float AttackSpeed => attackSpeed.GetValue;
    public float DamageMultiplier => (1 + 0.02f * atk.GetValue);
    public float MovementSpeed => (1.0f + (Mathf.Max(AGI, minAgiPossible) * 0.025f)) * baseMovementSpeed.GetValue;
    public float MovementSpeedRatio => MovementSpeed / baseMoveSpeed;
    public float MaxHealth => (1.0f + (Mathf.Max(VIT, minVitPossible) * 0.03f)) * basePlayerMaxHealth;
    public float MaxDragonEnergyMultiplier => 1.0f + 0.02f * TAL;
    public float DragonEnergyDrainMultiplier => 1.0f / (1.0f + 0.02f * TAL);
    public float DragonDamageMultiplier => 1.0f + 0.02f * TAL;

    /// <summary>
    /// Actual value of skill cooldowns after cooldown reduction
    /// from items and stats.
    /// </summary>
    /// <returns>Array of float with length = 4</returns>
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
    private Stats<float> baseMovementSpeed = new Stats<float>(baseMoveSpeed);
    [SerializeField]
    private Stats<float> healthRegen = new Stats<float>(1.0f);
    [SerializeField]
    private Stats<float> attackSpeed = new Stats<float>(1.0f); // Only affects skill 1, auto attack
    [SerializeField]
    private Stats<float> cooldownReduction = new Stats<float>(0.0f); // Only affects skill 2, 3, and 4
    private float basePlayerMaxHealth = 200;
    private float[] baseSkillCooldown = new float[4];
    private float[] baseSkillDamage = new float[4];
    private float actualCooldownRatio => 1 / (1 + cooldownReduction.GetValue);
    private const float baseMoveSpeed = 4.0f;

    public void CalculateDamage(float baseDamage, out float finalDamage, out bool crit, bool canCrit = true)
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);

        // Damage +3% for each atk
        float damage = baseDamage * DamageMultiplier;

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
    public void AssignStatsFromItems(StatsDto stats)
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

        if (PlayerStatsStatic.shouldRandom)
        {
            RandomStat();
        }
        else
        {
            AssignStatsFromStatic();
        }

        // Events
        EventPublisher.PlayerChangeClass += AdjustSkillParams;
        EventPublisher.PlayerDead += ShouldResetStat;
    }

    private void RandomStat()
    {
        System.Random rnd = new System.Random();
        float sumStatFactor = (float)rnd.NextDouble();
        float sumStat = 50f * Mathf.Min(0.75f + sumStatFactor, 1.5f);

        float atkFactor = (float)rnd.NextDouble();
        float agiFactor = (float)rnd.NextDouble();
        float vitFactor = (float)rnd.NextDouble();
        float talFactor = (float)rnd.NextDouble();
        float lukFactor = (float)rnd.NextDouble();

        atkFactor += 0.1f;

        float sumFactor = atkFactor + agiFactor + vitFactor + talFactor + lukFactor;

        atk = new Stats<int>(Mathf.CeilToInt(atkFactor / sumFactor * sumStat));
        agi = new Stats<int>(Mathf.CeilToInt(agiFactor / sumFactor * sumStat));
        vit = new Stats<int>(Mathf.CeilToInt(vitFactor / sumFactor * sumStat));
        tal = new Stats<int>(Mathf.CeilToInt(talFactor / sumFactor * sumStat));
        luk = new Stats<int>(Mathf.CeilToInt(lukFactor / sumFactor * sumStat));

        PlayerStatsStatic.atk = atk.GetValue;
        PlayerStatsStatic.agi = agi.GetValue;
        PlayerStatsStatic.vit = vit.GetValue;
        PlayerStatsStatic.tal = tal.GetValue;
        PlayerStatsStatic.luk = luk.GetValue;

        PlayerStatsStatic.shouldRandom = false;
    }

    private void AssignStatsFromStatic()
    {
        atk = new Stats<int>(PlayerStatsStatic.atk);
        agi = new Stats<int>(PlayerStatsStatic.agi);
        vit = new Stats<int>(PlayerStatsStatic.vit);
        tal = new Stats<int>(PlayerStatsStatic.tal);
        luk = new Stats<int>(PlayerStatsStatic.luk);
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerChangeClass -= AdjustSkillParams;
        EventPublisher.PlayerDead -= ShouldResetStat;
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
            if (i == 0)
            {
                // Only skill 1 is affected by attack speed
                results[i] /= attackSpeed.GetValue;
            }
            else
            {
                // Skill 1 2 3 affected by cooldown reduction
                results[i] *= actualCooldownRatio;
            }
        }

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

    private void ShouldResetStat()
    {
        PlayerStatsStatic.shouldRandom = true;
    }
}
