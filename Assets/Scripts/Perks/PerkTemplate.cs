using UnityEngine;

/// <summary>
/// This is a template using for creating Perk.
/// This won't be added into PerkStatic by any means.
/// </summary>
[CreateAssetMenu(fileName = "New Perk", menuName = "Roguelite/Perk")]
public class PerkTemplate : ScriptableObject
{
    public PerkType type;
    public PerkCategory category;
    public PerkTier tier;
    [TextArea]
    public string description = "This is default perk description";
    public Sprite icon = null;
    [SerializeField]
    public int maxPerkLevel = 5;

    public Perk CreatePerk()
    {
        return new Perk(type, category, tier, icon, description, maxPerkLevel);
    }
}
