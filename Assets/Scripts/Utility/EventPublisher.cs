using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPublisher
{
    // Player delegates
    public delegate void OnPlayerJump();
    public delegate void OnPlayerLand();
    public delegate void OnPlayerRun();
    public delegate void OnPlayerStop();
    public delegate void OnPlayerUseSkill(int number);
    public delegate void OnPlayerShapeshift(bool isDragon);
    public delegate void OnPlayerHealthChange();
    public delegate void OnDragonGaugeChange(float dragonGauge);
    public delegate void OnPlayerDead();
    public delegate void OnPlayerChangeClass(PlayerClass pc);
    public delegate void OnPlayerStatsChange();
    public delegate void OnStopFireBreath();

    // Player events
    public static event OnPlayerJump PlayerJump;
    public static event OnPlayerLand PlayerLand;
    public static event OnPlayerRun PlayerRun;
    public static event OnPlayerStop PlayerStop;
    public static event OnPlayerUseSkill PlayerUseSkill;
    public static event OnPlayerShapeshift PlayerShapeshift;
    public static event OnPlayerHealthChange PlayerHealthChange;
    public static event OnDragonGaugeChange DragonGaugeChange;
    public static event OnPlayerDead PlayerDead;
    public static event OnPlayerChangeClass PlayerChangeClass;
    public static event OnPlayerStatsChange PlayerStatsChange;
    public static event OnStopFireBreath StopFireBreath;


    public static void TriggerPlayerJump()
    {
        PlayerJump?.Invoke();
    }

    public static void TriggerPlayerLand()
    {
        PlayerLand?.Invoke();
    }

    public static void TriggerPlayerRun()
    {
        PlayerRun?.Invoke();
    }

    public static void TriggerPlayerStop()
    {
        PlayerStop?.Invoke();
    }

    public static void TriggerPlayerUseSkill(int number)
    {
        PlayerUseSkill?.Invoke(number);
    }

    public static void TriggerPlayerShapeshift(bool isDragon)
    {
        PlayerShapeshift?.Invoke(isDragon);
    }

    public static void TriggerPlayerHealthChange()
    {
        PlayerHealthChange?.Invoke();
    }

    public static void TriggerDragonGaugeChange(float dragonGauge)
    {
        DragonGaugeChange?.Invoke(dragonGauge);
    }

    public static void TriggerPlayerDead()
    {
        PlayerDead?.Invoke();
    }

    public static void TriggerPlayerChangeClass(PlayerClass playerClass)
    {
        PlayerChangeClass?.Invoke(playerClass);
    }

    public static void TriggerPlayerStatsChange()
    {
        PlayerStatsChange?.Invoke();
    }

    public static void TriggerStopFireBreath()
    {
        StopFireBreath?.Invoke();
    }
}
