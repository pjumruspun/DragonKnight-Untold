using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PerkDisplay : MonoBehaviour
{
    [SerializeField]
    private Image perkIcon;

    [SerializeField]
    private TextMeshProUGUI perkName;

    [SerializeField]
    private TextMeshProUGUI perkDescription;

    [SerializeField]
    private TextMeshProUGUI perkCategory;

    [SerializeField]
    private TextMeshProUGUI perkTier;

    [SerializeField]
    private TextMeshProUGUI perkLevel;

    // Parameters
    private const int lvFontSize = 16;

    private Dictionary<PerkTier, string> colorCodes = new Dictionary<PerkTier, string>()
    {
        { PerkTier.S, "10A0FF" },
        { PerkTier.A, "10F010" },
        { PerkTier.B, "FFFFFF" },
    };

    public void SetPerk(Perk perk)
    {
        SetProperties(perk.icon, perk.type, perk.description, perk.category, perk.tier, perk.perkLevel);
    }

    private void SetProperties(Sprite icon, PerkType name, string description, PerkCategory category, PerkTier tier, int level)
    {
        SetIcon(icon);
        SetName(name);
        SetDescription(description);
        SetCategory(category);
        SetTier(tier);
        SetLevel(level);
    }

    private void SetIcon(Sprite icon)
    {
        if (icon != null)
        {
            perkIcon.sprite = icon;
        }
    }

    private void SetName(PerkType name)
    {
        perkName.text = name.ToString();
    }

    private void SetDescription(string description)
    {
        perkDescription.text = description;
    }

    private void SetCategory(PerkCategory category)
    {
        perkCategory.text = "Type: " + category.ToString();
    }

    private void SetTier(PerkTier tier)
    {
        perkTier.text = tier.ToString().Color(colorCodes[tier]);
    }

    private void SetLevel(int level)
    {
        perkLevel.text = "Lv".Size(16) + level.ToString();
    }
}
