using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoSingleton<KeyUI>
{
    [SerializeField]
    private Text keyCount;

    private void Start()
    {
        SetText(KeyStatic.numberOfKeys, KeyStatic.maxKeyCount);
        GameEvents.KeyCollected += UpdateUI;
    }

    private void OnDestroy()
    {
        GameEvents.KeyCollected -= UpdateUI;
    }

    private void UpdateUI()
    {
        SetText(KeyStatic.numberOfKeys, KeyStatic.maxKeyCount);
    }

    private void SetText(int currentKeyCount, int maxKeyCount)
    {
        keyCount.text = $"x {currentKeyCount}";
    }
}
