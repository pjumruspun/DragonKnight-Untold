using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemChestManager : MonoBehaviour
{
    [SerializeField]
    private int totalChestToSpawn = 0;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float shinyChestRatio = 0.2f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float normalChestRatio = 0.8f;

    private const string itemChestSpawnTag = "Item Chest Spawn";

    private void Start()
    {
        GameObject[] itemChestSpawns = GameObject.FindGameObjectsWithTag(itemChestSpawnTag);

        // Shuffle array so chest randomly spawns
        System.Random rnd = new System.Random();
        GameObject[] randomChestSpawns = itemChestSpawns.OrderBy(x => rnd.Next()).ToArray();

        // There should be enough existing chest location to spawn
        if (randomChestSpawns.Length < totalChestToSpawn)
        {
            Debug.LogWarning($"Found {itemChestSpawns.Length} GameObject with {itemChestSpawnTag} in the scene, but commanded to spawn total of {totalChestToSpawn}");
        }

        // Calculate probability
        float totalRatio = shinyChestRatio + normalChestRatio;
        float softmaxShiny = shinyChestRatio / totalRatio;
        float softmaxNormal = normalChestRatio / totalRatio;

        // Actual spawning
        int spawnedCount = 0;
        foreach (var obj in randomChestSpawns)
        {
            if (obj.TryGetComponent<ItemChestSpawn>(out var spawn))
            {
                float random = Random.Range(0.0f, 1.0f);
                if (random < softmaxShiny)
                {
                    spawn.SpawnShinyChest();
                }
                else if (random < softmaxShiny + softmaxNormal)
                {
                    spawn.SpawnNormalChest();
                }
                else
                {
                    throw new System.Exception("Softmax probability error");
                }

                ++spawnedCount;
                if (spawnedCount >= totalChestToSpawn)
                {
                    break;
                }
            }
            else
            {
                throw new System.Exception($"Could not TryGetCompoenent<ItemChestSpawn> from GameObject {obj.name}");
            }
        }
    }
}
