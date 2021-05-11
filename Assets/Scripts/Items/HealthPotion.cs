using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Interactable
{
    [SerializeField]
    private float healAmount = 20.0f;

    private void Start()
    {
        RenderPickupUI();
    }

    private void RenderPickupUI()
    {
        actionText = "Consume";
        detailText = "Health Potion";
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    private void PickUp()
    {
        bool success = !PlayerHealth.Instance.IsAtMaxHP;

        if (success)
        {
            // Return gameObject to the pool
            transform.parent.gameObject.SetActive(false);
            PlayerHealth.Instance.Heal(healAmount);
        }
        else
        {
            FloatingTextSpawner.Spawn("You cannot hold any more key!", transform.position);
        }
    }
}

