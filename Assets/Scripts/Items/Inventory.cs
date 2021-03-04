using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);
        EventPublisher.TriggerInventoryChange();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
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
