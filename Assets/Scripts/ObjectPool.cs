using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For pooling any GameObject that will be constantly created and destroyed during the gameplay.
/// </summary>
public class ObjectPool
{
    private GameObject objectToPool;
    private List<GameObject> pooledObjects;
    private int poolSize = 0;

    /// <summary>
    /// Create GameObject pool from a prefab, with size limit.
    /// </summary>
    /// <param name="objectToPool">Prefab to instantiate.</param>
    /// <param name="initialPoolSize">Limit number of the prefab to be in the scene at time.</param>
    public ObjectPool(GameObject objectToPool, int initialPoolSize)
    {
        this.objectToPool = objectToPool;
        this.poolSize = initialPoolSize;
        pooledObjects = new List<GameObject>(poolSize);
        for (int _ = 0; _ < poolSize; ++_)
        {
            GameObject instantiatedObj = GameObject.Instantiate(objectToPool);
            instantiatedObj.SetActive(false);
            pooledObjects.Add(instantiatedObj);
        }
    }

    /// <summary>
    /// Get a pooled GameObject.
    /// </summary>
    /// <returns>Pooled GameObject.</returns>
    public GameObject GetObject()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                pooledObject.SetActive(true);
                return pooledObject;
            }
        }

        throw new System.IndexOutOfRangeException();
    }

    /// <summary>
    /// Get a pooled GameObject.
    /// </summary>
    /// <param name="position">Position of the pooled GameObject to spawn at.</param>
    /// <returns>Pooled GameObject.</returns>
    public GameObject GetObject(Vector3 position)
    {
        GameObject pooledObject = GetObject();
        pooledObject.transform.position = position;
        return pooledObject;
    }
}
