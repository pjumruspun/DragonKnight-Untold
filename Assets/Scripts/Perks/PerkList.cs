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
    }

    public void AddDevelopedPerk(Perk perk)
    {
        Perk newPerk = new Perk(perk);
        newPerk.PerkLevel = 1;
        newPerk.type = PerkType.Developed;
        PerkListStatic.perks.Add(newPerk);
    }

    public void Upgrade(Perk targetPerk)
    {
        foreach (var perk in PerkListStatic.perks)
        {
            if (perk.name == targetPerk.name)
            {
                perk.Upgrade();
                Debug.Log($"{perk.name} is at level {perk.PerkLevel}");
                break;
            }
        }
    }

    public void ResetPerk()
    {
        PerkListStatic.perks = new List<Perk>();
    }

    private void Start()
    {
        CalculatePerkStats();
        List<Perk> initPerk = new List<Perk>(PerkRepository.GetRandomGiftedPerk());
        for (int i = 0; i < initPerk.Count; i++)
        {
            this.AddGiftedPerk(initPerk[i]);
        }

        GameEvents.RestartGame += ResetPerk;
    }

    private void OnDestroy()
    {
        GameEvents.RestartGame -= ResetPerk;
    }

    private void CalculatePerkStats()
    {
        ItemStats accumulatedStats = new ItemStats();
        foreach (var perk in PerkListStatic.perks)
        {
            accumulatedStats = accumulatedStats + perk.stats;
        }
    }
}
