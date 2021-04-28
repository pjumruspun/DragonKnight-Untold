using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Roguelite/Perk")]
public class Perk : ScriptableObject
{
    new public string name = "New Perk";
    public string description;
    public int[] soulToUpgrade = { 100, 300, 500, 750, 1200 };
    public Sprite icon = null;
    public StatsDto stats;
    public int PerkLevel;
    public int Chance = 1;
    public PerkType type;
    private const int maxPerkLevel = 5;

    public Perk(Perk perk)
    {
        this.name = perk.name;
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
