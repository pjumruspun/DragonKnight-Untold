using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : MonoSingleton<FloatingTextSpawner>
{
    [SerializeField]
    private float secondsToDespawn = 2.0f; // 2.0 seconds and the damage will despawn

    public static Float Spawn(float damage, Vector3 position, bool crit)
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
            // Ensure white color
            // Please fix this later
            textMesh.color = Color.white;

            // Set damage number to the text mesh
            textMesh.text = $"{Mathf.Ceil(damage)}";

            // Crit will add "!" at the end
            textMesh.text += crit ? "!" : "";

            // Disable and change color back
            CoroutineUtility.ExecDelay(() =>
            {
                floatingDamage.SetActive(false);
            }, Instance.secondsToDespawn);

            Float floatingComponent = floatingDamage.GetComponent<Float>();
            return floatingComponent;
        }
        else
        {
            throw new System.Exception($"Failed to get TextMesh component from {floatingDamage.name} GameObject.");
        }
    }

    public static Float Spawn(string text, Vector3 position, bool green = false)
    {
        GameObject floatingText = ObjectManager.Instance.FloatingDamage.SpawnObject(position);

        if (floatingText.TryGetComponent<TextMesh>(out TextMesh textMesh))
        {
            // Ensure the color is original
            if (!green)
            {
                textMesh.color = Color.white;
            }
            else
            {
                textMesh.color = new Color(0.2f, 0.9f, 0.2f);
            }

            // Set text to the text mesh
            textMesh.text = $"{text}";
        }
        else
        {
            Debug.LogAssertion($"Failed to get TextMesh component from {floatingText.name} GameObject.");
        }

        Instance.StartCoroutine(CoroutineUtility.Instance.HideAfterSeconds(floatingText, Instance.secondsToDespawn));
        Float floatingComponent = floatingText.GetComponent<Float>();
        return floatingComponent;
    }
}
