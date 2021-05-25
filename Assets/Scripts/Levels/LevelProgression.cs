using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgression : MonoSingleton<LevelProgression>
{
    public static bool MetCriteriaToCompleteLevel => Instance.enemyCostKilledThisStage >= StageManager.CostToPassLevel;
    public static float LevelCompletionPercentage => Mathf.Min(1.0f, Instance.enemyCostKilledThisStage / StageManager.CostToPassLevel);
    private float enemyCostKilledThisStage = 0;
    private bool hasCompletedLevel = false;

    private void OnEnable()
    {
        GameEvents.MoveToNextLevel += IncreaseStage;
        EventPublisher.EnemyDead += ProcessSpawnAmount;
    }

    private void OnDestroy()
    {
        GameEvents.MoveToNextLevel -= IncreaseStage;
        EventPublisher.EnemyDead -= ProcessSpawnAmount;
    }

    private void IncreaseStage()
    {
        if (SceneManager.GetActiveScene().name != LevelChanger.SceneNames[Scenes.Camp])
        {
            if (!StageManager.IsBossStage)
            {
                StageManager.currentStage += 1;
            }
            else
            {
                // Reset stage but increase world
                StageManager.currentStage = 1;
                StageManager.currentWorld += 1;
            }
        }
    }

    private void ProcessSpawnAmount(Enemy enemy)
    {
        enemyCostKilledThisStage += enemy.SpawnCost;

        // For normal stages, killing more than threshold should let the player pass level
        if (!hasCompletedLevel && MetCriteriaToCompleteLevel)
        {
            GameEvents.TriggerCompleteLevel();
            OnKillingLastEnemy(enemy);
            hasCompletedLevel = true;
        }

        // For boss stage, boss should be defeated, which can be found in boss script itself
    }

    private void OnKillingLastEnemy(Enemy enemy)
    {
        ObjectManager.Instance.PerkUpgradePotion.SpawnObject(enemy.transform.position);
    }
}
