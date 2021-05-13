using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulUI : MonoBehaviour
{
    [SerializeField]
    private Text soulCountText;

    private Vector3 spawnFloatingTextOffset => new Vector3(1.5f, 1.5f, 1.0f);
    private Color soulTextColor => new Color(0.1f, 0.2f, 0.9f);

    private void Start()
    {
        GameEvents.SoulChange += UpdateUI;
        GameEvents.SoulChange += SpawnSoulText;
        UpdateUI(0); // Param doesn't have any meaning here
    }

    private void OnDestroy()
    {
        GameEvents.SoulChange -= UpdateUI;
        GameEvents.SoulChange -= SpawnSoulText;
    }

    private void UpdateUI(int amount)
    {
        soulCountText.text = $"x {SoulStatic.soul}";
    }

    private void SpawnSoulText(int amount)
    {
        if (amount > 0.0f)
        {
            FloatingTextSpawner.Spawn($"+{amount} Soul", PlayerMovement.Instance.transform.position + spawnFloatingTextOffset, soulTextColor);
        }
    }
}
