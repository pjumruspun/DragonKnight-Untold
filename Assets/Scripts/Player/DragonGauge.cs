using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGauge : MonoSingleton<DragonGauge>
{
    public float MaxDragonEnergy => maxDragonEnergy;
    public bool IsDragonForm => isDragonForm;

    [SerializeField]
    private float drainingRate = 2.0f;
    [SerializeField]
    private float regenRate = 1.0f;
    private bool isDragonForm = false;
    private const float maxDragonEnergy = 100.0f;
    private float dragonEnergy;

    private void Start()
    {
        EventPublisher.PlayerChangeClass += StripDragonForm;
        dragonEnergy = 95.0f;
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
    }

    private void ListenToShapeshiftEvent()
    {
        if (InputManager.Shapeshift && CanShapeShift())
        {
            // Player transforms here
            ShapeShift();
        }
    }

    private void ShapeShift()
    {
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
        return dragonEnergy > 0.0f;
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
}
