using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnEffect
{
    None,
    GreenFireColumn
}

public class EnemySpawnEffect : MonoSingleton<EnemySpawnEffect>
{
    [SerializeField]
    private float spawnEffectExpireTime = 1.5f;
    [SerializeField]
    private GameObject greenFireColumnPrefab;
    private ObjectPool greenFireColumnPool;

    public static GameObject CreateEffect(SpawnEffect spawnEffect, Vector3 position)
    {
        GameObject spawnedObject;
        switch (spawnEffect)
        {
            case SpawnEffect.None:
                spawnedObject = null;
                break;
            case SpawnEffect.GreenFireColumn:
                spawnedObject = Instance.greenFireColumnPool.SpawnObject(position);
                break;
            default:
                throw new System.ArgumentOutOfRangeException("Unregistered SpawnEffect enum");
        }

        // Delay disabled
        if (spawnedObject != null)
        {
            CoroutineUtility.ExecDelay(() => spawnedObject.SetActive(false), Instance.spawnEffectExpireTime);
        }

        return spawnedObject;
    }

    private void Start()
    {
        greenFireColumnPool = new ObjectPool(greenFireColumnPrefab, 10);
    }
}
