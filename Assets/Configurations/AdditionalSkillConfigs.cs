using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdditionalSkillConfigs
{
    public float SwordSkill1LockMovementTime = 0.2f;
    public float SwordSkill2DelayTime = 0.5f;
    public float SwordSkill2LockMovementTime = 1.0f;
    public int ArcherSkill2ArrowCount = 10;
    public float ArcherSkill2Interval = 0.02f;
    public float ArcherSkill2LockMovementTime = 0.5f;
    public Vector2 ArcherSkill2ForceVector;
}
