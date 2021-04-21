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
            if(perk == targetPerk) 
            {
                perk.upgrade();
                break;
            }
        }
        //EventPublisher.TriggerInventoryChange();
    }

    private void Start()
    {
        //EventPublisher.InventoryChange += CalculateItemStats;
        CalculatePerkStats();
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
