using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassConfig
{
    // Contains information of each weapon
    public PlayerClass playerClass;
    public float primaryAttackDamage;
    public float primaryAttackRate = 1.0f;
    public float secondaryAttackDamage;
    public float secondaryAttackRate = 1.0f;
}
