using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPublisher
{
    // Player stuff
    public delegate void OnPlayerJump();
    public delegate void OnPlayerLand();
    public delegate void OnPlayerRun();
    public delegate void OnPlayerStop();
    public delegate void OnPlayerPrimaryAttack();
    public delegate void OnPlayerShapeshift();

    public static event OnPlayerJump PlayerJump;
    public static event OnPlayerLand PlayerLand;
    public static event OnPlayerRun PlayerRun;
    public static event OnPlayerStop PlayerStop;
    public static event OnPlayerPrimaryAttack PlayerPrimaryAttack;
    public static event OnPlayerShapeshift PlayerShapeshift;

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

    public static void TriggerPlayerPrimaryAttack()
    {
        PlayerPrimaryAttack?.Invoke();
    }

    public static void TriggerPlayerShapeshift()
    {
        PlayerShapeshift?.Invoke();
    }
}
