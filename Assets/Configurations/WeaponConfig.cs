using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponConfig
{
    // Contains information of each weapon
    public WeaponType Type;
    public float primaryAttackDamage;
    public float primaryAttackRate = 1.0f;
    public float secondaryAttackDamage;
    public float secondaryAttackRate = 1.0f;
}
