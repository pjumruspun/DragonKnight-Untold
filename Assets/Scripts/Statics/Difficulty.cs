using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty
{
    public static float EnemyHealthScalingFactor =>
        1.0f + ((StageManager.currentWorld - 1) * enemyHealthScalingPerWorld) + ((StageManager.currentStage - 1) * enemyHealthScalingPerLevel);
    public static float EnemyAttackScalingFactor =>
        1.0f + ((StageManager.currentWorld - 1) * enemyAttackScalingPerWorld) + ((StageManager.currentStage - 1) * enemyAttackScalingPerLevel);

    private const float enemyHealthScalingPerWorld = 1.25f;
    private const float enemyHealthScalingPerLevel = 0.25f;
    private const float enemyAttackScalingPerWorld = 1.00f;
    private const float enemyAttackScalingPerLevel = 0.20f;
}
