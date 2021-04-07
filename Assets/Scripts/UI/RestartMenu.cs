using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenu : MonoSingleton<RestartMenu>
{
    [SerializeField]
    private GameObject restartMenu;

    private void Start()
    {
        restartMenu.SetActive(false);
        EventPublisher.PlayerDead += ShowRestartMenu;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerDead -= ShowRestartMenu;
    }

    private void ShowRestartMenu()
    {
        restartMenu.SetActive(true);
    }
}
