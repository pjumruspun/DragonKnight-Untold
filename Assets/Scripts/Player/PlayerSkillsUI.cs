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

    private void Start()
    {

    }

    private void Update()
    {
        for (int i = 0; i < 4; ++i)
        {
            float currentCooldownPercentage = PlayerAbilities.Instance.GetCurrentSkillCooldown(i, true);
            cooldownMasks[i].fillAmount = currentCooldownPercentage;
        }
    }
}
