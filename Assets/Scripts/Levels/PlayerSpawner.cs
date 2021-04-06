using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoSingleton<PlayerSpawner>
{
    [SerializeField]
    private GameObject playerPrefab;
    private List<SpawnPosition> spawnPosToEnableWhenComplete = new List<SpawnPosition>();

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        // CoroutineUtility.ExecDelay(() => SpawnPlayer(), Time.deltaTime);
        SpawnPlayer();
        GameEvents.CompleteLevel += ReEnableExits;
    }

    private void OnDestroy()
    {
        GameEvents.CompleteLevel -= ReEnableExits;
    }

    private void SpawnPlayer()
    {
        if (GameStateManager.State == GameState.Gameplay)
        {
            // Spawn if in game
            GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.SpawnPoint);
            if (allSpawnPoints.Length == 0)
            {
                throw new System.Exception("Spawn point not exist in the scene");
            }

            List<SpawnPosition> possibleSpawnPositions = new List<SpawnPosition>();

            // Spawn player with appropriate spawn point
            foreach (var spawn in allSpawnPoints)
            {
                if (spawn.TryGetComponent<SpawnPosition>(out SpawnPosition spawnPosition))
                {
                    // If last stage player exit at left side, player should spawn at the right side of the new stage
                    // And vise versa
                    if (spawnPosition.Side == SpawnSide.Left && StageManager.lastStageExitSide == SpawnSide.Right)
                    {
                        possibleSpawnPositions.Add(spawnPosition);
                    }
                    else if (spawnPosition.Side == SpawnSide.Right && StageManager.lastStageExitSide == SpawnSide.Left)
                    {
                        possibleSpawnPositions.Add(spawnPosition);
                    }
                    else if (spawnPosition.Side == SpawnSide.None)
                    {
                        // Camp spawn side
                        possibleSpawnPositions.Add(spawnPosition);
                    }
                }
                else
                {
                    throw new System.Exception($"Object {spawn.name} does not have SpawnPoint script attached to.");
                }
            }

            if (possibleSpawnPositions.Count == 0)
            {
                throw new System.Exception($"Could not spawn player because there is no possible spawn location.");
            }

            // The spawn position will the player spawn at
            int random = Random.Range(0, possibleSpawnPositions.Count);
            SpawnPosition spawnPositionToSpawn = possibleSpawnPositions[random];

            // Unblock the gate
            spawnPositionToSpawn.Unblock();

            // Disable all exits
            foreach (var spawnPoint in allSpawnPoints)
            {
                SpawnPosition spawnPosition = spawnPoint.GetComponent<SpawnPosition>();
                spawnPosition.DisableTeleportArea();
                // Keep track of all non-spawn exits to enable later
                if (spawnPosition != spawnPositionToSpawn)
                {
                    spawnPosToEnableWhenComplete.Add(spawnPosition);
                }
            }

            // And spawn player there
            GameObject player = Instantiate(playerPrefab, spawnPositionToSpawn.transform.position, Quaternion.identity);
            EventPublisher.TriggerPlayerSpawn(player.transform);
        }
    }

    private void ReEnableExits()
    {
        foreach (var spawnPosition in spawnPosToEnableWhenComplete)
        {
            spawnPosition.EnableTeleportArea();
        }
    }
}
