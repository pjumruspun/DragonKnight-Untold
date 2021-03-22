using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Roguelite/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public ItemRarity rarity;
    public ItemStats stats;
}
