using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 2.0f)]
    private float timeScale = 1.0f;

    private void Update()
    {
        if (Mathf.Abs(Time.timeScale - timeScale) > 0.01f)
        {
            Time.timeScale = timeScale;
        }
    }
}
