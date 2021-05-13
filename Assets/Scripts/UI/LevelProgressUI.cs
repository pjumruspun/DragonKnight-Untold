using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField]
    private GameObject levelProgressObject;
    [SerializeField]
    private Image levelProgressFill;
    [SerializeField]
    private Text levelProgressText;
    [SerializeField]
    private string notCompletedLevelString = "Enemies Defeated";
    [SerializeField]
    private string notCompletedBossLevelString = "Defeat the boss!";
    [SerializeField]
    private string completedLevelString = "Find an exit and proceed to the next stage!";

    private void Start()
    {
        UpdateUI();
        if (StageManager.IsCampStage)
        {
            levelProgressObject.SetActive(false);
        }
        else
        {
            levelProgressObject.SetActive(true);
        }

        EventPublisher.EnemyDead += UpdateUI;
        BossEvents.BossDead += OnBossKill;
    }

    private void OnDestroy()
    {
        EventPublisher.EnemyDead -= UpdateUI;
        BossEvents.BossDead -= OnBossKill;
    }

    private void UpdateUI(Enemy enemy)
    {
        // This function is used for event delegate alone
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Real UpdateUI is here
        if (StageManager.IsBossStage)
        {
            ShowBossLevelNotCompletedText();
        }
        else
        {
            UpdateLevelProgressBar();
            UpdateLevelProgressText();
        }
    }

    private void UpdateLevelProgressBar()
    {
        if (!StageManager.IsBossStage)
        {
            levelProgressFill.fillAmount = LevelProgression.LevelCompletionPercentage;
        }
        else
        {
            throw new System.InvalidOperationException("UpdateLevelProgressBar() should not be called in boss stage");
        }
    }

    private void UpdateLevelProgressText()
    {
        if (!StageManager.IsBossStage)
        {
            float percentage = LevelProgression.LevelCompletionPercentage;
            if (percentage < 1.0f)
            {
                levelProgressText.text = $"{Mathf.Floor(percentage * 100)}% {notCompletedLevelString}";
            }
            else
            {
                levelProgressText.text = completedLevelString;
            }
        }
        else
        {
            throw new System.InvalidOperationException("UpdateLevelProgressText() should not be called in boss stage");
        }
    }

    private void ShowBossLevelNotCompletedText()
    {
        if (StageManager.IsBossStage)
        {
            levelProgressText.text = notCompletedBossLevelString;
        }
        else
        {
            throw new System.InvalidOperationException("ShowBossLevelNotCompletedText() should not be called in non-boss stage");
        }
    }

    private void OnBossKill(Boss boss)
    {
        if (StageManager.IsBossStage)
        {
            levelProgressText.text = completedLevelString;
            levelProgressFill.fillAmount = 1.0f;
        }
        else
        {
            throw new System.InvalidOperationException("OnBossKill() should not be called in non-boss stage");
        }
    }
}
