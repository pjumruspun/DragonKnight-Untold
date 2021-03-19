using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoSingleton<PlayerSpawner>
{
    [SerializeField]
    private GameObject playerPrefab;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point");
        if (spawnPoint == null)
        {
            Debug.LogAssertion("Spawn point not exist in the scene");
        }

        GameObject player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        EventPublisher.TriggerPlayerSpawn(player.transform);
    }
}
