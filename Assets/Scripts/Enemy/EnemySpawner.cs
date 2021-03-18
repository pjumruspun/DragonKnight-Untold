using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    [SerializeField]
    private bool shouldSpawn = true;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private float spawnRange = 10.0f;
    [SerializeField]
    private float castRadius = 2.0f;
    [SerializeField]
    private float spawnInterval = 15.0f;
    [SerializeField]
    private int spawnAmountPerInterval = 15;
    [SerializeField]
    private int maxSpawnAmount = 50;
    private float lastTimeSpawned = 0.0f;
    private float currentSpawnAmount = 0;

    private void Start()
    {
        EventPublisher.EnemyDead += ProcessSpawnAmount;
        lastTimeSpawned = -spawnInterval + 2.0f; // So it spawns right at start plus some seconds
    }

    private void OnDestroy()
    {
        EventPublisher.EnemyDead -= ProcessSpawnAmount;
    }

    private void Update()
    {
        if (shouldSpawn && Time.time - lastTimeSpawned >= spawnInterval && currentSpawnAmount < maxSpawnAmount)
        {
            // Try spawn
            MassSpawnEnemy();
            lastTimeSpawned = Time.time;
        }
    }

    private void MassSpawnEnemy()
    {
        int spawnAmountThisInterval = 0;
        while (spawnAmountThisInterval < spawnAmountPerInterval)
        {
            spawnAmountThisInterval += TrySpawnEnemy();
        }

        Debug.Log($"Spawn amount increased from {currentSpawnAmount} to {currentSpawnAmount + spawnAmountThisInterval}");
        currentSpawnAmount += spawnAmountThisInterval;
    }

    private int TrySpawnEnemy()
    {
        // Debug
        float randomX = player.position.x + Random.Range(-spawnRange, spawnRange);
        float randomY = player.position.y + Random.Range(-spawnRange, spawnRange);
        Vector2 randomPosition = new Vector2(randomX, randomY);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(randomPosition, castRadius, Vector2.zero);
        Debug.DrawLine(player.position, randomPosition, Color.blue, 2.0f);

        // Check 9 tiles around the random tile
        for (int x = (int)randomX - 1; x <= (int)randomX + 1; ++x)
        {
            for (int y = (int)randomY - 1; y <= (int)randomY + 1; ++y)
            {
                TileBase tileBase = tilemap.GetTile(new Vector3Int((int)randomX, (int)randomY, 0));
                if (tileBase != null)
                {
                    // Something exist here
                    return 0;
                }
            }
        }

        int spawnCost = EnemyRepository.Instance.SpawnRandomEnemy(randomPosition);
        return spawnCost;
    }

    private void ProcessSpawnAmount(Enemy enemy)
    {
        currentSpawnAmount -= enemy.SpawnCost;
    }
}
