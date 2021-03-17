using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script does nothing but float the object upward
public class Float : MonoBehaviour
{
    [SerializeField]
    private float floatingSpeed = 0.5f;

    private void FixedUpdate()
    {
        transform.position = transform.position + Vector3.up * floatingSpeed * Time.fixedDeltaTime;
    }
}
