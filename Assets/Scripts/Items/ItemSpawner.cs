using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float ultraRareSpawnChance = 0.15f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float rareSpawnChance = 0.30f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float commonSpawnChance = 0.65f;

    public void SpawnItem()
    {
        float random = Random.Range(0.0f, 1.0f);
        bool willSpawn = random < commonSpawnChance + rareSpawnChance + ultraRareSpawnChance;
        GameObject spawnedItem;
        if (willSpawn)
        {
            // Blank item
            spawnedItem = ObjectManager.Instance.ItemPickups.SpawnObject(transform.position);
        }
        else
        {
            // Do not spawn anything
            return;
        }

        // Try get ItemPickup component
        var itemPickup = spawnedItem.GetComponentInChildren<ItemPickup>();
        if (itemPickup != null)
        {
            Item randomItem;
            // Choose rarity
            if (random < ultraRareSpawnChance)
            {
                // Ultra rare spawn
                randomItem = ItemRepository.Instance.GetRandomItem(ItemRarity.UltraRare);
            }
            else if (random < rareSpawnChance + ultraRareSpawnChance)
            {
                // Rare spawn
                randomItem = ItemRepository.Instance.GetRandomItem(ItemRarity.Rare);
            }
            else if (random < commonSpawnChance + rareSpawnChance + ultraRareSpawnChance)
            {
                // Common spawn
                randomItem = ItemRepository.Instance.GetRandomItem(ItemRarity.Common);
            }
            else
            {
                throw new System.InvalidOperationException("Item spawn calculation error");
            }

            itemPickup.AssignItem(randomItem);
        }
        else
        {
            throw new System.NullReferenceException("Spawned ItemPickup object does not contain ItemPickup Script");
        }
    }
}
