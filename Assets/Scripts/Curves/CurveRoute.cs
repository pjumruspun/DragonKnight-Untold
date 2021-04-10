using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: Cubic Bezier Curve from Wiki https://en.wikipedia.org/wiki/B%C3%A9zier_curve
// Modified from this video: https://youtu.be/11ofnLOE8pw
public class CurveRoute : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;
    private Vector2 gizmosPosition;
    private Vector3 localEnd => controlPoints[3].localPosition;
    private Vector3 localStart => controlPoints[0].localPosition;

    public void Configure(Vector3 startingPosition, Vector3 endPosition, float curveness, bool curveDown = true, float timeOut = 1.0f)
    {
        transform.position = startingPosition;

        controlPoints[0].localPosition = Vector3.zero;
        controlPoints[3].localPosition = endPosition - startingPosition;

        controlPoints[1].localPosition = PerpendicularPosition(curveness, true);
        controlPoints[2].localPosition = PerpendicularPosition(curveness, true);

        CoroutineUtility.ExecDelay(() => Disable(), timeOut);
    }

    public void AssignBezierPath(GameObject spawnedObject, BezierConfig bezierConfig)
    {
        // spawnedObject.transform.SetParent(transform);
        BezierFollow bezierFollow = spawnedObject.AddComponent<BezierFollow>();
        bezierFollow.SetRoute(this);
        bezierFollow.Activate(bezierConfig);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition =
                Mathf.Pow(1 - t, 3) * controlPoints[0].position + // (1-t)^3 * P1
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + // 3(1-t)^2 * t * P2
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + // 3(1-t) * t^2 * P3
                Mathf.Pow(t, 3) * controlPoints[3].position; // t^3 * P4

            Gizmos.DrawSphere(gizmosPosition, 0.05f);
        }

        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y), new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y), new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }

    private Vector3 PerpendicularPosition(float amplitude, bool curveDown = true)
    {
        float startEndSlope = (localEnd.y - localStart.y) / (localEnd.x - localStart.x);

        // Curve down -> Y's slope always negative
        // Curve up -> Y's slop always positive
        float coefficientY = curveDown ? -1.0f : 1.0f;

        // X's slope follows startEndSlop, 
        float coefficientX = startEndSlope > 0.0f ? 1.0f : -1.0f;

        Vector3 perpendicularDirection = (new Vector3(coefficientX, coefficientY * Mathf.Abs(1.0f / startEndSlope))).normalized;
        float distance = Vector3.Distance(localEnd, localStart) * amplitude / 2.0f;
        return MidPoint() + distance * perpendicularDirection;
    }

    private Vector3 MidPoint()
    {
        return new Vector3(
            localStart.x + localEnd.x / 2.0f,
            localStart.y + localEnd.y / 2.0f,
            localStart.z + localEnd.z / 2.0f
        );
    }
}
