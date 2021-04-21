using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoSingleton<StageUI>
{
    [SerializeField]
    private Text stageText;

    private void Start()
    {
        RenderStageUI();
    }

    private void RenderStageUI()
    {
        if (StageManager.IsCampStage)
        {
            stageText.text = $"Next Stage: {StageManager.currentWorld}-{StageManager.currentStage}";
        }
        else
        {
            stageText.text = $"Stage {StageManager.currentWorld}-{StageManager.currentStage}";
        }
    }
}
