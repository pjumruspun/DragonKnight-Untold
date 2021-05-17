using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkList : MonoSingleton<PerkList>
{
    public void AddPerk(PerkTemplate perk)
    {
        PerkTemplate newPerk = new PerkTemplate(perk);
        newPerk.PerkLevel = 1;
        PerkStatic.perks.Add(newPerk);
    }

    public void Upgrade(PerkTemplate targetPerk)
    {
        foreach (var perk in PerkStatic.perks)
        {
            if (perk.type == targetPerk.type)
            {
                perk.Upgrade();
                Debug.Log($"{perk.type} is at level {perk.PerkLevel}");
                break;
            }
        }
    }

    public void ResetPerk()
    {
        PerkStatic.perks = new List<PerkTemplate>();
        PerkStatic.shouldRandom = true;
    }

    private void Start()
    {
        CalculatePerkStats();
        // Try to not random perk on start anymore
        GameEvents.RestartGame += ResetPerk;
        GameEvents.CompleteLevel += AddTokenOnCompleteLevel;
    }

    private void RandomPerk()
    {
        List<PerkTemplate> initPerk = new List<PerkTemplate>(PerkRepository.GetRandomGiftedPerk());
        for (int i = 0; i < initPerk.Count; i++)
        {
            this.AddPerk(initPerk[i]);
        }

        PerkStatic.shouldRandom = false;
    }

    private void OnDestroy()
    {
        GameEvents.RestartGame -= ResetPerk;
        GameEvents.CompleteLevel -= AddTokenOnCompleteLevel;
    }

    private void CalculatePerkStats()
    {
        StatsDto accumulatedStats = new StatsDto();
        foreach (var perk in PerkStatic.perks)
        {
            accumulatedStats = accumulatedStats + perk.stats;
        }
    }

    private void AddTokenOnCompleteLevel()
    {
        PerkStatic.upgradeToken += 1;
        Debug.Log("Upgrade token = " + PerkStatic.upgradeToken);
    }
}
