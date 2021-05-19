using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUpgradeStation : Interactable
{
    public override void Interact()
    {
        PerkUpgradeMenu.Activate();
    }

    private void Start()
    {
        SetText();
    }

    private void SetText()
    {
        actionText = "Choose a perk";
        detailText = "";
    }
}
