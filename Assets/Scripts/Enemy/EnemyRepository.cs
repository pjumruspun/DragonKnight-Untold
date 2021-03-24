using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRepository : MonoSingleton<EnemyRepository>
{
    [SerializeField]
    private List<GameObject> enemyPrefabs;
    [SerializeField]
    private float spawnDelay = 0.5f;
    private List<ObjectPool> enemyPools = new List<ObjectPool>();

    public int SpawnRandomEnemy(Vector3 position)
    {
        System.Random random = new System.Random();
        int idx = random.Next(enemyPools.Count);
        GameObject spawnedEnemy = enemyPools[idx].SpawnObject(position);

        // Disable first, then enable with delay
        spawnedEnemy.SetActive(false);
        CoroutineUtility.ExecDelay(() => spawnedEnemy.SetActive(true), spawnDelay);

        if (spawnedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
        {
            EnemySpawnEffect.CreateEffect(enemy.GetSpawnEffect, position); // Spawn the effect
            return enemy.SpawnCost;
        }
        else
        {
            throw new System.Exception($"Enemy script not found within the object name: {spawnedEnemy.name}");
        }
    }

    private void Start()
    {
        foreach (var prefab in enemyPrefabs)
        {
            enemyPools.Add(new ObjectPool(prefab));
        }
    }
}
