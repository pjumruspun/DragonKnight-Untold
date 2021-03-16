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
        pooledObjects = new List<GameObject>();
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
    public GameObject SpawnObject()
    {
        GameObject objectToSpawn = GetFirstActiveObject();
        if (objectToSpawn)
        {
            return objectToSpawn;
        }

        // If reaches here it means that we need to increase size
        // 2 times the old size
        for (int _ = 0; _ < poolSize; ++_)
        {
            GameObject instantiatedObj = GameObject.Instantiate(objectToPool);
            instantiatedObj.SetActive(false);
            pooledObjects.Add(instantiatedObj);
        }

        poolSize *= 2;

        // Now we can Get first active object again
        objectToSpawn = GetFirstActiveObject();
        if (objectToSpawn)
        {
            return objectToSpawn;
        }

        throw new System.IndexOutOfRangeException();
    }

    /// <summary>
    /// Get a pooled GameObject.
    /// </summary>
    /// <param name="position">Position of the pooled GameObject to spawn at.</param>
    /// <returns>Pooled GameObject.</returns>
    public GameObject SpawnObject(Vector3 position)
    {
        GameObject pooledObject = SpawnObject();
        pooledObject.transform.position = position;
        return pooledObject;
    }

    /// <summary>
    /// Get a pooled GameObject.
    /// </summary>
    /// <param name="position">Position of the pooled GameObject to spawn at.</param>
    /// <param name="quaternion">Rotation angle in quaternion of the pooled GameObject to be tilted.</param>
    /// <returns>Pooled GameObject.</returns>
    public GameObject SpawnObject(Vector3 position, Quaternion quaternion)
    {
        GameObject pooledObject = SpawnObject();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = quaternion;
        return pooledObject;
    }

    /// <summary>
    /// Returning a GameObject. Equivalent to Destroy(GameObject).
    /// </summary>
    /// <param name="gameObject">GameObject to return.</param>
    public void ReturnObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private GameObject GetFirstActiveObject()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                pooledObject.SetActive(true);
                return pooledObject;
            }
        }

        return null;
    }
}
