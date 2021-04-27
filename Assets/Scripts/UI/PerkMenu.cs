using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject perkMenuObject;

    private void Start()
    {
        perkMenuObject.SetActive(false);
    }

    private void Update()
    {
        ListenToPerkMenuInput();
    }

    private void ListenToPerkMenuInput()
    {
        if (InputManager.PerkMenu)
        {
            bool shouldActive = !perkMenuObject.activeInHierarchy;
            perkMenuObject.SetActive(shouldActive);
            Debug.Log($"Set active {shouldActive}");
        }
    }
}
