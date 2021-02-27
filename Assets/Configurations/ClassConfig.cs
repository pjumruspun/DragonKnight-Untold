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
    public int atk = 10;
    public int agi = 10;
    public int vit = 10;
    public int tal = 10;
    public int luk = 10;
    public float baseMoveSpeed = 3.0f;
    public float baseHealthRegen = 1.0f;
}