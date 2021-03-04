using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : Interactable
{
    [SerializeField]
    private Item item;

    public override void Interact()
    {
        base.Interact();
        PickItem();
    }

    private void Start()
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
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }

    private void PickItem()
    {
        // Add to player
        Inventory.Instance.Add(item);

        // Remove gameObject from the scene
        // Need to change to disable object pool later
        Destroy(gameObject);
    }
}
