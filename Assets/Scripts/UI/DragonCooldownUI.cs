using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonCooldownUI : MonoSingleton<DragonCooldownUI>
{
    [SerializeField]
    private Image dragonCooldownMask;
    [SerializeField]
    private Text cooldownTimer;

    private void Update()
    {
        UpdateCooldownFill();
        UpdateCooldownTimer();
    }

    private void UpdateCooldownFill()
    {
        dragonCooldownMask.fillAmount = DragonGauge.CurrentShapeshiftCooldownPercentage;
    }

    private void UpdateCooldownTimer()
    {
        float currentCooldown = DragonGauge.CurrentShapeshiftCooldown;
        if (currentCooldown < 0.01f)
        {
            // Zero, hide
            cooldownTimer.text = "";
        }
        else
        {
            cooldownTimer.text = $"{Mathf.Ceil(currentCooldown)}";
        }
    }
}
