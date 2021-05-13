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
    [SerializeField]
    private Sprite originalSkillSprite;

    private List<TooltipTrigger> tooltips = new List<TooltipTrigger>();

    private void Start()
    {
        EventPublisher.PlayerChangeClass += ChangeSkillIcons;
        EventPublisher.PlayerShapeshift += ChangeSkillIcons;
        EventPublisher.PlayerStatsChange += UpdateTooltips;

        for (int i = 0; i < skillIcons.Length; ++i)
        {
            Image icon = skillIcons[i];
            TooltipTrigger tooltipTrigger = icon.gameObject.AddComponent<TooltipTrigger>();
            tooltips.Add(tooltipTrigger);
        }

        UpdateTooltips();
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerChangeClass -= ChangeSkillIcons;
        EventPublisher.PlayerShapeshift -= ChangeSkillIcons;
        EventPublisher.PlayerStatsChange -= UpdateTooltips;
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
                if (skillIcons[i] != null)
                {
                    if (icons[i] != null)
                    {
                        skillIcons[i].sprite = icons[i];
                    }
                    else
                    {
                        skillIcons[i].sprite = originalSkillSprite;
                    }
                }
            }
        }
        else
        {
            ChangeSkillIcons(PlayerClassStatic.currentClass);
        }
    }

    private void ChangeSkillIcons(PlayerClass playerClass)
    {
        PlayerSkills skills = SkillsRepository.GetSkills(playerClass);
        Sprite[] icons = skills.GetIcons.Cast<Sprite>().ToArray();
        for (int i = 0; i < 4; ++i)
        {
            if (skillIcons[i] != null)
            {
                if (icons[i] != null)
                {
                    skillIcons[i].sprite = icons[i];
                }
                else
                {
                    skillIcons[i].sprite = originalSkillSprite;
                }
            }
        }

        UpdateTooltipsOnClassChange(playerClass);
    }

    private void UpdateTooltips()
    {
        List<Skill> skills = SkillsRepository.GetSkillsWithDragon(PlayerClassStatic.currentClass).GetSkills.Cast<Skill>().ToList<Skill>();
        float[] skillCooldowns = PlayerStats.Instance.SkillCooldown;

        for (int i = 0; i < tooltips.Count; ++i)
        {
            string header = "";
            switch (i)
            {
                case 0:
                    header += $"<color=#FEAE10>[Space Bar]</color> {skills[i].skillName}";
                    Debug.Log(header);
                    break;
                case 1:
                    header += $"<color=#FEAE10>[Z]</color> {skills[i].skillName}";
                    break;
                case 2:
                    header += $"<color=#FEAE10>[X]</color> {skills[i].skillName}";
                    break;
                case 3:
                    header += $"<color=#FEAE10>[C]</color> {skills[i].skillName}";
                    break;
            }

            string content = skills[i].description;
            content += $"\n\n Cooldown: {skillCooldowns[i]} seconds";
            tooltips[i].SetText(content, header);
        }
    }

    private void UpdateTooltipsOnClassChange(PlayerClass playerClass)
    {
        List<Skill> skills = SkillsRepository.GetSkillsWithDragon(playerClass).GetSkills.Cast<Skill>().ToList<Skill>();
        float[] skillCooldowns = PlayerStats.Instance.SkillCooldown;

        for (int i = 0; i < tooltips.Count; ++i)
        {
            string header = "";
            switch (i)
            {
                case 0:
                    header += $"<color=#FEAE10>[Space Bar]</color> {skills[i].skillName}";
                    Debug.Log(header);
                    break;
                case 1:
                    header += $"<color=#FEAE10>[Z]</color> {skills[i].skillName}";
                    break;
                case 2:
                    header += $"<color=#FEAE10>[X]</color> {skills[i].skillName}";
                    break;
                case 3:
                    header += $"<color=#FEAE10>[C]</color> {skills[i].skillName}";
                    break;
            }

            string content = skills[i].description;
            content += $"\n\n Cooldown: {skillCooldowns[i]} seconds";
            tooltips[i].SetText(content, header);
        }
    }
}
