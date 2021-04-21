using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : MonoSingleton<FloatingTextSpawner>
{
    [SerializeField]
    private float secondsToDespawn = 2.0f; // 2.0 seconds and the damage will despawn

    public static void Spawn(float damage, Vector3 position, bool crit)
    {
        GameObject floatingDamage;
        if (crit)
        {
            floatingDamage = ObjectManager.Instance.CritFloatingDamage.SpawnObject(position);
        }
        else
        {
            floatingDamage = ObjectManager.Instance.FloatingDamage.SpawnObject(position);
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

        Instance.StartCoroutine(CoroutineUtility.Instance.HideAfterSeconds(floatingDamage, Instance.secondsToDespawn));
    }

    public static void Spawn(string text, Vector3 position)
    {
        GameObject floatingText = ObjectManager.Instance.FloatingDamage.SpawnObject(position);

        if (floatingText.TryGetComponent<TextMesh>(out TextMesh textMesh))
        {
            // Set text to the text mesh
            textMesh.text = $"{text}";
        }
        else
        {
            Debug.LogAssertion($"Failed to get TextMesh component from {floatingText.name} GameObject.");
        }

        Instance.StartCoroutine(CoroutineUtility.Instance.HideAfterSeconds(floatingText, Instance.secondsToDespawn));
    }
}
