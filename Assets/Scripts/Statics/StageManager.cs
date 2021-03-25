using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    public static bool IsBossStage => currentStage == stageCountToFightBoss + 1;
    public static float DifficultyFactor => 1.0f + (currentStage - 1) * difficultyScalingPerLevel;
    public static int CostToPassLevel => baseEnemyCostToPassLevel;

    public static int currentWorld = 1;
    public static int currentStage = 1;

    public static int currentSceneIndex;

    public static SpawnSide lastStageExitSide = SpawnSide.Left;
    public const int stageCountToFightBoss = 2;
    private const int baseEnemyCostToPassLevel = 6;
    private const float difficultyScalingPerLevel = 0.25f;
}
