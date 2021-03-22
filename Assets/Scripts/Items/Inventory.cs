using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{


    public void Add(Item item)
    {
        InventoryStatic.items.Add(item);
        if (InventoryStatic.itemCount.TryGetValue(item, out int count))
        {
            // Item exist
            ++InventoryStatic.itemCount[item];
        }
        else
        {
            // Item not exist
            InventoryStatic.itemCount[item] = 1;
        }

        EventPublisher.TriggerInventoryChange();
    }

    public void Remove(Item item)
    {
        InventoryStatic.items.Remove(item);
        --InventoryStatic.itemCount[item];
        if (InventoryStatic.itemCount[item] == 0)
        {
            InventoryStatic.itemCount.Remove(item);
        }

        EventPublisher.TriggerInventoryChange();
    }

    private void Start()
    {
        EventPublisher.InventoryChange += CalculateItemStats;
        CalculateItemStats();
    }

    private void OnDestroy()
    {
        EventPublisher.InventoryChange -= CalculateItemStats;
    }

    private void CalculateItemStats()
    {
        ItemStats accumulatedStats = new ItemStats();
        foreach (var item in InventoryStatic.items)
        {
            // Debug.Log(item.stats);
            accumulatedStats = accumulatedStats + item.stats;
            // Debug.Log(accumulatedStats + item.stats);
        }

        PlayerStats.Instance.AssignStatsFromItems(accumulatedStats);
    }
}
