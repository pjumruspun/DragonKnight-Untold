using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Text detailText;
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
        if (InputManager.Interact)
        {
            focus.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.gameObject.layer == Layers.interactableLayerIndex
            && other.TryGetComponent<Interactable>(out Interactable interactable)
        )
        {
            SetFocus(interactable);
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
        this.actionText.text = interactable.ActionText;
        this.detailText.text = interactable.DetailText;
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
        this.actionText.text = "";
        this.detailText.text = "";
    }
}
