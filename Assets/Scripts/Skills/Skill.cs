using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Roguelite/Skill")]
public class Skill : ScriptableObject
{
    public string skillName = "New Skill";
    public Sprite skillIcon = null;
    public float baseDamage = 0.0f;
    public float baseCooldown = 1.0f;
}
