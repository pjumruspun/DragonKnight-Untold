using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEffects
{
    private const string lifesteal = "Lifesteal";
    private const string bonusDamage = "Bonus Damage";
    private const string berserk = "Berserk";
    public static void LifeSteal(float damageDealt)
    {
        // Player lifesteal
        bool hasLifeStealPerk = PerkListStatic.HasPerk(lifesteal);
        int lifeStealLevel = PerkListStatic.GetPerkLevel(lifesteal);
        float lifestealRatio = 0.025f + 0.015f * lifeStealLevel;
        if (hasLifeStealPerk)
        {
            PlayerHealth.Instance.Heal(damageDealt * lifestealRatio);
        }
    }

    public static void TakeBonusDamage(float damage, Enemy target)
    {
        bool hasBonusDamage = PerkListStatic.HasPerk(bonusDamage);
        int bonusDamageLevel = PerkListStatic.GetPerkLevel(bonusDamage);
        float bonusDamageValue = 3.0f + 2.0f * bonusDamageLevel;
        Vector3 offset = new Vector3(0.5f, 0.3f, 0.0f);
        if (hasBonusDamage)
        {
            target.TakeDamage(bonusDamageValue);
            Color orange = new Color(0.9f, 0.5f, 0.1f);
            FloatingTextSpawner.Spawn(bonusDamageValue.ToString(), target.transform.position + offset, orange);
        }
    }

    public static float CalculateBerserkDamage(float damage)
    {
        bool hasBerserk = PerkListStatic.HasPerk(berserk);
        int berserkLevel = PerkListStatic.GetPerkLevel(berserk);
        float multiplier = 0.05f + 0.1f * berserkLevel;
        if (hasBerserk)
        {
            return damage * (1.0f + multiplier);
        }
        return damage;
    }

    public static void BerserkConsumeHealth()
    {
        bool hasBerserk = PerkListStatic.HasPerk(berserk);
        int berserkLevel = PerkListStatic.GetPerkLevel(berserk);
        float healthConsumed = 1.0f * berserkLevel;
        if (hasBerserk)
        {
            PlayerHealth.Instance.TakeDamage(healthConsumed);
        }
    }
}
