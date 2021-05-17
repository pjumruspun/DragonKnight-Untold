using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Roguelite/Perk")]
public class PerkTemplate : ScriptableObject
{
    public PerkType type;
    [TextArea]
    public string description = "This is default perk description";
    public int[] soulToUpgrade = { 100, 300, 500, 750, 1200 };
    public Sprite icon = null;
    public StatsDto stats;
    public int PerkLevel;
    public int Chance = 1; // Unused for now
    private const int maxPerkLevel = 5;

    public PerkTemplate(PerkTemplate perk)
    {
        this.icon = perk.icon;
        this.PerkLevel = perk.PerkLevel;
        this.Chance = perk.Chance;
        this.type = perk.type;
    }

    public void Upgrade()
    {
        if (PerkLevel < maxPerkLevel)
        {
            PerkLevel += 1;
        }
    }
}
