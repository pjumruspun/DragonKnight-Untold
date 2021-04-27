using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitEffect
{
    None,
    Slash
}

public class HitEffectUtility
{
    public static Dictionary<HitEffect, System.Action<Vector3>> HitEffectFunction = new Dictionary<HitEffect, System.Action<Vector3>>
    {
        { HitEffect.None, null },
        { HitEffect.Slash, ObjectManager.Instance.SpawnSlashingEffect },
    };
}
