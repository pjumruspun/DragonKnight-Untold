using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageManager : MonoSingleton<FloatingDamageManager>
{
    [SerializeField]
    private GameObject floatingDamagePrefab;
    [SerializeField]
    private int floatingDamagePoolSize = 100; // How many floating damage can there be at time?
    [SerializeField]
    private float secondsToDespawn = 2.0f; // 2.0 seconds and the damage will despawn
    private ObjectPool floatingDamagePool;

    public void Spawn(float damage, Vector3 position)
    {
        GameObject floatingDamage = floatingDamagePool.GetObject(position);
        if (floatingDamage.TryGetComponent<TextMesh>(out TextMesh textMesh))
        {
            // Set damage number to the text mesh
            textMesh.text = $"{Mathf.Ceil(damage)}";
        }
        else
        {
            Debug.LogAssertion($"Failed to get TextMesh component from {floatingDamage.name} GameObject.");
        }

        StartCoroutine(HideAfterSeconds(floatingDamage, secondsToDespawn));
    }

    private void Start()
    {
        floatingDamagePool = new ObjectPool(floatingDamagePrefab, floatingDamagePoolSize);
    }

    private IEnumerator HideAfterSeconds(GameObject objectToHide, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        objectToHide.SetActive(false);
    }
}
