using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvents
{
    public delegate void OnBossSpawn(Boss boss);
    public delegate void OnBossHpChange(Boss boss);
    public delegate void OnBossDead(Boss boss);
    public delegate void OnBossSAChange(Boss boss);

    public static event OnBossSpawn BossSpawn;
    public static event OnBossHpChange BossHpChange;
    public static event OnBossDead BossDead;
    public static event OnBossSAChange BossSAChange;

    public static void TriggerBossSpawn(Boss boss)
    {
        BossSpawn?.Invoke(boss);
    }

    public static void TriggerBossHpChange(Boss boss)
    {
        BossHpChange?.Invoke(boss);
    }

    public static void TriggerBossDead(Boss boss)
    {
        BossDead?.Invoke(boss);
    }

    public static void TriggerBossSAChange(Boss boss)
    {
        BossSAChange?.Invoke(boss);
    }
}
