using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGauge : MonoSingleton<DragonGauge>
{
    public static float CurrentShapeshiftCooldownPercentage => Instance.currentShapeshiftCooldown / shapeshiftCooldown;
    public static float CurrentShapeshiftCooldown => Instance.currentShapeshiftCooldown;
    public float MaxDragonEnergy => maxDragonEnergy;
    public bool IsDragonForm => isDragonForm;
    [SerializeField]
    private float startingDragonEnergy = 50.0f;
    [SerializeField]
    private float drainingRate = 2.0f;
    [SerializeField]
    private float regenRate = 1.0f;
    private bool isDragonForm = false;
    private const float maxDragonEnergy = 100.0f;
    private float dragonEnergy;
    private const float shapeshiftCooldown = 3.0f;
    private float currentShapeshiftCooldown = 0.0f;

    private void Start()
    {
        EventPublisher.PlayerChangeClass += StripDragonForm;
        dragonEnergy = startingDragonEnergy;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerChangeClass -= StripDragonForm;
    }

    private void Update()
    {
        if (!PlayerHealth.Instance.IsDead)
        {
            ListenToShapeshiftEvent();
            ProcessDragonEnergy();
        }

        ProcessDragonCooldown();
    }

    private void ListenToShapeshiftEvent()
    {
        if (InputManager.Shapeshift && CanShapeShift())
        {
            // Player transforms here
            ShapeShift();
            // (isDragonForm);
        }
    }

    private void ShapeShift()
    {
        if (!isDragonForm)
        {
            currentShapeshiftCooldown = shapeshiftCooldown;
        }

        isDragonForm = !isDragonForm;
        EventPublisher.TriggerPlayerShapeshift(isDragonForm);
    }

    private void ProcessDragonEnergy()
    {
        if (isDragonForm)
        {
            // Drain energy
            dragonEnergy -= drainingRate * Time.deltaTime;
            // Strip dragon form if dragon energy is empty
            if (dragonEnergy <= 0.0f)
            {
                ShapeShift();
            }
        }
        else
        {
            // Regen?
            // Could be attacking and gain energy here too
            if (dragonEnergy < maxDragonEnergy)
            {
                dragonEnergy += regenRate * Time.deltaTime;
            }
        }

        EventPublisher.TriggerDragonGaugeChange(dragonEnergy);
    }

    private bool CanShapeShift()
    {
        if (!isDragonForm)
        {
            return dragonEnergy > 0.0f && currentShapeshiftCooldown < 0.01f;
        }
        else
        {
            return true;
        }
    }

    private void StripDragonForm(PlayerClass playerClass)
    {
        // Player should not be in dragon form when changing class, just in case
        if (IsDragonForm)
        {
            // Player dragon down
            isDragonForm = false;
            EventPublisher.TriggerPlayerShapeshift(false);
        }
    }

    private void ProcessDragonCooldown()
    {
        if (currentShapeshiftCooldown > 0.0f)
        {
            currentShapeshiftCooldown -= Time.deltaTime;
        }
        else if (currentShapeshiftCooldown < 0.0f)
        {
            currentShapeshiftCooldown = 0.0f;
        }
    }
}
