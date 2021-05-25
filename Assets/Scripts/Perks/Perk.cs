using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Perk class that will get added into PerkStatic.
/// </summary>
public class Perk
{
    public PerkType type;
    public PerkCategory category;
    public PerkTier tier;
    public Sprite icon = null;
    public string description;
    public int maxPerkLevel;
    public int perkLevel;
    public PerkUpgradeDetail[] perkUpgradeDetails;

    public Perk(
        PerkType type,
        PerkCategory category,
        PerkTier tier,
        Sprite icon,
        string description,
        int maxPerkLevel,
        PerkUpgradeDetail[] details
    )
    {
        this.type = type;
        this.category = category;
        this.tier = tier;
        this.icon = icon;
        this.description = description;
        this.maxPerkLevel = maxPerkLevel;
        this.perkLevel = 1;
        this.perkUpgradeDetails = details;
    }
}
