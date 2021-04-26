using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Roguelite/Perk")]
public class Perk : ScriptableObject
{
    new public string name = "New Perk";
    public Sprite icon = null;
    public ItemStats stats;
    public int PerkLevel = 1;
    public int Chance = 1;
    public PerkType type;
    private const int maxPerkLevel = 5;

    public void Upgrade()
    {
        if (PerkLevel < maxPerkLevel) 
        {
            PerkLevel += 1;
        }
    }
}
