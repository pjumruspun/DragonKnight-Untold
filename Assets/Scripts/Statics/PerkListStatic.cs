using System.Collections.Generic;

public class PerkListStatic
{
    public static bool shouldRandom = true;
    public static List<PerkTemplate> perks = new List<PerkTemplate>();

    public static bool HasPerk(PerkType perkType)
    {
        foreach (var perk in perks)
        {
            if (perk.type == perkType)
            {
                return true;
            }
        }

        return false;
    }

    public static int GetPerkLevel(PerkTemplate targetPerk)
    {
        return GetPerkLevel(targetPerk.type);
    }

    public static int GetPerkLevel(PerkType perkType)
    {
        foreach (var perk in PerkListStatic.perks)
        {
            if (perk.type == perkType)
            {
                return perk.PerkLevel;
            }
        }

        return 0;
    }
}
