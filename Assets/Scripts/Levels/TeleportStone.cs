using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportStone : Interactable
{
    [SerializeField]
    private Scenes sceneToLoad;
    private Dictionary<Scenes, string> sceneNames = new Dictionary<Scenes, string>()
    {
        { Scenes.Camp, "Camp" },
        { Scenes.Tutorial, "Gameplay" },
    };

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
        SceneManager.LoadScene(sceneNames[sceneToLoad]);
    }
}
