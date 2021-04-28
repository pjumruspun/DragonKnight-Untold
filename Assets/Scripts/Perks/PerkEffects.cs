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
}
