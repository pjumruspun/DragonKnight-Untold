using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportStone : Interactable
{
    [SerializeField]
    private Scenes sceneToLoad;

    public override void Interact()
    {
        base.Interact();
        Teleport();
    }

    private void Start()
    {
        RenderInterationUI();
    }

    private void RenderInterationUI()
    {
        actionText = "Teleport";
        detailText = sceneToLoad == Scenes.Camp ? "Camp" : "Next Level";
    }

    private void Teleport()
    {
        LevelChanger.LoadScene(sceneToLoad);
    }
}
