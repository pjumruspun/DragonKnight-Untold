using System.Collections.Generic;
using System.Linq;

public class PerkStatic
{
    public static int upgradeToken = 0;
    public static bool shouldRandom = true;
    public static List<Perk> perks = new List<Perk>();

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
        var query = from perk in perks where perk.type == perkType select perk;
        int count = query.Count<Perk>();

        if (count == 0)
        {
            // No perk
            return 0;
        }
        else
        {
            return query.First().perkLevel;
        }
    }
}
