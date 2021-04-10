using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveRoute : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;
    private Vector2 gizmosPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject spawnedArrow = ObjectManager.Instance.Arrows.SpawnObject();
            AssignBezierPath(spawnedArrow);
        }
    }

    private void AssignBezierPath(GameObject spawnedObject)
    {
        spawnedObject.transform.SetParent(transform.parent);
        BezierFollow bezierFollow = spawnedObject.AddComponent<BezierFollow>();
        bezierFollow.SetRoute(transform);
        bezierFollow.Activate();
    }

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            // Source: Cubic Bezier Curve from Wiki https://en.wikipedia.org/wiki/B%C3%A9zier_curve
            // Video: https://youtu.be/11ofnLOE8pw
            gizmosPosition =
                Mathf.Pow(1 - t, 3) * controlPoints[0].position + // (1-t)^3 * P1
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + // 3(1-t)^2 * t * P2
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + // 3(1-t) * t^2 * P3
                Mathf.Pow(t, 3) * controlPoints[3].position; // t^3 * P4

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y), new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y), new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }
}
