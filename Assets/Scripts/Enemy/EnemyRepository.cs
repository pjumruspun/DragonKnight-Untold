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

    public Enemy SpawnRandomEnemy(Vector3 position)
    {
        System.Random random = new System.Random();
        int idx = random.Next(enemyPools.Count);
        GameObject spawnedEnemy = enemyPools[idx].SpawnObject(position);

        if (spawnedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
        {
            // Disable first, then enable with delay
            enemy.SetRendererActive(false);
            CoroutineUtility.ExecDelay(() => enemy.SetRendererActive(true), spawnDelay);

            // Spawn the effect
            EnemySpawnEffect.CreateEffect(enemy.GetSpawnEffect, position);
            return enemy;
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
