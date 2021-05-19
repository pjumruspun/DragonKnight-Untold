using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoSingleton<PerkManager>
{
    public static void Upgrade(PerkTemplate targetPerk)
    {
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

        Debug.Log($"Upgrading perk: {targetPerk.type}");

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
                Debug.Log($"{perkToUpgrade.type} is at Lv{perkToUpgrade.perkLevel}");
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
            Debug.Log($"{perkToAdd.type} is at Lv{perkToAdd.perkLevel}");
        }
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
        GameEvents.CompleteLevel += AddTokenOnCompleteLevel;
    }

    private void ShouldRandomPerk()
    {
        PerkStatic.shouldRandom = true;
    }

    private void OnDestroy()
    {
        GameEvents.RestartGame -= ResetPerk;
        GameEvents.RestartGame -= ShouldRandomPerk;
        GameEvents.CompleteLevel -= AddTokenOnCompleteLevel;
    }

    private void AddTokenOnCompleteLevel()
    {
        PerkStatic.upgradeToken += 1;
    }

    private void Update()
    {
        // Debug only
        if (Input.GetKeyDown(KeyCode.U))
        {
            PerkUpgradeMenu.Activate();
        }
    }
}
