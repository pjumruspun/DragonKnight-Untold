using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all types of perks
public class PerkRepository : MonoSingleton<PerkRepository>
{
    [SerializeField]
    private List<Perk> giftedPerk;
    [SerializeField]
    private List<Perk> developedPerk;
    [SerializeField]
    private List<Perk> wearablePerk;

    public Perk GetRandomGiftedPerk()
    {
        System.Random rnd = new System.Random();
        int idx = -1;
        idx = rnd.Next(giftedPerk.Count);
        return giftedPerk[idx];
           
    }
}
