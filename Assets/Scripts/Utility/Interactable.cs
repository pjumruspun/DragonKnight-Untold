using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public bool IsActive => isActive;
    public string ActionText => actionText + " [F]";
    public string DetailText => detailText;

    protected string actionText = "Interact";
    protected string detailText = "An Interactable";
    protected bool isActive = true;

    public virtual void Interact()
    {
        // Debug.Log("Player interacted with " + transform.name);
    }
}
