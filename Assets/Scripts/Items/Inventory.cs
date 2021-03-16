using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public Dictionary<Item, int> ItemCount => itemCount;
    private List<Item> items = new List<Item>();
    private Dictionary<Item, int> itemCount = new Dictionary<Item, int>();

    public void Add(Item item)
    {
        items.Add(item);
        if (itemCount.TryGetValue(item, out int count))
        {
            // Item exist
            ++itemCount[item];
        }
        else
        {
            // Item not exist
            itemCount[item] = 1;
        }

        EventPublisher.TriggerInventoryChange();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        --itemCount[item];
        if (itemCount[item] == 0)
        {
            itemCount.Remove(item);
        }

        EventPublisher.TriggerInventoryChange();
    }

    private void Start()
    {
        EventPublisher.InventoryChange += CalculateItemStats;
    }

    private void OnDestroy()
    {
        EventPublisher.InventoryChange -= CalculateItemStats;
    }

    private void CalculateItemStats()
    {
        ItemStats accumulatedStats = new ItemStats();
        foreach (var item in items)
        {
            // Debug.Log(item.stats);
            accumulatedStats = accumulatedStats + item.stats;
            // Debug.Log(accumulatedStats + item.stats);
        }

        PlayerStats.Instance.AssignStatsFromItems(accumulatedStats);
    }
}
