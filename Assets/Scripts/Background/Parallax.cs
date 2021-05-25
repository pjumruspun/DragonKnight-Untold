using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float backgroundLength;
    
    private float startPosition;
    
    private float distance;

    private float boundaryChecker;
    
    public GameObject Camera;
    
    public float parallaxEffect;

    void Start()
    {
        startPosition = transform.position.x;
        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        distance = (Camera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        // Extend the background when the camera is near the boundary
        boundaryChecker = (Camera.transform.position.x * (1 - parallaxEffect));

        if (boundaryChecker > startPosition + backgroundLength) {
            startPosition += backgroundLength;
        }
        else if (boundaryChecker < startPosition - backgroundLength) {
            startPosition -= backgroundLength;
        }

    }
}
