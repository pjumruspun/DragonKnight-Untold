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
    private int normalLevelCount = 2;
    [SerializeField]
    private string bossLevelNamePrefix = "Boss_";
    [SerializeField]
    private int bossLevelCount = 1;
    private Animator animator;
    private bool isTransitioning = false;


    private static readonly Dictionary<Scenes, string> sceneNames = new Dictionary<Scenes, string>()
    {
        { Scenes.Camp, "Camp" },
        { Scenes.Tutorial, "Gameplay" },
        { Scenes.MainMenu, "MainMenu" },
        { Scenes.TestScene, "Test Level" },
    };

    public static void LoadNextLevel()
    {
        int random = -1;
        string nameOfSceneToLoad = "";

        if (StageManager.currentStage == StageManager.stageCountToFightBoss)
        {
            // Time to fight boss
            // Load Random Boss scene
            random = Random.Range(1, Instance.bossLevelCount + 1);
            nameOfSceneToLoad += Instance.bossLevelNamePrefix + random.ToString();
        }
        else if (StageManager.IsBossStage)
        {
            // Just killed boss
            // Load camp
            nameOfSceneToLoad += sceneNames[Scenes.Camp];
        }
        else
        {
            Debug.Log($"loading anything that is not {StageManager.currentSceneIndex}");
            while (random <= -1 || random == StageManager.currentSceneIndex)
            {
                // Random until gets other scene
                random = Random.Range(1, Instance.normalLevelCount);
            }

            nameOfSceneToLoad += Instance.levelNamePrefix + random.ToString();
        }

        if (!Instance.isTransitioning)
        {
            Instance.isTransitioning = true;
            LevelChanger.Instance.FadeOut();
            CoroutineUtility.ExecDelay(() =>
            {
                GameEvents.TriggerMoveToNextLevel();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Debugging
        {
            LoadNextLevel();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeIn;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FadeIn;
        GameEvents.RestartGame -= OnRestartGame;
        GameEvents.ResetGame -= ResetStage;
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
        string prefix = words[0];
        prefix += '_';
        if (prefix == levelNamePrefix || prefix == bossLevelNamePrefix)
        {
            int index = int.Parse(words[words.Length - 1]);
            StageManager.currentSceneIndex = index;
        }

        GameEvents.RestartGame += OnRestartGame;
        GameEvents.ResetGame += ResetStage;
    }

    private void OnRestartGame()
    {
        ResetStage();
        LoadScene(Scenes.Camp);
        KeyStatic.numberOfKeys = 0;
        SoulStatic.soul = 0;
    }

    private void ResetStage()
    {
        StageManager.currentStage = 1;
        StageManager.currentWorld = 1;
    }
}
