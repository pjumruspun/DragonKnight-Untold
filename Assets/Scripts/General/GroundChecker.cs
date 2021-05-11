using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.groundLayerIndex)
        {
            Debug.Log("Touched ground!");
            EventPublisher.TriggerStopRush();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.groundLayerIndex)
        {
            Debug.Log("Untouched ground!");
        }
    }
}
