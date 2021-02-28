using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageManager : MonoSingleton<FloatingDamageManager>
{
    [SerializeField]
    private GameObject floatingDamagePrefab;
    [SerializeField]
    private GameObject critFloatingDamagePrefab;
    [SerializeField]
    private int floatingDamagePoolSize = 100; // How many floating damage can there be at time?
    [SerializeField]
    private int critFloatingDamagePoolSize = 100;
    [SerializeField]
    private float secondsToDespawn = 2.0f; // 2.0 seconds and the damage will despawn
    private ObjectPool floatingDamagePool;
    private ObjectPool critFloatingDamagePool;

    public void Spawn(float damage, Vector3 position, bool crit)
    {
        GameObject floatingDamage;
        if (crit)
        {
            floatingDamage = critFloatingDamagePool.SpawnObject(position);
        }
        else
        {
            floatingDamage = floatingDamagePool.SpawnObject(position);
        }

        if (floatingDamage.TryGetComponent<TextMesh>(out TextMesh textMesh))
        {
            // Set damage number to the text mesh
            textMesh.text = $"{Mathf.Ceil(damage)}";

            // Crit will add "!" at the end
            textMesh.text += crit ? "!" : "";
        }
        else
        {
            Debug.LogAssertion($"Failed to get TextMesh component from {floatingDamage.name} GameObject.");
        }

        StartCoroutine(CoroutineUtility.Instance.HideAfterSeconds(floatingDamage, secondsToDespawn));
    }

    private void Start()
    {
        floatingDamagePool = new ObjectPool(floatingDamagePrefab, floatingDamagePoolSize);
        critFloatingDamagePool = new ObjectPool(critFloatingDamagePrefab, critFloatingDamagePoolSize);
    }
}
