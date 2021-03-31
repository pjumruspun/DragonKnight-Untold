using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For camera panning and spawn area
public class Border : MonoSingleton<Border>
{
    public static float Top => Instance.topRightCorner.position.y;
    public static float Bottom => Instance.bottomLeftCorner.position.y;
    public static float Right => Instance.topRightCorner.position.x;
    public static float Left => Instance.bottomLeftCorner.position.x;

    private Transform topRightCorner;
    private Transform bottomLeftCorner;

    private void Start()
    {
        topRightCorner = GameObject.FindGameObjectWithTag(Tags.TopRightCorner).transform;
        bottomLeftCorner = GameObject.FindGameObjectWithTag(Tags.BottomLeftCorner).transform;

        if (topRightCorner == null)
        {
            throw new System.Exception($"Top right corner not found by FindGameObjectWithTag({Tags.TopRightCorner})");
        }

        if (bottomLeftCorner == null)
        {
            throw new System.Exception($"Bottom left corner not found by FindGameObjectWithTag({Tags.BottomLeftCorner})");
        }
    }
}
