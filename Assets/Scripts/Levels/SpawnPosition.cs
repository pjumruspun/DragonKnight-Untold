using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnSide
{
    Right,
    Left,
    None,
}

public class SpawnPosition : MonoBehaviour
{
    public SpawnSide Side => spawnSide;
    [SerializeField]
    private GameObject blockingObject;
    [SerializeField]
    private TeleportArea gateToNextLevel;
    [SerializeField]
    private SpawnSide spawnSide;

    public void Block()
    {
        if (blockingObject != null)
        {
            blockingObject.SetActive(true);
        }
    }

    public void Unblock()
    {
        if (blockingObject != null)
        {
            blockingObject.SetActive(false);
        }
    }

    public void DisableTeleportArea()
    {
        if (gateToNextLevel != null)
        {
            print($"Disabling {gateToNextLevel.gameObject.name}");
            gateToNextLevel.gameObject.SetActive(false);
        }
    }

    public void EnableTeleportArea()
    {
        if (gateToNextLevel != null)
        {
            print($"Enabling {gateToNextLevel.gameObject.name}");
            gateToNextLevel.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        // Unblock when player completes level
        GameEvents.CompleteLevel += Unblock;

        // Tell teleport area which side this portal is
        if (gateToNextLevel != null)
        {
            gateToNextLevel.Initialize(spawnSide);
        }
    }

    private void OnDestroy()
    {
        GameEvents.CompleteLevel -= Unblock;
    }
}
