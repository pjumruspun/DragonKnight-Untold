using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulUI : MonoBehaviour
{
    [SerializeField]
    private Text soulCountText;

    private void Start()
    {
        GameEvents.SoulChange += UpdateUI;
    }

    private void OnDestroy()
    {
        GameEvents.SoulChange -= UpdateUI;
    }

    private void UpdateUI()
    {
        soulCountText.text = $"x {SoulStatic.soul}";
    }
}
