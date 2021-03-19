using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class LevelChanger : MonoSingleton<LevelChanger>
{
    public static Dictionary<Scenes, string> SceneNames => sceneNames;

    [SerializeField]
    private float sceneTransitionTime = 1.0f;
    private Animator animator;
    private bool isTransitioning = false;

    private static readonly Dictionary<Scenes, string> sceneNames = new Dictionary<Scenes, string>()
    {
        { Scenes.Camp, "Camp" },
        { Scenes.Tutorial, "Gameplay" },
        { Scenes.MainMenu, "MainMenu" },
    };

    public static void LoadScene(Scenes scene)
    {
        if (!Instance.isTransitioning)
        {
            Instance.isTransitioning = true;
            LevelChanger.Instance.FadeOut();
            Debug.Log(Instance.sceneTransitionTime);
            CoroutineUtility.ExecDelay(() =>
            {
                SceneManager.LoadScene(sceneNames[scene]);
                Instance.isTransitioning = false;
            }, Instance.sceneTransitionTime);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeIn;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FadeIn;
    }

    public void FadeOut()
    {
        Debug.Log("Fade out");
        animator.SetTrigger("FadeOut");
    }

    private void FadeIn(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("FAde in");
        animator.SetTrigger("FadeIn");
    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
}
