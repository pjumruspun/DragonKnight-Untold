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

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float keySpawnChance = 0.3f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float potionSpawnChance = 0.25f;

    [SerializeField]
    private bool guaranteedSpawn = false;

    public void SpawnKey()
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random < keySpawnChance)
        {
            // Spawn a key
            ObjectManager.Instance.Keys.SpawnObject(transform.position + RandomPosition());
        }
    }

    public void SpawnPotion()
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random < potionSpawnChance)
        {
            // Spawn a potion
            ObjectManager.Instance.HealthPotion.SpawnObject(transform.position + RandomPosition());
        }
    }

    public void SpawnItem()
    {
        float commonSpawnChance = this.commonSpawnChance;
        float rareSpawnChance = this.rareSpawnChance;
        float ultraRareSpawnChance = this.ultraRareSpawnChance;

        SpawnItem(commonSpawnChance, rareSpawnChance, ultraRareSpawnChance);
    }

    public void SpawnItem(float common, float rare, float ultraRare)
    {
        float random = Random.Range(0.0f, 1.0f);
        bool willSpawn = random < common + rare + ultraRare;
        GameObject spawnedItem;
        if (willSpawn)
        {
            // Blank item
            spawnedItem = ObjectManager.Instance.ItemPickups.SpawnObject(transform.position + RandomPosition());
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
            if (random < ultraRare)
            {
                // Ultra rare spawn
                randomItem = ItemRepository.Instance.GetRandomItem(ItemRarity.UltraRare);
            }
            else if (random < rare + ultraRare)
            {
                // Rare spawn
                randomItem = ItemRepository.Instance.GetRandomItem(ItemRarity.Rare);
            }
            else if (random < common + rare + ultraRare || guaranteedSpawn)
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

    private Vector3 RandomPosition()
    {
        float randomX = Random.Range(-0.5f, 0.5f);
        return Vector3.right * randomX;
    }
}
