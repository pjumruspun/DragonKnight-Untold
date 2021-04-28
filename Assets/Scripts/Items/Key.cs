using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    private void RenderPickupUI()
    {
        actionText = "Pick up";
        detailText = "Treasure Chest Key";
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    private void PickUp()
    {
        bool success = KeyStatic.AddKey();

        if (success)
        {
            // Return gameObject to the pool
            transform.parent.gameObject.SetActive(false);
            GameEvents.TriggerKeyCollected();
        }
        else
        {
            FloatingTextSpawner.Spawn("You cannot hold any more key!", transform.position);
        }
    }
}
