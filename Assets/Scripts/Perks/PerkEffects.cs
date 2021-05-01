using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEffects
{
    public static void LifeSteal(float damageDealt)
    {
        // Player lifesteal
        string lifesteal = "Lifesteal";
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
        string bonusDamage = "Bonus Damage";
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
}
