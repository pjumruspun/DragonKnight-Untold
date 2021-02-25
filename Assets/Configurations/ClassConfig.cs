using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassConfig
{
    // Contains information of each weapon
    public PlayerClass playerClass;
    public float[] skillDamage = new float[4] { 20.0f, 20.0f, 20.0f, 20.0f };
    public float[] skillCooldown = new float[4] { 1.0f, 1.0f, 1.0f, 1.0f };
}