using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChestSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject previewChest;

    [SerializeField]
    private GameObject shinyChestPrefab;

    [SerializeField]
    private GameObject normalChestPrefab;

    public void SpawnShinyChest()
    {
        GameObject spawnedChest = Instantiate(shinyChestPrefab, transform.position, Quaternion.identity);
        spawnedChest.transform.SetParent(transform);
    }

    public void SpawnNormalChest()
    {
        GameObject spawnedChest = Instantiate(normalChestPrefab, transform.position, Quaternion.identity);
        spawnedChest.transform.SetParent(transform);
    }

    private void Start()
    {
        previewChest.SetActive(false);
    }
}
