using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGauge : MonoSingleton<DragonGauge>
{
    public static float CurrentShapeshiftCooldownPercentage => Instance.currentShapeshiftCooldown / shapeshiftCooldown;
    public static float CurrentShapeshiftCooldown => Instance.currentShapeshiftCooldown;
    public float MaxDragonEnergy => maxDragonEnergy;
    public bool IsDragonForm => isDragonForm;
    private const float startingDragonEnergy = 100.0f;
    [SerializeField]
    private float drainingRate = 2.0f;
    [SerializeField]
    private float regenRate = 1.0f;
    private bool isDragonForm = false;
    private const float maxDragonEnergy = 100.0f;
    private const float shapeshiftCooldown = 3.0f;
    private float currentShapeshiftCooldown = 0.0f;

    private void Start()
    {
        EventPublisher.PlayerChangeClass += StripDragonForm;
        GameEvents.RestartGame += ResetDragonEnergy;

        if (DragonGaugeStatic.dragonEnergy < 0.0f)
        {
            // Because dragonEnergy is set to -1.0f by default
            // This means we only set dragonEnergy to startingDragonEnergy when initializing
            DragonGaugeStatic.dragonEnergy = startingDragonEnergy;
        }
    }

    private void ResetDragonEnergy()
    {
        DragonGaugeStatic.dragonEnergy = startingDragonEnergy;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerChangeClass -= StripDragonForm;
        GameEvents.RestartGame -= ResetDragonEnergy;
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
            DragonGaugeStatic.dragonEnergy -= drainingRate * Time.deltaTime;
            // Strip dragon form if dragon energy is empty
            if (DragonGaugeStatic.dragonEnergy <= 0.0f)
            {
                ShapeShift();
            }
        }
        else
        {
            // Regen?
            // Could be attacking and gain energy here too
            if (DragonGaugeStatic.dragonEnergy < maxDragonEnergy)
            {
                DragonGaugeStatic.dragonEnergy += regenRate * Time.deltaTime;
            }
        }

        EventPublisher.TriggerDragonGaugeChange(DragonGaugeStatic.dragonEnergy);
    }

    private bool CanShapeShift()
    {
        if (!isDragonForm)
        {
            return DragonGaugeStatic.dragonEnergy > 0.0f && currentShapeshiftCooldown < 0.01f && !PlayerCombat.Instance.IsCastingSkill;
        }
        else
        {
            return !PlayerCombat.Instance.IsCastingSkill;
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
