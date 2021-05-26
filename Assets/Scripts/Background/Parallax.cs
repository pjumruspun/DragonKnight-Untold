using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float parallaxEffect;
    private float backgroundLength;
    private float startPosition;
    private float distance;
    private float boundaryChecker;

    private void Start()
    {
        startPosition = transform.position.x;
        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // We will be using main camera
        Transform camTransform = Camera.main.transform;
        distance = (camTransform.position.x * parallaxEffect);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        // Extend the background when the camera is near the boundary
        boundaryChecker = (camTransform.position.x * (1 - parallaxEffect));

        if (boundaryChecker > startPosition + backgroundLength)
        {
            startPosition += backgroundLength;
        }
        else if (boundaryChecker < startPosition - backgroundLength)
        {
            startPosition -= backgroundLength;
        }
    }
}
