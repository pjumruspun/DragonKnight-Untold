using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    SpriteRenderer lightFromGround;
    [SerializeField]
    private Item item;
    private Color normalItemColor = new Color(0.9f, 0.9f, 0.9f);
    private Color rareItemColor = new Color(0.0f, 0.9f, 0.1f);
    private Color ultraRareItemColor = new Color(0.1f, 0.3f, 1.0f);

    public override void Interact()
    {
        base.Interact();
        PickItem();
    }

    public void AssignItem(Item item)
    {
        this.item = item;
        lightFromGround.color = GetColor();
        EnableItem();
    }

    private Color GetColor()
    {
        switch (item.rarity)
        {
            case ItemRarity.Common:
                return normalItemColor;
            case ItemRarity.Rare:
                return rareItemColor;
            case ItemRarity.UltraRare:
                return ultraRareItemColor;
            default:
                throw new System.IndexOutOfRangeException($"Unhandled enum ItemRarity: {item.rarity}");
        }
    }

    private void Start()
    {
        EnableItem();
    }

    private void EnableItem()
    {
        RenderPickupUI();
        RenderSprite();
    }

    private void RenderPickupUI()
    {
        actionText = "Pick up";
        detailText = item.name;
    }

    private void RenderSprite()
    {
        spriteRenderer.sprite = item.icon;
    }

    private void PickItem()
    {
        // Add to player
        Inventory.Instance.Add(item);

        // Play sound
        SoundManager.Play(SFXName.ItemPickup);

        // Spawn guide text
        Debug.Log(item.stats);
        FloatingTextSpawner.Spawn(item.stats.GetDescription(), transform.position, GetColor());

        // Remove gameObject from the scene
        // Need to change to disable object pool later
        ObjectManager.Instance.ItemPickups.ReturnObject(transform.parent.gameObject);
    }
}
