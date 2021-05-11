using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.groundLayerIndex && PlayerCombat.Instance.IsRushing)
        {
            ScreenShake.Instance.StartShaking(0.25f, 0.2f);
            EventPublisher.TriggerStopRush();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.groundLayerIndex)
        {
            // Debug.Log("Untouched ground!");
        }
    }
}
