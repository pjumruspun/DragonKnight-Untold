using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemSpawner))]
public class ItemChest : Chest
{
    private ItemSpawner itemSpawner;
    private const float spawnItemDelayTime = 1.1f;
    private bool hasBeenOpened = false;

    protected override void OpenChest()
    {
        if (!hasBeenOpened)
        {
            base.OpenChest();
            isActive = false;
            // Spawn item
            CoroutineUtility.ExecDelay(() => itemSpawner.SpawnItem(), spawnItemDelayTime);
        }
    }

    private void RenderPickupUI()
    {
        actionText = "Open";
        detailText = "Item Chest";
        // Need to change text after open somehow
    }

    private void Start()
    {
        itemSpawner = GetComponent<ItemSpawner>();
        RenderPickupUI();
    }
}
