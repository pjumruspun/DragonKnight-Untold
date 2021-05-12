using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable focus;

    private void Start()
    {
        ClearText();
    }

    private void Update()
    {
        ListenToInteractSignal();
    }

    private void ListenToInteractSignal()
    {
        if (InputManager.Interact && focus != null)
        {
            focus.Interact();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (
            other.gameObject.layer == Layers.interactableLayerIndex
            && other.TryGetComponent<Interactable>(out Interactable interactable)
        )
        {
            if (interactable.IsActive)
            {
                SetFocus(interactable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (
            other.gameObject.layer == Layers.interactableLayerIndex
            && other.TryGetComponent<Interactable>(out Interactable interactable)
        )
        {
            ResetFocus(interactable);
        }
    }

    private void SetFocus(Interactable interactable)
    {
        focus = interactable;
        InteractionText.Instance.SetActionText(interactable.ActionText);
        InteractionText.Instance.SetDetailText(interactable.DetailText);
    }

    private void ResetFocus(Interactable interactable)
    {
        if (interactable == focus)
        {
            focus = null;
            // Debug.Log($"Stop setting focus on {interactable.gameObject.name}");
            ClearText();
        }
    }

    private void ClearText()
    {
        InteractionText.Instance.SetActionText("");
        InteractionText.Instance.SetDetailText("");
    }
}
