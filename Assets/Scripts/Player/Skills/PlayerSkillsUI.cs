using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsUI : MonoBehaviour
{
    [SerializeField]
    private Image[] skillIcons;
    [SerializeField]
    private Image[] cooldownMasks;
    [SerializeField]
    private Text[] cooldownlabels;

    private void Update()
    {
        UpdateCooldownFill();
        UpdateCooldownLabel();
    }

    private void UpdateCooldownFill()
    {
        for (int i = 0; i < 4; ++i)
        {
            float cooldownPercentage = PlayerCombat.Instance.CurrentCooldownPercentage(i);
            cooldownMasks[i].fillAmount = cooldownPercentage;
        }
    }

    private void UpdateCooldownLabel()
    {
        float[] currentCooldowns = PlayerCombat.Instance.GetCurrentCooldown();
        for (int i = 0; i < 4; ++i)
        {
            if (currentCooldowns[i] < 0.01f)
            {
                // Zero, hide
                cooldownlabels[i].text = "";
                continue;
            }
            int cooldownInt = (int)Mathf.Ceil(currentCooldowns[i]);
            cooldownlabels[i].text = $"{cooldownInt}";
        }
    }
}
