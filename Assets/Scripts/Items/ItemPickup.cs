using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void RenderPickupUI()
    {
        actionText = "Pick up";
        detailText = item.name;
    }

    private void PickItem()
    {
        Debug.Log("Picking up an item.");
        // Add to player
        Inventory.Instance.Add(item);

        // Remove gameObject from the scene
        // Need to change to disable object pool later
        Destroy(gameObject);
    }
}
