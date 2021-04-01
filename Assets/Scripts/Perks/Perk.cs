using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Roguelite/Perk")]
public class Perk : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public ItemStats stats;
    public int PerkLevel = 1;
    public int RandomRate = 1;
    public PerkType type;

    public void upgrade()
    {
        if (PerkLevel < 10) 
        {
            PerkLevel += 1;
        }
    }
}
