using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    public static IEnumerable<Buff> Buffs => buffs;
    private static List<Buff> buffs = new List<Buff>();

    public static void UpdateBuffManager()
    {
        for (int i = buffs.Count - 1; i >= 0; --i)
        {
            buffs[i].Update();
            if (buffs[i].IsExpired)
            {
                // Safely remove while iterating
                buffs.RemoveAt(i);

            }
        }

        // Debug.Log(buffs.Count);
    }

    public static void AddBuff(Buff buff)
    {
        buffs.Add(buff);
        EventPublisher.TriggerPlayerReceiveBuff(buff);
    }
}
