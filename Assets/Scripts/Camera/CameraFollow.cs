using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTransform;

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        if (followTransform == null)
        {
            Debug.LogAssertion("Error: followTransform is null");
            return;
        }

        float x = followTransform.position.x;
        float y = followTransform.position.y;
        float z = Camera.main.transform.position.z;
        Camera.main.transform.position = new Vector3(x, y, z);
    }
}
