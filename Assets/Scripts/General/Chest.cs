using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chest : Interactable
{
    [SerializeField]
    private Animator animator;

    public override void Interact()
    {
        base.Interact();
        OpenChest();
    }

    protected virtual void OpenChest()
    {
        animator.SetTrigger("Open");
    }
}
