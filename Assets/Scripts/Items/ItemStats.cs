using UnityEngine;

[System.Serializable]
public struct StatsDto
{
    // Main Stats
    [Header("Additive Modifiers")]
    public int AtkAddModifier;
    public int AgiAddModifier;
    public int VitAddModifier;
    public int TalAddModifier;
    public int LukAddModifier;

    [Header("Multiplicative Modifiers")]
    public float AtkMultModifier;
    public float AgiMultModifier;
    public float VitMultModifier;
    public float TalMultModifier;
    public float LukMultModifier;

    // Additional Stats
    [Header("Extra Stats Additive Modifiers")]
    public float CritDamageAddModifier;
    public float MovementSpeedAddModifier;
    public float HealthRegenAddModifier;
    public float AttackSpeedAddModifier;
    public float CooldownReductionAddModifier;

    [Header("Extra Stats Multiplicative Modifiers")]
    public float CritDamageMultModifier;
    public float MovementSpeedMultModifier;
    public float HealthRegenMultModifier;
    public float AttackSpeedMultModifier;
    public float CooldownReductionMultModifier;


    public StatsDto(
        int atkAddModifier,
        int agiAddModifier,
        int vitAddModifier,
        int talAddModifier,
        int lukAddModifier,
        float atkMultModifier,
        float agiMultModifier,
        float vitMultModifier,
        float talMultModifier,
        float lukMultModifier,
        float critDamageAddModifier,
        float movementSpeedAddModifier,
        float healthRegenAddModifier,
        float attackSpeedAddModifier,
        float cooldownReductionAddModifier,
        float critDamageMultModifier,
        float movementSpeedMultModifier,
        float healthRegenMultModifier,
        float attackSpeedMultModifier,
        float cooldownReductionMultModifier
    )
    {
        this.AtkAddModifier = atkAddModifier;
        this.AgiAddModifier = agiAddModifier;
        this.VitAddModifier = vitAddModifier;
        this.TalAddModifier = talAddModifier;
        this.LukAddModifier = lukAddModifier;
        this.AtkMultModifier = atkMultModifier;
        this.AgiMultModifier = agiMultModifier;
        this.VitMultModifier = vitMultModifier;
        this.TalMultModifier = talMultModifier;
        this.LukMultModifier = lukMultModifier;
        this.CritDamageAddModifier = critDamageAddModifier;
        this.MovementSpeedAddModifier = movementSpeedAddModifier;
        this.HealthRegenAddModifier = healthRegenAddModifier;
        this.AttackSpeedAddModifier = attackSpeedAddModifier;
        this.CooldownReductionAddModifier = cooldownReductionAddModifier;
        this.CritDamageMultModifier = critDamageMultModifier;
        this.MovementSpeedMultModifier = movementSpeedMultModifier;
        this.HealthRegenMultModifier = healthRegenMultModifier;
        this.AttackSpeedMultModifier = attackSpeedMultModifier;
        this.CooldownReductionMultModifier = cooldownReductionMultModifier;
    }

    public static StatsDto operator +(StatsDto a) => a;

    public static StatsDto operator -(StatsDto a)
    {
        return new StatsDto(
            -a.AtkAddModifier,
            -a.AgiAddModifier,
            -a.VitAddModifier,
            -a.TalAddModifier,
            -a.LukAddModifier,
            -a.AtkMultModifier,
            -a.AgiMultModifier,
            -a.VitMultModifier,
            -a.TalMultModifier,
            -a.LukMultModifier,
            -a.CritDamageAddModifier,
            -a.MovementSpeedAddModifier,
            -a.HealthRegenAddModifier,
            -a.AttackSpeedAddModifier,
            -a.CooldownReductionAddModifier,
            -a.CritDamageMultModifier,
            -a.MovementSpeedMultModifier,
            -a.HealthRegenMultModifier,
            -a.AttackSpeedMultModifier,
            -a.CooldownReductionMultModifier
        );
    }

    public static StatsDto operator +(StatsDto a, StatsDto b)
    {
        return new StatsDto(
            a.AtkAddModifier + b.AtkAddModifier,
            a.AgiAddModifier + b.AgiAddModifier,
            a.VitAddModifier + b.VitAddModifier,
            a.TalAddModifier + b.TalAddModifier,
            a.LukAddModifier + b.LukAddModifier,
            a.AtkMultModifier + b.AtkMultModifier,
            a.AgiMultModifier + b.AgiMultModifier,
            a.VitMultModifier + b.VitMultModifier,
            a.TalMultModifier + b.TalMultModifier,
            a.LukMultModifier + b.LukMultModifier,
            a.CritDamageAddModifier + b.CritDamageAddModifier,
            a.MovementSpeedAddModifier + b.MovementSpeedAddModifier,
            a.HealthRegenAddModifier + b.HealthRegenAddModifier,
            a.AttackSpeedAddModifier + b.AttackSpeedAddModifier,
            a.CooldownReductionAddModifier + b.CooldownReductionAddModifier,
            a.CritDamageMultModifier + b.CritDamageMultModifier,
            a.MovementSpeedMultModifier + b.MovementSpeedMultModifier,
            a.HealthRegenMultModifier + b.HealthRegenMultModifier,
            a.AttackSpeedMultModifier + b.AttackSpeedMultModifier,
            a.CooldownReductionMultModifier + b.CooldownReductionMultModifier
        );
    }

    public static StatsDto operator -(StatsDto a, StatsDto b)
    {
        return new StatsDto(
            a.AtkAddModifier - b.AtkAddModifier,
            a.AgiAddModifier - b.AgiAddModifier,
            a.VitAddModifier - b.VitAddModifier,
            a.TalAddModifier - b.TalAddModifier,
            a.LukAddModifier - b.LukAddModifier,
            a.AtkMultModifier - b.AtkMultModifier,
            a.AgiMultModifier - b.AgiMultModifier,
            a.VitMultModifier - b.VitMultModifier,
            a.TalMultModifier - b.TalMultModifier,
            a.LukMultModifier - b.LukMultModifier,
            a.CritDamageAddModifier - b.CritDamageAddModifier,
            a.MovementSpeedAddModifier - b.MovementSpeedAddModifier,
            a.HealthRegenAddModifier - b.HealthRegenAddModifier,
            a.AttackSpeedAddModifier - b.AttackSpeedAddModifier,
            a.CooldownReductionAddModifier - b.CooldownReductionAddModifier,
            a.CritDamageMultModifier - b.CritDamageMultModifier,
            a.MovementSpeedMultModifier - b.MovementSpeedMultModifier,
            a.HealthRegenMultModifier - b.HealthRegenMultModifier,
            a.AttackSpeedMultModifier - b.AttackSpeedMultModifier,
            a.CooldownReductionMultModifier - b.CooldownReductionMultModifier
        );
    }

    public override string ToString()
    {
        return "(" +
        AtkAddModifier + ", " +
        AgiAddModifier + ", " +
        VitAddModifier + ", " +
        TalAddModifier + ", " +
        LukAddModifier +
        "), (" +
        AtkMultModifier + ", " +
        AgiMultModifier + ", " +
        VitMultModifier + ", " +
        TalMultModifier + ", " +
        LukMultModifier + ")";
    }
}
