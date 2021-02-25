using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DragonConfig
{
    public float[] dragonAttackDamage = new float[4] { 30.0f, 30.0f, 30.0f, 30.0f };
    public float[] dragonAttackCooldown = new float[4] { 1.0f, 1.0f, 1.0f, 1.0f };
    public int jumpCount = 3;
}
