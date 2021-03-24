using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    public static int currentSceneIndex;
    public static float DifficultyFactor => 1.0f + (currentStage - 1) * difficultyScalingPerLevel;
    public static int CostToPassLevel => baseEnemyCostToPassLevel;
    public static int currentStage = 1;
    public static SpawnSide lastStageExitSide = SpawnSide.Left;
    private const int baseEnemyCostToPassLevel = 6;
    private const float difficultyScalingPerLevel = 0.25f;
}
