using System.Collections.Generic;

public class PerkListStatic
{
    public static List<Perk> perks = new List<Perk>();

    public static bool HasPerk(string perkName)
    {
        foreach (var perk in perks)
        {
            if (perk.name == perkName)
            {
                return true;
            }
        }

        return false;
    }

    public static int GetPerkLevel(Perk targetPerk)
    {
        return GetPerkLevel(targetPerk.name);
    }

    public static int GetPerkLevel(string perkName)
    {
        foreach (var perk in PerkListStatic.perks)
        {
            if (perk.name == perkName)
            {
                return perk.PerkLevel;
            }
        }

        return 0;
    }
}
