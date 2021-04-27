using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TeleportArea : MonoBehaviour
{
    private SpawnSide playerSpawnerSpawnSide;

    public void Initialize(SpawnSide playerSpawnerSpawnSide)
    {
        this.playerSpawnerSpawnSide = playerSpawnerSpawnSide;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerMovement>(out var _))
        {
            StageManager.lastStageExitSide = playerSpawnerSpawnSide;
            LevelChanger.LoadNextLevel();
        }
    }
}
