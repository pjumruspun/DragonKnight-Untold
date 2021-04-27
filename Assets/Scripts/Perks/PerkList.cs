using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkList : MonoSingleton<PerkList>
{
    public void AddGiftedPerk(Perk perk)
    {
        Perk newPerk = new Perk(perk);
        newPerk.PerkLevel = 1;
        newPerk.type = PerkType.Gifted;
        PerkListStatic.perks.Add(newPerk);
        //EventPublisher.TriggerInventoryChange();
    }

    public void AddDevelopedPerk(Perk perk)
    {
        Perk newPerk = new Perk(perk);
        newPerk.PerkLevel = 1;
        newPerk.type = PerkType.Developed;
        PerkListStatic.perks.Add(newPerk);
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

    public void ResetPerk()
    {
        PerkListStatic.perks = new List<Perk>();
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
            this.AddGiftedPerk(initPerk[i]);
        }
        
    }

    private void OnDestroy()
    {
        //EventPublisher.InventoryChange -= CalculateItemStats;
        this.ResetPerk();
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
