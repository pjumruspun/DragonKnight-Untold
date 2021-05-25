using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUpgradePotion : Interactable
{
    private void Start()
    {
        RenderPickupUI();
    }

    private void RenderPickupUI()
    {
        actionText = "Consume";
        detailText = "Perk Upgrade Potion";
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    private void PickUp()
    {
        PerkStatic.upgradeToken += 1;

        // Return gameObject to the pool
        transform.parent.gameObject.SetActive(false);
    }
}
