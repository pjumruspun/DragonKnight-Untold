using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRepository : MonoSingleton<EnemyRepository>
{
    [SerializeField]
    private List<GameObject> enemyPrefabs;
    private List<ObjectPool> enemyPools = new List<ObjectPool>();

    public int SpawnRandomEnemy(Vector3 position)
    {
        System.Random random = new System.Random();
        int idx = random.Next(enemyPools.Count);
        GameObject spawnedEnemy = enemyPools[idx].SpawnObject(position);
        if (spawnedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
        {
            return enemy.SpawnCost;
        }
        else
        {
            throw new System.Exception("Spawned enemy does not have Enemy script attached to.");
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
