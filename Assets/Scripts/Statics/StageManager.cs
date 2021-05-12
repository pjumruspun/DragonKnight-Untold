using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageManager
{
    public static bool IsBossStage => currentStage == stageCountToFightBoss + 1;
    public static bool IsCampStage => SceneManager.GetActiveScene().name == LevelChanger.SceneNames[Scenes.Camp];
    public static int CostToPassLevel => baseEnemyCostToPassLevel;

    public static int currentWorld = 1;
    public static int currentStage = 1;

    public static int currentSceneIndex;

    public static SpawnSide lastStageExitSide = SpawnSide.Left;
    public const int stageCountToFightBoss = 3; // How many normal stages before we fight boss?
    private const int baseEnemyCostToPassLevel = 50;
}
