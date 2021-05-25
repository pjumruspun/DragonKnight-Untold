using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsRepository : MonoSingleton<SkillsRepository>
{
    public static SwordSkills Sword => Instance.swordSkills;
    public static ArcherSkills Archer => Instance.archerSkills;
    public static DragonSkills Dragon => Instance.dragonSkills;

    [SerializeField]
    private SwordSkills swordSkills;
    [SerializeField]
    private ArcherSkills archerSkills;
    [SerializeField]
    private DragonSkills dragonSkills;

    public static PlayerSkills GetSkillsWithDragon(PlayerClass playerClass)
    {
        if (DragonGauge.Instance.IsDragonForm)
        {
            return Dragon;
        }
        else
        {
            return GetSkills(playerClass);
        }
    }

    public static PlayerSkills GetSkills(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Sword:
                return Sword;
            case PlayerClass.Archer:
                return Archer;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
