using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoSingleton<PerkManager>
{
    public static void Upgrade(PerkTemplate targetPerk)
    {
        // Check validity
        if (PerkStatic.upgradeToken < 1)
        {
            throw new System.InvalidOperationException($"Upgrade perk being called without having any upgrade token");
        }

        bool exists = false;
        Perk perkToUpgrade = null;
        foreach (var perk in PerkStatic.perks)
        {
            if (perk.type == targetPerk.type)
            {
                exists = true;
                perkToUpgrade = perk;
                break;
            }
        }

        if (exists)
        {
            // Perk already exists in PerkStatic.perks
            // We just need to add one level into it
            if (perkToUpgrade == null)
            {
                throw new System.Exception("Error in PerkManager.Upgrade(): perkToUpgrade is null");
            }

            if (perkToUpgrade.perkLevel < perkToUpgrade.maxPerkLevel)
            {
                // Increase level
                ++perkToUpgrade.perkLevel;
            }
            else
            {
                // Cannot upgrade
                throw new System.Exception($"Illegal operation: Perk {perkToUpgrade.type} is at Lv{perkToUpgrade.perkLevel}/{perkToUpgrade.maxPerkLevel}");
            }
        }
        else
        {
            // New perk is being added
            // Need to create a new instance
            Perk perkToAdd = targetPerk.CreatePerk();
            PerkStatic.perks.Add(perkToAdd);
        }

        // Use perk upgrade token
        PerkStatic.upgradeToken -= 1;
    }

    public void ResetPerk()
    {
        PerkStatic.perks = new List<Perk>();
    }

    private void Start()
    {
        // Try to not random perk on start anymore
        GameEvents.RestartGame += ResetPerk;
        GameEvents.RestartGame += ShouldRandomPerk;
    }

    private void ShouldRandomPerk()
    {
        PerkStatic.shouldRandom = true;
    }

    private void OnDestroy()
    {
        GameEvents.RestartGame -= ResetPerk;
        GameEvents.RestartGame -= ShouldRandomPerk;
    }
}
