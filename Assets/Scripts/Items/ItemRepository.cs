using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all types of items
public class ItemRepository : MonoSingleton<ItemRepository>
{
    [SerializeField]
    private List<Item> ultraRareItems;
    [SerializeField]
    private List<Item> rareItems;
    [SerializeField]
    private List<Item> commonItems;

    public Item GetRandomItem(ItemRarity rarity)
    {
        System.Random rnd = new System.Random();
        int idx = -1;
        switch (rarity)
        {
            case ItemRarity.UltraRare:
                idx = rnd.Next(ultraRareItems.Count);
                return ultraRareItems[idx];
            case ItemRarity.Rare:
                idx = rnd.Next(rareItems.Count);
                return rareItems[idx];
            case ItemRarity.Common:
                idx = rnd.Next(commonItems.Count);
                return commonItems[idx];
            default:
                throw new System.NotImplementedException("No such rarity exist");
        }
    }
}
