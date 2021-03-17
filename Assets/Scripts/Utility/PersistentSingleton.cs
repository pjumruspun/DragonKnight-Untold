using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : MonoSingleton<T> where T : MonoBehaviour
{
    [SerializeField]
    private string managerTag = "GameManager";
    [SerializeField]
    private MonoSingleton<T>[] singletons;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void CreateGameManager()
    {
        GameObject manager = GameObject.FindGameObjectWithTag(managerTag);
        if (manager == null)
        {
            GameObject newManager = new GameObject();
            newManager.name = managerTag;
            newManager.tag = managerTag;
        }
    }
}
