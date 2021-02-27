using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerConfig
{
    // Health
    public float MaxHealth = 200.0f;
    public ClassConfig SwordConfig;
    public ClassConfig ArcherConfig;
    public DragonConfig NightDragonConfig;
    public AdditionalSkillConfigs AdditionalConfigs;
}
