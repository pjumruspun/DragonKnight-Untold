using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerSkillsUI : MonoBehaviour
{
    [SerializeField]
    private Image[] skillIcons;
    [SerializeField]
    private Image[] cooldownMasks;
    [SerializeField]
    private Text[] cooldownlabels;

    private Sprite originalSkillSprite;

    private void Start()
    {
        EventPublisher.PlayerChangeClass += ChangeSkillIcons;
        EventPublisher.PlayerShapeshift += ChangeSkillIcons;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerChangeClass += ChangeSkillIcons;
        EventPublisher.PlayerShapeshift -= ChangeSkillIcons;
    }

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

    private void ChangeSkillIcons(bool isDragon)
    {
        if (isDragon)
        {
            Sprite[] icons = SkillsRepository.Dragon.GetIcons.Cast<Sprite>().ToArray();
            for (int i = 0; i < 4; ++i)
            {
                skillIcons[i].sprite = icons[i];
            }
        }
        else
        {
            ChangeSkillIcons(PlayerCombat.Instance.CurrentClass);
        }
    }

    private void ChangeSkillIcons(PlayerClass playerClass)
    {
        PlayerSkills skills = SkillsRepository.GetSkills(playerClass);
        Sprite[] icons = skills.GetIcons.Cast<Sprite>().ToArray();
        for (int i = 0; i < 4; ++i)
        {
            skillIcons[i].sprite = icons[i];
        }
    }
}
