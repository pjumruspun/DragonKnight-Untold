using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkList : MonoSingleton<PerkList>
{
    public void Add(Perk perk)
    {
        PerkListStatic.perks.Add(perk);
        //EventPublisher.TriggerInventoryChange();
    }

    public void Upgrade(Perk targetPerk)
    {
        foreach (var perk in PerkListStatic.perks)
        {
            if(perk.name == targetPerk.name) 
            {
                perk.Upgrade();
                break;
            }
        }
        //EventPublisher.TriggerInventoryChange();
    }

    public int GetPerkLevel(Perk targetPerk)
    {
        foreach (var perk in PerkListStatic.perks)
        {
            if(perk.name == targetPerk.name) 
            {
                return perk.PerkLevel;
            }
        }
        return 0;
        //EventPublisher.TriggerInventoryChange();
    }

    private void Start()
    {
        //EventPublisher.InventoryChange += CalculateItemStats;
        CalculatePerkStats();
        List<Perk> initPerk = new List<Perk>(PerkRepository.GetRandomGiftedPerk());
        for(int i = 0; i < initPerk.Count; i++)
        {
            this.Add(initPerk[i]);
        }
        Debug.Log(PerkListStatic.perks);
    }

    private void OnDestroy()
    {
        //EventPublisher.InventoryChange -= CalculateItemStats;
    }

    private void CalculatePerkStats()
    {
        ItemStats accumulatedStats = new ItemStats();
        foreach (var perk in PerkListStatic.perks)
        {
            // Debug.Log(perk.stats);
            accumulatedStats = accumulatedStats + perk.stats;
            // Debug.Log(accumulatedStats + perk.stats);
        }

        //PlayerStats.Instance.AssignStatsFromItems(accumulatedStats);
    }
}
