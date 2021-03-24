using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnSide
{
    Right,
    Left
}

public class SpawnPosition : MonoBehaviour
{
    public SpawnSide Side => spawnSide;
    [SerializeField]
    private GameObject blockingObject;
    [SerializeField]
    private GameObject gateToNextLevel;
    [SerializeField]
    private SpawnSide spawnSide;

    public void Block()
    {
        blockingObject.SetActive(true);
    }

    public void Unblock()
    {
        blockingObject.SetActive(false);
    }

    public void DisableTeleportArea()
    {
        gateToNextLevel.SetActive(false);
    }

    public void EnableTeleportArea()
    {
        gateToNextLevel.SetActive(true);
    }

    private void Start()
    {
        // Unblock when player completes level
        GameEvents.CompleteLevel += Unblock;
    }

    private void OnDestroy()
    {
        GameEvents.CompleteLevel -= Unblock;
    }
}
