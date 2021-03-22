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
}
