using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    private List<Buff> buffs;

    public BuffManager()
    {
        buffs = new List<Buff>();
    }

    public void UpdateBuffManager()
    {
        for(int i = buffs.Count - 1; i >= 0; --i)
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

    public void AddSwordUltBuff()
    {
        AddBuff(new SwordUltBuff(3.0f));
    }

    private void AddBuff(Buff buff)
    {
        buffs.Add(buff);
    }
}
