using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public string ActionText => actionText + " [F]";
    public string DetailText => detailText;

    protected string actionText = "Interact";
    protected string detailText = "An Interactable";

    public virtual void Interact()
    {
        // Debug.Log("Player interacted with " + transform.name);
    }
}
