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
            if (KeyStatic.numberOfKeys > 0)
            {
                base.OpenChest();
                isActive = false;
                hasBeenOpened = true;

                // Spawn item
                CoroutineUtility.ExecDelay(() => itemSpawner.SpawnItem(), spawnItemDelayTime);

                // Reduce number of key
                --KeyStatic.numberOfKeys;

                // Notify that key amount has changed
                GameEvents.TriggerKeyAmountChange();
            }
            else
            {
                // Inform player
                FloatingTextSpawner.Spawn("Insufficient Keys", transform.position);
            }
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
