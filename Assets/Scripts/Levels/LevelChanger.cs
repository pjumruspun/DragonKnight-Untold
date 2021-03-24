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
    [SerializeField]
    private string levelNamePrefix = "Level_";
    [SerializeField]
    private int levelCount = 1;
    private Animator animator;
    private bool isTransitioning = false;


    private static readonly Dictionary<Scenes, string> sceneNames = new Dictionary<Scenes, string>()
    {
        { Scenes.Camp, "Camp" },
        { Scenes.Tutorial, "Gameplay" },
        { Scenes.MainMenu, "MainMenu" },
    };

    public static void LoadRandomLevel()
    {
        int random = Random.Range(1, Instance.levelCount + 1);
        while (random == StageManager.currentSceneIndex)
        {
            // Random until gets other scene
            random = Random.Range(1, Instance.levelCount + 1);
        }

        string nameOfSceneToLoad = Instance.levelNamePrefix + random.ToString();

        if (!Instance.isTransitioning)
        {
            Instance.isTransitioning = true;
            LevelChanger.Instance.FadeOut();
            CoroutineUtility.ExecDelay(() =>
            {

                SceneManager.LoadScene(nameOfSceneToLoad);
                Instance.isTransitioning = false;
                GameEvents.TriggerPause(false);
            }, Instance.sceneTransitionTime);
        }
    }

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
                GameEvents.TriggerPause(false);
            }, Instance.sceneTransitionTime);
        }
    }

    public static void LoadSceneInstant(Scenes scene)
    {
        SceneManager.LoadScene(sceneNames[scene]);
        GameEvents.TriggerPause(false);
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
        animator.SetTrigger("FadeOut");
    }

    private void FadeIn(Scene scene, LoadSceneMode mode)
    {
        animator.SetTrigger("FadeIn");
    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string[] words = currentSceneName.Split('_');
        int index = int.Parse(words[words.Length - 1]);
        StageManager.currentSceneIndex = index;
    }
}
