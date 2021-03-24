using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    [SerializeField]
    private bool shouldSpawn = true;
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
    private int maxSpawnAmountAtTime = 30;
    private float lastTimeSpawned = 0.0f;
    private float currentSpawnAmount = 0;
    private float enemyCostKilledThisStage = 0;
    private Transform player;

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
        if (shouldSpawn && Time.time - lastTimeSpawned >= spawnInterval && currentSpawnAmount < maxSpawnAmountAtTime)
        {
            // Try spawn
            MassSpawnEnemy();
            lastTimeSpawned = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            int spawnCost = 0;
            while (spawnCost <= 0)
            {
                spawnCost = TrySpawnEnemy();
            }
        }
    }

    private void MassSpawnEnemy()
    {
        int spawnAmountThisInterval = 0;
        while (spawnAmountThisInterval < spawnAmountPerInterval)
        {
            spawnAmountThisInterval += TrySpawnEnemy();
        }

        currentSpawnAmount += spawnAmountThisInterval;
    }

    private int TrySpawnEnemy()
    {
        // Check first if player is referenced
        if (player == null)
        {
            player = PlayerFinder.FindByTag().transform;
        }

        // Debug
        float randomX = player.position.x + Random.Range(-spawnRange, spawnRange);
        float randomY = player.position.y + Random.Range(-spawnRange, spawnRange);
        Vector2 randomPosition = new Vector2(randomX, randomY);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(randomPosition, castRadius, Vector2.zero);


        // Check 9 tiles around the random tile
        for (int x = (int)randomX - 1; x <= (int)randomX + 1; ++x)
        {
            for (int y = (int)randomY - 1; y <= (int)randomY + 1; ++y)
            {
                if (!IsTileEmpty(randomX, randomY))
                {
                    // Something exist at 3 tiles in the middle and upper
                    Debug.DrawLine(player.position, randomPosition, Color.red, 2.0f);
                    return 0;
                }
            }
        }

        if (!IsGroundPosition(randomX, randomY))
        {
            Debug.DrawLine(player.position, randomPosition, Color.red, 2.0f);
            return 0;
        }

        randomY = Mathf.Ceil(randomY);
        Debug.DrawLine(player.position, randomPosition, Color.green, 5.0f);

        int spawnCost = EnemyRepository.Instance.SpawnRandomEnemy(randomPosition);
        return spawnCost;
    }

    private bool IsGroundPosition(float positionX, float positionY)
    {
        for (int x = (int)positionX - 1; x <= (int)positionX + 1; ++x)
        {
            for (int y = (int)positionY; y <= (int)positionY + 1; ++y)
            {
                if (!IsTileEmpty(positionX, positionY))
                {
                    // Something is on upper six tile
                    return false;
                }
            }
        }

        if (IsTileEmpty((int)positionX, (int)positionY - 1))
        {
            // No ground
            return false;
        }

        return true;
    }

    private bool IsTileEmpty(float positionX, float positionY)
    {
        return tilemap.GetTile(new Vector3Int(Mathf.CeilToInt(positionX), Mathf.FloorToInt(positionY), 0)) == null;
    }

    private void ProcessSpawnAmount(Enemy enemy)
    {
        currentSpawnAmount -= enemy.SpawnCost;
        enemyCostKilledThisStage += enemy.SpawnCost;
        if (enemyCostKilledThisStage >= StageManager.CostToPassLevel)
        {
            GameEvents.TriggerCompleteLevel();
            Debug.Log("Player completed a level!");
        }
    }
}
