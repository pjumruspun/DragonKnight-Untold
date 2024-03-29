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
    public delegate void OnPlayerTakeDamage();
    public delegate void OnDragonGaugeChange(float dragonGauge);
    public delegate void OnPlayerDead();
    public delegate void OnPlayerChangeClass(PlayerClass pc);
    public delegate void OnPlayerStatsChange();
    public delegate void OnStopChargeShot();
    public delegate void OnStopFireBreath();
    public delegate void OnStopRush();
    public delegate void OnInventoryChange();
    public delegate void OnPlayerSpawn(Transform player);
    public delegate void OnPlayerReceiveBuff(Buff buff);

    // Enemy delegate
    public delegate void OnEnemyDead(Enemy enemy);

    // Player events
    public static event OnPlayerJump PlayerJump;
    public static event OnPlayerLand PlayerLand;
    public static event OnPlayerRun PlayerRun;
    public static event OnPlayerStop PlayerStop;
    public static event OnPlayerUseSkill PlayerUseSkill;
    public static event OnPlayerShapeshift PlayerShapeshift;
    public static event OnPlayerHealthChange PlayerHealthChange;
    public static event OnPlayerTakeDamage PlayerTakeDamage;
    public static event OnDragonGaugeChange DragonGaugeChange;
    public static event OnPlayerDead PlayerDead;
    public static event OnPlayerChangeClass PlayerChangeClass;
    public static event OnPlayerStatsChange PlayerStatsChange;
    public static event OnStopChargeShot StopChargeShot;
    public static event OnStopFireBreath StopFireBreath;
    public static event OnStopRush StopRush;
    public static event OnInventoryChange InventoryChange;
    public static event OnPlayerSpawn PlayerSpawn;
    public static event OnPlayerReceiveBuff PlayerReceiveBuff;

    // Enemy event
    public static event OnEnemyDead EnemyDead;


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

    public static void TriggerPlayerTakeDamage()
    {
        PlayerTakeDamage?.Invoke();
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

    public static void TriggerStopChargeShot()
    {
        StopChargeShot?.Invoke();
    }

    public static void TriggerStopFireBreath()
    {
        StopFireBreath?.Invoke();
    }

    public static void TriggerStopRush()
    {
        StopRush?.Invoke();
    }

    public static void TriggerInventoryChange()
    {
        InventoryChange?.Invoke();
    }

    public static void TriggerEnemyDead(Enemy enemy)
    {
        EnemyDead?.Invoke(enemy);
    }

    public static void TriggerPlayerSpawn(Transform player)
    {
        PlayerSpawn?.Invoke(player);
    }

    public static void TriggerPlayerReceiveBuff(Buff buff)
    {
        PlayerReceiveBuff?.Invoke(buff);
    }
}
