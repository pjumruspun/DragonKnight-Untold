using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : MonoSingleton<FloatingTextSpawner>
{
    private const float defaultSecondsToHide = 2.0f; // 2.0 seconds and the damage will despawn

    public static FloatingText SpawnDamage(float damage, Vector3 position, bool crit, float secondsToHide = defaultSecondsToHide)
    {
        position += RandomVector();
        GameObject floatingDamage = ObjectManager.Instance.FloatingDamage.SpawnObject(position);

        if (floatingDamage.TryGetComponent<FloatingText>(out var floatingText))
        {
            if (crit)
            {
                // Red text if crit
                floatingText.SetColor(new Color(0.9f, 0.1f, 0.1f));
            }

            // Set damage number to the text mesh
            string damageText = $"{Mathf.Ceil(damage)}";

            // Crit will add "!" at the end
            damageText += crit ? "!" : "";

            // Then set text
            floatingText.SetText(damageText);

            // Disable and change color back
            CoroutineUtility.ExecDelay(() => floatingDamage.SetActive(false), secondsToHide);

            return floatingText;
        }
        else
        {
            throw new System.Exception($"Failed to get FloatingText component from {floatingDamage.name} GameObject.");
        }
    }

    public static FloatingText Spawn(string text, Vector3 position, Color? color = null, float secondsToHide = defaultSecondsToHide)
    {
        position += RandomVector();
        GameObject floatingTextObj = ObjectManager.Instance.FloatingDamage.SpawnObject(position);

        if (floatingTextObj.TryGetComponent<FloatingText>(out var floatingText))
        {
            // New color if not null else white
            floatingText.SetColor(color ?? Color.white);

            // Set text to the text mesh
            floatingText.SetText($"{text}");

            CoroutineUtility.ExecDelay(() => floatingTextObj.SetActive(false), secondsToHide);
            return floatingText;
        }
        else
        {
            throw new System.Exception($"Failed to get FloatingText component from {floatingTextObj.name} GameObject.");
        }
    }

    private static Vector3 RandomVector()
    {
        float x = Random.Range(0.3f, -0.3f);
        float y = Random.Range(0.3f, -0.3f);

        return new Vector3(x, y, 0.0f);
    }
}
