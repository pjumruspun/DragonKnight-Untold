using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillConfig
{
    public string skillName = "Default Skill Name";
    public float skillDamage = 20.0f;
    public float skillCooldown = 1.0f;
    public Image icon;
}