using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkList : MonoSingleton<PerkList>
{
    public void AddPerk(PerkTemplate perk)
    {
        PerkTemplate newPerk = new PerkTemplate(perk);
        newPerk.PerkLevel = 1;
        PerkListStatic.perks.Add(newPerk);
    }

    public void Upgrade(PerkTemplate targetPerk)
    {
        foreach (var perk in PerkListStatic.perks)
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
        PerkListStatic.perks = new List<PerkTemplate>();
        PerkListStatic.shouldRandom = true;
    }

    private void Start()
    {
        CalculatePerkStats();
        // Try to not random perk on start anymore
        GameEvents.RestartGame += ResetPerk;
    }

    private void RandomPerk()
    {
        List<PerkTemplate> initPerk = new List<PerkTemplate>(PerkRepository.GetRandomGiftedPerk());
        for (int i = 0; i < initPerk.Count; i++)
        {
            this.AddPerk(initPerk[i]);
        }

        PerkListStatic.shouldRandom = false;
    }

    private void OnDestroy()
    {
        GameEvents.RestartGame -= ResetPerk;
    }

    private void CalculatePerkStats()
    {
        StatsDto accumulatedStats = new StatsDto();
        foreach (var perk in PerkListStatic.perks)
        {
            accumulatedStats = accumulatedStats + perk.stats;
        }
    }
}
