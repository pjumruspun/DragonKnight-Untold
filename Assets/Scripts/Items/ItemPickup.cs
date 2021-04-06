using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    private Item item;

    public override void Interact()
    {
        base.Interact();
        PickItem();
    }

    public void AssignItem(Item item)
    {
        this.item = item;
        EnableItem();
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

        // Remove gameObject from the scene
        // Need to change to disable object pool later
        ObjectManager.Instance.ItemPickups.ReturnObject(gameObject);
    }
}
